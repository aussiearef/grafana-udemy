using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Net.Mime;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);
var metricCollectorUrl = builder.Configuration["OtelMetricCollector:Host"] ?? "";
var traceCollectorUrl = builder.Configuration["OtelTraceCollector:Host"] ?? "";

if (builder.Environment.IsProduction())
{
    traceCollectorUrl = traceCollectorUrl.Replace("localhost", "alloy"); // set to docker container name of Alloy
    metricCollectorUrl = metricCollectorUrl.Replace("localhost", "alloy");
    Console.WriteLine("Alloy address is set to http://alloy:4318/");
}

const string serviceName = "Order Service";
const string serviceVersion = "1.0.0";
var meterName = $"{serviceName}.meter";

builder.Services
    .AddHttpClient()
    .AddSingleton(TracerProvider.Default.GetTracer(serviceName))
    .AddMetrics()
    .AddOpenTelemetry()
    .WithMetrics(m =>
    {
        m.SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService(serviceName, serviceVersion: serviceVersion))
            .AddMeter(meterName)
            .AddOtlpExporter(o =>
            {
                o.Protocol = OtlpExportProtocol.HttpProtobuf;
                o.Endpoint = new Uri(metricCollectorUrl);
            })
            .AddConsoleExporter();
    })
    .WithTracing(t =>
    {
        t.AddSource(serviceName)
            .SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                    .AddService(serviceName, serviceVersion: serviceVersion))
            .AddAspNetCoreInstrumentation()
            .AddOtlpExporter(o =>
            {
                o.Protocol = OtlpExportProtocol.HttpProtobuf;
                o.Endpoint = new Uri(traceCollectorUrl);
            })
            .AddConsoleExporter();
    });


var app = builder.Build();


app.MapGet("/",
    async (Tracer tracer, IMeterFactory metricFactory, IHttpClientFactory httpClientFactory) =>
    {
        
        var isProduction = builder.Environment.IsProduction();
        
        #region Metric collection

        var meter = metricFactory.Create(new MeterOptions(meterName));
        var otlOrderCount = meter.CreateCounter<int>("otel_order");
        otlOrderCount.Add(1);

        #endregion

        #region trace

        using var httpSpan = tracer.StartActiveSpan("Making HTTP Call", SpanKind.Client);

        httpSpan.SetAttribute("comms", "api");
        httpSpan.SetAttribute("protocol", "http");
        httpSpan.SetStatus(Status.Ok);
       
        var paymentServiceUrl = isProduction ? "http://paymentservice:8080": "http://localhost:5001";
        var httpClient = httpClientFactory.CreateClient();
        var paymentRequest = new HttpRequestMessage(HttpMethod.Get, paymentServiceUrl);

        // Pass Trace Context to Payment Service
        var propagator = new TraceContextPropagator();

        var parentSpanContext = httpSpan.Context;
        var activity = Activity.Current?.SetParentId(parentSpanContext.TraceId, parentSpanContext.SpanId);

        if (activity != null)
        {
            var propagationContext = new PropagationContext(activity.Context, Baggage.Current);

            propagator.Inject(propagationContext, paymentRequest.Headers,
                (headers, name, value) => { headers.Add(name, value); });
        }

        // End passing trace context
        try
        {
            Console.WriteLine($"Calling Payment Service at {paymentServiceUrl}");
            await httpClient.SendAsync(paymentRequest);
        }
        catch
        {
            return "Run the Payment Service First.";
        }

        activity?.Stop();

        #endregion

        return "OK";
    });
app.Run();