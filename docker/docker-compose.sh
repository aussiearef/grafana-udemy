#!/bin/bash

set -e

echo "Preparing shared folders and downloading configuration files..."

# Create all necessary shared folders
if [ ! -d "shared" ]; then
mkdir -p shared/tempo
mkdir -p shared/prometheus
mkdir -p shared/loki/chunks
mkdir -p shared/loki/rules
mkdir -p shared/logs
mkdir -p shared/alloy
mkdir -p shared/grafana/provisioning/datasources
mkdir -p shared/grafana/provisioning/dashboards
mkdir -p shared/grafana/dashboards
fi


# Download Tempo configuration
curl -sSL -o shared/tempo/tempo.yml https://raw.githubusercontent.com/aussiearef/grafana-udemy/main/tempo/tempo.yml

# Download Prometheus configuration
curl -sSL -o shared/prometheus/prometheus.yml https://raw.githubusercontent.com/aussiearef/grafana-udemy/main/prometheus/prometheus.yml

# Download Alloy configuration
curl -sSL -o shared/alloy/config.alloy https://raw.githubusercontent.com/aussiearef/grafana-udemy/main/alloy/config.alloy

# Download Grafana dashboard and provisioning files
curl -sSL -o shared/grafana/dashboards/ShoeHub_Dashboard.json https://raw.githubusercontent.com/aussiearef/grafana-udemy/main/grafana/ShoeHub_Dashboard.json
curl -sSL -o shared/grafana/provisioning/dashboards/dashboards.yml https://raw.githubusercontent.com/aussiearef/grafana-udemy/main/grafana/dashboards.yml
curl -sSL -o shared/grafana/provisioning/datasources/datasources.yml https://raw.githubusercontent.com/aussiearef/grafana-udemy/main/grafana/datasources.yml

if [ ! -d "tempo-data" ]; then
# Download Loki configuration
mkdir tempo-data 
mkdir -p ./tempo-data/traces
mkdir -p ./tempo-data/wal   
fi

# Set correct permissions
chmod -R 755 shared
chmod -R 777 tempo-data


echo "All files downloaded and permissions set."
echo "You can now run: docker-compose up -d"

docker-compose -f docker-compose.yaml up -d