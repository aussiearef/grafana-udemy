<!--
  Title: Online Course: Grafana, Prometheus, Tempo and Loki from Beginner to Advanced
  Description: A comprehensive online course about Grafana, Alloy, Tempo, Prometheus and Loki
  Author: Aref Karimi
  -->
  
# Grafana from Beginner to Advanced
## Learn Grafana, Prometheus, Loki, Alloy, and Tempo and become an expert!

![Grafana](https://img-c.udemycdn.com/course/750x422/1473698_386a_11.jpg)

This repository belongs to Grafana from Beginner to Advanced [](https://www.udemy.com/course/grafana-graphite-and-statsd-visualize-metrics/?referralCode=F9360D03CB430529BEAD)

The course covers the installation and use of Grafana, Prometheus, Loki, and Tempo using their Open-Source (OSS) versions.. It also includes integrations with various databases such as InfluxDB, MySQL, Elasticsearch, SQL Server, and Prometheus.

## Microservices for Opentelemetry

The code and binary files of the Microservice(s) demonstrating how OpenTelemetry signals are produced and pushed to Grafana Alloy are located in the Microservices folder (i.e., Microservices/OrderService). The code is written in C# and .NET, but you don't need to know any coding.

You don't need to compile the code; you can download the binary files from the Microservices/releases/* folder. These binary files assume that Grafana Alloy is available locally. To change its address, open the appSettings.json file and update the URL accordingly.

## Setup Full Grafana Stack and Prometheus Locally, in Seconds!

I have provided the learners with a "docker-compose" file to launch the Grafana Stack and Prometheus locally using Docker Desktop. After the launch, you will have access to:
- Grafana
- ShowHub mock data generator
- Prometheus
- Grafana Loki
- Grafana Tempo
- Grafana Alloy
- Mock microservices to generate traces (for Alloy and Tempo)

To launch the stack:
- Download the [docker-compose file](https://github.com/aussiearef/grafana-udemy/blob/6f36ffd6413015b342a83ce130e5ce04a5e9cd78/docker/docker-compose.yaml)  (or clone the entire repository)
- Copy the docker-compose file into a folder where the current logged-in user has write access. Do not place docker-compose.yml file in the root directory!
- run ``` docker compose up -d ```
- Access http://localhost:3000 to visit Grafana.
- Prometheus, Loki, and Tempo are added automatically as Grafana Datasources.
- A sample dashboard is added automatically.
  
Mock data, including the metrics from the Shoe Hub company, OpenTelemetry metrics, and traces from microservices, is available and can be seen via Drill Down, Explorer or Panels that link to Prometheus and/or Tempo datasources.

You will not need to do anything other than focus on your learning! If you launch your local Grafana Stack and Prometheus, everything is set up automatically using the provided Docker Compose file.


## More Free Courses on YouTube

[![YouTube](https://img.shields.io/badge/YouTube-Subscribe-red?style=flat&logo=youtube)](http://www.youtube.com/@FreeTechnologyLectures)

Subscribe to the Free Technology and Technology Management Courses channel for free lectures about Coding, DevOps, and Technology Management. [Here is the link to the YouTube channel](http://www.youtube.com/@FreeTechnologyLectures).


