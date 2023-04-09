# minikube start --cni calico

# Set Docker environment for minikube

echo "Setting Docker env..."
minikube docker-env | Invoke-Expression
echo $(minikube docker-env)

# Make sure you start Docker Desktop and minikube first (`minikube start`)

echo "Entering public API and building..."
cd Dummy.PublicAPI
docker build -t greetings-api  -f Dockerfile .

echo "Entering server and building..."
cd ../Dummy.Server
docker build -t greetings-service -f Dockerfile .

cd ..

# Database
echo "Deploying database..."
kubectl apply -f ./Dummy.Database/deployment.yaml 
kubectl apply -f ./Dummy.Database/service.yaml 
kubectl apply -f ./Dummy.Database/network-policy.yaml 

# The Dummy.Server service will fail to start as the MySQL
# connection is not immediately available. K8S will eventually
# heal it, retrying until the database is up. 

# Server
echo "Deploying server..."
kubectl apply -f ./Dummy.Server/deployment.yaml 
kubectl apply -f ./Dummy.Server/service.yaml

# Client
echo "Deploying public API..."
kubectl apply -f ./Dummy.PublicAPI/deployment.yaml
kubectl apply -f ./Dummy.PublicAPI/service.yaml


# Restart to make sure we have updates, due to ImagePullPolicy:
# Because these are local images we never want to pull, which 
# inconveniently means that if we update them, the deployments
# will still carry on using the old version until restarted.
kubectl rollout restart deployment/greetings-service
kubectl rollout restart deployment/greetings-api

echo "Done! :-)"
