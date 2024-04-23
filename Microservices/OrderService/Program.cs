using System.Diagnostics;
using System.Diagnostics.Metrics;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);
var metricCollectorUrl = builder.Configuration["OtelMetricCollector:Host"] ?? "";
var traceCollectorUrl = builder.Configuration["OtelTraceCollector:Host"] ?? "";

builder.Services
    .AddMetrics()
    .AddOpenTelemetry()
    .ConfigureResource(r =>
    {
        r.AddService("OrderService");
    })
    .WithMetrics(m =>
    {
        m.AddMeter("OrderServiceMeter")
            .AddOtlpExporter(o =>
            {
                o.Protocol = OtlpExportProtocol.HttpProtobuf;
                o.Endpoint = new Uri(metricCollectorUrl);
            })
            .AddConsoleExporter();

    })
    .WithTracing(t =>
    {
        t.AddSource("Order Service APIs")
            .AddConsoleExporter()
            .AddOtlpExporter(o =>
            {
                o.Protocol = OtlpExportProtocol.HttpProtobuf;
                o.Endpoint = new Uri(traceCollectorUrl);
            });
    });


var app = builder.Build();

Counter<int>? otlOrderCount = null;

ActivitySource.AddActivityListener(new ActivityListener()
{
    ActivityStarted = r =>
    {
        Console.WriteLine($"Activity {r.DisplayName} started.");
    }
});
app.MapGet("/", async (context) =>
{
    using var activity = Activity.Current?.Start();
    activity.SetTag("Service", "Order");
    activity.DisplayName = "Order Processing";

    var metricFactory = context.RequestServices.GetService<IMeterFactory>();
    var meter = metricFactory?.Create(new MeterOptions("OrderServiceMeter"));
    otlOrderCount = meter?.CreateCounter<int>("otel_order");

    otlOrderCount?.Add(1);

    using (var step1Activity = activity.Start()) // Create child spans within the root activity
    {
        step1Activity.SetTag("Step", "Get Customer Info.");
        step1Activity.DisplayName = "Customer Info";
        step1Activity.SetStatus(ActivityStatusCode.Ok);
            
    }

    using (var step2Activity = activity.Start())
    {
        step2Activity.SetTag("Step", "Get Shipping Info.");
        step2Activity.DisplayName = "Shipping Info";
        step2Activity.SetStatus(ActivityStatusCode.Ok);
    }

    activity.SetStatus(ActivityStatusCode.Ok); // Set status on root activity
});
app.Run();
