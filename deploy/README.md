# Deploy hướng dẫn

## Build Docker image

```shell
docker build -t kudorivis/cleanarch-api:latest -f src/CleanArchExample.API/Dockerfile .
docker push kudorivis/cleanarch-api:latest
```

## Deploy lên Kubernetes

```shell
kubectl apply -f deploy/k8s/deployment.yaml
kubectl apply -f deploy/k8s/service.yaml
kubectl apply -f deploy/k8s/ingress.yaml # nếu cần
```