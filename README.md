# Getting started

1. Install Docker and Minikube

2. Start Minikube (`minikube start`)

3. Deploy (run `deploy.ps`)

4. Expose the minikube service on the local machine `minikube service greetings-api --url`

This returns something like `http://127.0.0.1:61175` (the port will vary).

5. Go to the URL indicated in a browser. You should see a message. Try `http://127.0.0.1:61175/greet/<name>`.

6. [Optional] Stop and clean up (run `stop.ps`)