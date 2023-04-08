#!/bin/bash

# Make sure you start Docker Desktop and minikube first (`minikube start`)
cd Dummy.Server && \
docker build -t greetings-service -f Dockerfile . && \
kubectl apply -f ./Dummy.Server/deployment.yaml && \
kubectl apply -f ./Dummy.Server/service.yaml
