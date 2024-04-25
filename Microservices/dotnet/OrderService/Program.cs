using System.Diagnostics.Metrics;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);
var metricCollectorUrl = builder.Configuration["OtelMetricCollector:Host"] ?? "";
var traceCollectorUrl = builder.Configuration["OtelTraceCollector:Host"] ?? "";

const string serviceName = "Order Service";
const string serviceVersion = "1.0.0";
var meterName = $"{serviceName}.meter";

builder.Services
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


app.MapGet("/", (HttpContext context, Tracer tracer, IMeterFactory metricFactory) =>
{
    #region Metric collection

    var meter = metricFactory?.Create(new MeterOptions(meterName));
    var otlOrderCount = meter?.CreateCounter<int>("otel_order");
    otlOrderCount?.Add(1);

    #endregion

    #region trace

    using var dbSpan = tracer.StartActiveSpan("Connecting to DB");

    try
    {
        dbSpan.SetAttribute("db-name", "prod-sql");
        dbSpan.SetAttribute("connection-status", "success");
        dbSpan.SetAttribute("Query result count", "1");
        dbSpan.SetStatus(Status.Ok);
    }
    catch (Exception e)
    {
        dbSpan.SetStatus(Status.Error);
        dbSpan.RecordException(e);
        Console.WriteLine(e);
        // throw ?
    }

    #endregion

    return "OK";
});
app.Run();