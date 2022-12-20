# ArgoCD
ArgoCD follows the GitOps pattern of using Git repositories as the source of truth for defining the desired application state. In other words, the ArgoCD automatically synchronize the cluster to the desired state defined in a Git repository.

Each workload is defined declarative through a resource manifest in a YAML file. Argo CD checks if the state defined in the Git repository matches what is running on the cluster and synchronizes it if changes were detected.
## Installation

### Create namespace
```
kubectl apply -f argo-01-namespace.yaml
```
### Deployment, services, and the other stuff...
```
kubectl apply -f argo-02-deployment.yaml
```
Alternative you can run the file published here (the above command is just using a copy of the official one):
```
kubectl apply -n argocd -f https://raw.githubusercontent.com/argoproj/argo-cd/master/manifests/install.yaml
```

### Port forward manually
This how to manually do it, but if you could also jump over to the **Login and start using ArgoCD** step and use PowerCommands client to start the port forward, grab the initial password and open up the ArgoCD UI.
```
kubectl port-forward svc/argocd-server -n argocd 8080:443 
```
### Get the initial secret manually
```
kubectl -n argocd get secret argocd-initial-admin-secret -o jsonpath="{.data.password}"
```
This will give you the password base64 encoded, you need do decode it with Linux command or C# or whatever you prefer. You could use the [PowerCommandsClient](../PowerCommandsClient/) command:
```
base64 --decode <encoded-password>
```
Next step explains how you could to this with one simple PowerCommand (but you must of course learn the manual way also, so that you understand what happens under the hood).

## Login and start using ArgoCD
Easiest way is to start the [PowerCommandsClient](../PowerCommandsClient/) and run the argocd command.
```
argocd
```
This will start the port forwarding console, it will decode the initial password that you need to login with the username **admin** and it will also startup the ArgoCD UI in your browser. 

### Add your repo
Navigate to settings -> Repositories and choose **+Connect Repo**
Easiest is to connect to a public Github repo using ** Via https** method (you point out a directory later)

### Add an application
Use the **+New App** button, fill in the necessary info, the path to the directory you want to sync, project name could be **default** sync policy **manual** cluster URL **https://kubernetes.default.svc** (it is a predefined value).

### Sync
And then you will have your first ArgoCD application, as in this example, using the configuration files for the **[Persistent storage tutorial](../src/persistent-storage/)** tutorial.

![Alt text](images/tool_argocd_1.png?raw=true "Argo CD screenshot")

**Special thanks to BogoToBogo**

I have used this [tutorial](https://www.bogotobogo.com/DevOps/Docker/Docker_Kubernetes_ArgoCD_on_Kubernetes_cluster.php) when writing this.

