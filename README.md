# Getting started

1. Install minikube

2. Start minikube (`minikube start`)

3. Deploy server (run `deploy-server.sh`)

4. Deploy client (run `deploy.client-sh`)

You should see request/response pairs every 2.5s in client/server logs (either use `kubectl logs <pod_name>` or an IDE like Lens).