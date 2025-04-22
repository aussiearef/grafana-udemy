# Open Telemetry (Metric Only) Sample Micro Service

The files in this directory belong to the online course:

**[Mastering Prometheus with Grafana (Including Loki and Alloy)](https://www.udemy.com/course/mastering-prometheus-and-grafana/?referralCode=C929F0178B24DAD1F809)**

## 📦 Purpose

The application provided in this directory is a pre-built executable that sends sample metrics to Grafana Alloy using **OpenTelemetry (OTel)**.

This is ideal for testing Grafana dashboards, exploring OTel pipelines, and learning how Prometheus metrics flow through modern observability stacks.

## ⬇️ Download Instructions

Download the release that matches your operating system:

- Windows 64-bit
- Mac (Intel) 64-bit (`osx-x64`)
- Mac (Apple Silicon) 64-bit (`osx-arm64`)

Each release contains a compiled binary (OrderService) and a sample configuration file.


## ⚙️ Configuration

After downloading, open the included `appSettings.json` file and verify that the Alloy port is set correctly.

Example:

```json
"OtelMetricCollector": {
  "Host": "http://localhost:4318/v1/metrics"
}
```

In the above example, metrics are sent to an Alloy instance running on your local machine, using the default OTel port 4318.


