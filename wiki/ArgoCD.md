# ArgoCD
ArgoCD follows the GitOps pattern of using Git repositories as the source of truth for defining the desired application state. In other words, the ArgoCD automatically synchronize the cluster to the desired state defined in a Git repository.

Each workload is defined declarative through a resource manifest in a YAML file. Argo CD checks if the state defined in the Git repository matches what is running on the cluster and synchronizes it if changes were detected.
## Installation

### Create namespace
```
kubectl apply -f argo-01-namespace.yaml
```
### The rest
```
kubectl apply -f argo-02-deployment.yaml
```
Alternative you can run the file published here (the above command is just using a copy of the official one):
```
kubectl apply -n argocd -f https://raw.githubusercontent.com/argoproj/argo-cd/master/manifests/install.yaml
```

### Port forward to http://localhost:8080
```
kubectl port-forward svc/argocd-server -n argocd 8080:443 
```

## Login and start using ArgoCD

