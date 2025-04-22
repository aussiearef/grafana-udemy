# Sample Microservices: Order Service & Payment Service

This directory contains pre-built executables for two sample microservices:

- **Order Service**
- **Payment Service**

These applications are designed to send **traces** to **Grafana Alloy**, which forwards them to **Grafana Tempo** for distributed tracing.

---

## 📥 Download

To run these microservices, download the executables from the appropriate subdirectory. Releases are available for the following operating systems:

- Windows x64
- macOS (Intel CPU) – `osx-x64`
- macOS (Apple M1/M2 CPU) – `osx-arm64`
- Generic Linux (Intel CPU) – `linux-x64`

---

## 📂 Included Services

Each release contains the following:

- `OrderService` binary
- `PaymentService` binary
- `appSettings.json` configuration file (one per service)

---

## ⚙️ Configuration Instructions

Before running the microservices, you must update the `appSettings.json` file in each directory to ensure that Alloy's port settings for metrics and traces are correct.

Example configuration:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Debug"
    }
  },
  "OtelMetricCollector": {
    "Host": "http://localhost:4318/v1/metrics"
  },
  "OtelTraceCollector": {
    "Host": "http://localhost:4318/v1/traces"
  },
  "AllowedHosts": "*"
}
```

In the example above:

Metrics are sent to http://localhost:4318/v1/metrics

Traces are sent to http://localhost:4318/v1/traces

These are the default OpenTelemetry ports used by Grafana Alloy. If your Alloy instance uses different ports or a remote address, ensure that you update these values accordingly.

Alternatively, you can stand up an entire Grafana and Prometheus stack using Docker.

- Ensure that Docker Desktop is installed on your computer.
- Download the dockercompose.yml file from [HERE](https://github.com/aussiearef/grafana-udemy/tree/4e6875dd314bc3551c33d8b0bd018eaa27fd5230/docker).
- Make sure the dockercompose.yml file is not in the root directory, otherwise your "docker compose" command will fail with a "permission denied" message. Copy the file to /home or /tempo or /shared or similar directories.
- run "docker compose up -d"
- Once the docker compose command is completed, visit Grafana via HTTP://localhost:3000
- Use "admin" for both username and password.
  
