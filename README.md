# Getting started

1. Install Docker and Minikube

2. Start Minikube with Calico, required for Network Policies to take effect (`minikube start --cni calico`)

3. Deploy (run `deploy.ps1`)

4. Expose the minikube service on the local machine `minikube service greetings-api --url`

This returns something like `http://127.0.0.1:61175` (the port will vary).

5. Go to the URL indicated in a browser. You should see a message. Try `http://127.0.0.1:61175/greet/<name>`.

6. Prove that the Network Policy is effective

First, check that the backend service can connect to the database: `http://127.0.0.1:61175/greet/database`. You should see a special greeting message.

Now check that the frontend (public API) service cannot connect directly to the database: `http://127.0.0.1:61175/database`. After 10 seconds or so, you should see a timeout.

6. [Optional] Stop and clean up (run `stop.ps1`)