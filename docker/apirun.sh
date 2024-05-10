#!/bin/bash

while true; do
    curl -sSf http://orderservice:8080
    curl -sSf http://paymentservice:8080
    sleep 5
done
