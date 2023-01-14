# ArgoCD
ArgoCD follows the GitOps pattern of using Git repositories as the source of truth for defining the desired application state. In other words, the ArgoCD automatically synchronize the cluster to the desired state defined in a Git repository.

Each workload is defined declarative through a resource manifest in a YAML file. Argo CD checks if the state defined in the Git repository matches what is running on the cluster and synchronizes it if changes were detected.
## Installation

### Create namespace
```
kubectl apply -f argocd-01-namespace.yaml
```
### Change namespace of the current context
Next step is to change the namespace to the newly created namespace named argo cd.
```
kubectl config set-context --current --namespace=argocd
```
### Deployment, services, and the other stuff...
```
kubectl apply -f argocd-02-deployment.yaml
```
Alternative you can run the file published here (the above command is just using a copy of the official one):
```
kubectl apply -n argocd -f https://raw.githubusercontent.com/argoproj/argo-cd/master/manifests/install.yaml
```
Before we jump in to the next step we need to check that the pods are up and running, it could take a while before they are, run this command to check this.
```
kubectl get pods
```
This is how the result should look like:
```
NAME                                                READY   STATUS    RESTARTS   AGE
argocd-application-controller-0                     1/1     Running   0          24s
argocd-applicationset-controller-5c7cfb9c5f-kps8d   1/1     Running   0          24s
argocd-dex-server-74cc9c9f78-5lhl2                  1/1     Running   0          24s
argocd-notifications-controller-56bd7f7f9d-bzvbl    1/1     Running   0          24s
argocd-redis-79c755c747-7582l                       1/1     Running   0          24s
argocd-repo-server-65c5b7899b-lhmw6                 1/1     Running   0          24s
argocd-server-7db799b589-s8x5g                      0/1     Running   0          24s
```
### Port forward
```
kubectl port-forward svc/argocd-server -n argocd 8080:443 
```
### Get the initial secret
```
kubectl -n argocd get secret argocd-initial-admin-secret -o jsonpath="{.data.password}"
```
This will give you the password base64 encoded, you need do decode it with Linux command or C# or whatever you prefer. You could use the [PowerCommandsClient](../PowerCommandsClient/) command:
```
base64 --decode <encoded-password>
```
### Open the browser and login
Open [https://localhost:8080/](https://localhost:8080/)

Login with username **admin** and the decoded password.

### Add your repo
Navigate to settings -> Repositories and choose **+Connect Repo**
Easiest is to connect to a public Github repo, why not use this repo ```https://github.com/PowerCommands/KubernetesTutorials``` KubernetesTutorials using **Via https** method (you point out a directory later)

![Alt text](images/argocd_connect_repo.png?raw=true "Argo CD add repository screenshot")


### Add an application
Use the **+New App** button, fill in the necessary info, witch is the path to the directory you want to sync, project name could be **default** sync policy **manual** cluster URL **https://kubernetes.default.svc** (it is a predefined value). If you have connected tho this repo the path to the SQL database manifest files is **manifests/persistent-storage**.

![Alt text](images/argocd_app.png?raw=true "Argo CD create app screenshot")

### Sync
And then you will have your first ArgoCD application, as in this example, using the configuration files for the **[Persistent storage tutorial](../src/persistent-storage/)** tutorial.

![Alt text](images/tool_argocd_1.png?raw=true "Argo CD screenshot")

**Special thanks to BogoToBogo**

I have used this [tutorial](https://www.bogotobogo.com/DevOps/Docker/Docker_Kubernetes_ArgoCD_on_Kubernetes_cluster.php) when writing this.

