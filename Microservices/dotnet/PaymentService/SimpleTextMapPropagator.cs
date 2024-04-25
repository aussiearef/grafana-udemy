using System.Diagnostics;
using OpenTelemetry.Context.Propagation;

namespace PaymentService;

public class SimpleTextMapPropagator
{
    public ActivityContext ExtractActivityContext<T>(PropagationContext context, T carrier,
        Func<T, string, IEnumerable<string>> getter)
    {
        // 00-0af7651916cd43dd8448eb211c80319c-00f067aa0ba902b7-01

        var traceparent = getter(carrier, "traceparent")?.FirstOrDefault();
        if (traceparent == null) return default;

        var traceId = ActivityTraceId.CreateFromString(traceparent.Substring(3, 32).AsSpan());
        var spanId = ActivitySpanId.CreateFromString(traceparent.Substring(36, 16).AsSpan());

        var activityContext = new ActivityContext(traceId, spanId, ActivityTraceFlags.None);
        return activityContext;
    }
}