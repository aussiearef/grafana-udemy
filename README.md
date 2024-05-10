<!--
  Title: Online Course: Grafana, Prometheus and Loki from Beginner to Advanced
  Description: A comprehensive online course about Grafana, Graphite, Prometheus and Loki
  Author: Aref Karimi
  -->
  
# Grafana from Beginner to Advanced
## Learn Grafana, Graphite, Prometheus and Loki

![Grafana](https://img-c.udemycdn.com/course/750x422/1473698_386a_11.jpg)

This repository belongs to Grafana from Beginner to Advanced [](https://www.udemy.com/course/grafana-graphite-and-statsd-visualize-metrics/?referralCode=F9360D03CB430529BEAD)

The course covers installing and using Grafana, Graphite, and Loki on-prem and in the cloud. It also includes integrations with various databases such as InfluxDB, MySQL, Elasticsearch, SQL Server, and Prometheus.

## Microservices for Opentelemetry

The code and binary files of Microservice(s) demonstrating how Opentelemetry signals are produced and pushed to Grafana Alloy are in /the Microservices folder (i.e., Microservices/OrderService). The code is written in C# and .NET.

You don't have to compile the code; you can download and unzip the binary files under the Microservices/releases/* folder. These binary files assume that Grafana Alloy is available locally. To change the address of Grafana Alloy, open the appSettings.json file and change the URL of Grafana Alloy there.

## Setup Full Grafana Stack and Prometheus Locally, in Seconds!

I have provided the learners with a "docker-compose" filet to launch Grafana Stack and Prometheus locally using Docker Desktop. After the launch, you will have access to:
- Grafana
- ShowHub mock data generator
- Prometheus
- Grafana Loki
- Grafana Tempo
- Grafana Alloy
- Mock microservices to generate metrics and traces (for Alloy and Tempo)

To launch the stack:
- Download the docker-compose file https://github.com/aussiearef/grafana-udemy/blob/6f36ffd6413015b342a83ce130e5ce04a5e9cd78/docker/docker-compose.yaml (or clone the entire repository)
- Copy the docker-compose file into a folder where the current logged-in user has write access.
- run ``` docker compose up -d ```
- Access http://localhost:3001 to visit Grafana.
- Prometheus, Loki and Tempo are added automatically as Grafana Datasources.
- Mock data, including the metrics from Shoe Hub company and OpenTelemetry metrics and traces from microservices, are available.

You will not need to do anything other than focus on your learning! Everything is setup automatically if you launch your local Grafana Stack and Prometheus using the provided docker-compose file.


## More Free Courses on YouTube

[![YouTube](https://img.shields.io/badge/YouTube-Subscribe-red?style=flat&logo=youtube)](http://www.youtube.com/@FreeTechnologyLectures)

Subscribe to the Free Technology and Technology Management Courses channel for free lectures about Coding, DevOps, and Technology Management. [Here is the link to the YouTube channel](http://www.youtube.com/@FreeTechnologyLectures).


## Buy me a coffee ☕

If you find my work helpful, consider treating me by buying coffee!

<a href="https://ko-fi.com/arefkarimi"><img src="https://storage.ko-fi.com/cdn/kofi2.png?v=3" alt="ko-fi" height="36"></a>
