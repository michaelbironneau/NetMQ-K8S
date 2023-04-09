# minikube start

# Set Docker environment for minikube

echo "Setting Docker env..."
minikube docker-env | Invoke-Expression
echo $(minikube docker-env)

# Make sure you start Docker Desktop and minikube first (`minikube start`)

echo "Entering client and building..."
cd Dummy.Client
docker build -t greetings-client  -f Dockerfile .

echo "Entering server and building..."
cd ../Dummy.Server
docker build -t greetings-service -f Dockerfile .

cd ..

# Server
echo "Deploying server..."
kubectl apply -f ./Dummy.Server/deployment.yaml 
kubectl apply -f ./Dummy.Server/service.yaml


echo "Sleeping for 5s to allow service time to deploy"
sleep 5

# Client
echo "Deploying client..."
kubectl apply -f ./Dummy.Client/deployment.yaml


# Restart to make sure we have updates, due to ImagePullPolicy:
# Because these are local images we never want to pull, which 
# inconveniently means that if we update them, the deployments
# will still carry on using the old version until restarted.
kubectl rollout restart deployment/greetings-service
kubectl rollout restart deployment/greetings-client

echo "Done! :-)"
