#!/bin/bash

# Make sure you start Docker Desktop and minikube first (`minikube start`)
cd Dummy.Client && \
docker build -t greetings-client  -f Dockerfile . && \
kubectl apply -f ./Dummy.Client/deployment.yaml && \
kubectl apply -f ./Dummy.Client/service.yaml
