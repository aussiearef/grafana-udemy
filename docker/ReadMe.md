
This directory includes a "docker compose" file that lets you stand up an entire Grafana Stack (Grafana, Alloy, Loki, and Tempo) alongside Prometheus. Metrics from the ShoeHub application and traces from the example microservices (OrderService and PaymentService) are automatically sent to Prometheus and Tempo. A sample dashboard is also provided.

- Ensure that Docker Desktop is installed on your computer.
- Download the ``dockercompose.yml`` and ``docker-compose.sh`` file from HERE.
- Make sure the ``dockercompose.yml`` file is not in the root directory, otherwise your "docker compose" command will fail with a "permission denied" message. Copy the file to /home or /temp,/shared,, or similar directories.
- run ``./docker-compose.sh``
- Once the script is run successfully, visit Grafana via ``HTTP://localhost:3000``
- Use "admin" for both username and password.
