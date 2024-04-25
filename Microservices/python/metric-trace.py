from fastapi import FastAPI, HTTPException
from opentelemetry import trace
from opentelemetry.sdk.metrics import MeterProvider
from opentelemetry.sdk.metrics.export import ConsoleMetricsExporter
from opentelemetry.exporter.otlp.proto.grpc.trace_exporter import OTLPSpanExporter
from opentelemetry.instrumentation.fastapi import FastAPIInstrumentor
from opentelemetry.sdk.resources import Resource
from opentelemetry.sdk.trace import TracerProvider
from opentelemetry.sdk.trace.export import BatchExportSpanProcessor

app = FastAPI()

# Configuring OTLP Exporter for tracing
trace.set_tracer_provider(TracerProvider(resource=Resource.create({"service.name": "Order Service"})))
tracer = trace.get_tracer(__name__)
span_exporter = OTLPSpanExporter(endpoint="http://trace-collector-host:55680")
span_processor = BatchExportSpanProcessor(span_exporter)
trace.get_tracer_provider().add_span_processor(span_processor)

# Configuring Meter Provider for metrics
meter_provider = MeterProvider(resource=Resource.create({"service.name": "Order Service"}))
meter = meter_provider.get_meter(__name__)
counter = meter.create_counter(name="otel_order", description="Count of orders")

@app.get('/')
async def read_root():
    with tracer.start_as_current_span("Connecting to DB"):
        try:
            # Simulate database connection
            # Set attributes
            tracer.current_span.set_attribute("db-name", "prod-sql")
            tracer.current_span.set_attribute("connection-status", "success")
            tracer.current_span.set_attribute("Query result count", "1")
            tracer.current_span.set_status(trace.Status.OK)
        except Exception as e:
            # Handle exceptions
            tracer.current_span.set_status(trace.Status.ERROR)
            tracer.current_span.record_exception(e)
            print(e)
    counter.add(1)
    return {"message": "OK"}

if __name__ == "__main__":
    FastAPIInstrumentor.instrument_app(app)
