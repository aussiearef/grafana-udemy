using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using PaymentService;

var builder = WebApplication.CreateBuilder(args);
var traceCollectorUrl = builder.Configuration["OtelTraceCollector:Host"] ?? "";

if (builder.Environment.IsProduction())
{
    traceCollectorUrl = traceCollectorUrl.Replace("localhost", "alloy"); // set to docker container name of Alloy
    Console.WriteLine($"Alloy address is set to {traceCollectorUrl}");
}

const string serviceName = "Payment Service";
const string serviceVersion = "1.0.0";

builder.Services
    .AddSingleton(TracerProvider.Default.GetTracer(serviceName))
    .AddOpenTelemetry()
    .WithTracing(t =>
    {
        t.AddSource(serviceName)
            .SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                    .AddService(serviceName, serviceVersion: serviceVersion)
            )
            .AddOtlpExporter(o =>
            {
                o.Protocol = OtlpExportProtocol.HttpProtobuf;
                o.Endpoint = new Uri(traceCollectorUrl);
            })
            .AddAspNetCoreInstrumentation()
            .AddConsoleExporter()
            .SetSampler<AlwaysOnSampler>();
    });


var app = builder.Build();

// Remove all registered propagators to avoid conflicts.
var propagators = new List<TextMapPropagator>();
var compositePropagator = new CompositeTextMapPropagator(propagators);
Sdk.SetDefaultTextMapPropagator(compositePropagator);

app.MapGet("/", (HttpContext httpContext, Tracer tracer) =>
{
    var carrierContextPropagator = new SimpleTextMapPropagator();

    var propagationContext =
        carrierContextPropagator.ExtractActivityContext(default, httpContext.Request.Headers,
            (headers, name) => headers[name]);

    var spanContext = new SpanContext(propagationContext);
    using var paymentSpan = tracer.StartActiveSpan("PaymentProcessing", SpanKind.Server,
        spanContext);

    paymentSpan.SetAttribute("db-name", "prod-sql");
    paymentSpan.SetAttribute("connection-status", "success");
    paymentSpan.SetAttribute("Query result count", "1");
    paymentSpan.SetStatus(Status.Ok);


    return "OK";
});


app.Run();