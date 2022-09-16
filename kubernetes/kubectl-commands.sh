#!/bin/bash

kubectl create secret generic secret-appsettings --from-file=./appsettings.secrets.json
kubectl create configmap config-appsettings --from-file=./appsettings.common.json

kubectl apply -f ./deployment.yaml
kubectl apply -f ./service.yaml
