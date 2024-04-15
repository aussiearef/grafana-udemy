using System.Diagnostics.Metrics;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

var builder = WebApplication.CreateBuilder(args);
var collectorUrl = builder.Configuration["OtelCollector:Host"]?? "http://localhost:4318/v1/metrics";

builder.Services
    .AddMetrics()
    .AddOpenTelemetry()
    
    .WithMetrics(m =>
{
    m.SetResourceBuilder(ResourceBuilder.CreateDefault()
            .AddService("OrderService"))
        .AddMeter("OrderServiceMeter")
        .AddInstrumentation<HttpClient>()
        .AddOtlpExporter(o =>
        {
            o.Protocol = OtlpExportProtocol.HttpProtobuf;
            o.Endpoint = new Uri(collectorUrl);
        })
        .AddConsoleExporter();
    
});


var app = builder.Build();


Counter<int> otelOrderCount= null;

app.Use(async (context, next) =>
{
    var metricFactory = context.RequestServices.GetService<IMeterFactory>();
    var meter = metricFactory?.Create(new MeterOptions("OrderServiceMeter"));
    otelOrderCount = meter.CreateCounter<int>("otel_order");
    otelOrderCount?.Add(1);
    await next();
});

app.MapGet("/", () =>
{
    otelOrderCount?.Add(1);
    return "OK";
});

app.Run();

