# Deploy a simple web application
This example will guide you trough the steps to deploy one webb application in one pod on a Docker Desktop K8:s kluster. As the Docker Desktop K8:s environment only consist of one kluster already prepared for you, the creation of a K8:s kluster is omitted. 

## Deployment
### Get the nodes of our K8:s kluster, just to know that your kubernetes kluster is up and running.

``` 
kubectl get nodes
```
Should return something like this on a fresh install.
``` 
NAME             STATUS   ROLES           AGE   VERSION
docker-desktop   Ready    control-plane   22h   v1.25.2
``` 
### Create a deployment
``` 
kubectl create deployment kubernetes-bootcamp --image=bkimminich/juice-shop:latest
``` 
**View your result**
``` 
kubectl get deployments
``` 
``` 
NAME                  READY   UP-TO-DATE   AVAILABLE   AGE
kubernetes-bootcamp   0/1     1            0           7s
``` 
### Create a service
You now have a pod running, you can check that out with 
``` 
kubectl get pods
``` 
``` 
NAME                                   READY   STATUS    RESTARTS   AGE
kubernetes-bootcamp-6b7cccd9c4-ftljv   1/1     Running   0          3m38s
``` 
To expose your app you need a K8:s to ge a IP address reachable outside the K8:s kluster.
We create a new service like this:
``` 
kubectl expose deployment/kubernetes-bootcamp --type="NodePort" --port 3000
```
Check out your new service.
``` 
kubectl get services
```
``` 
NAME                  TYPE        CLUSTER-IP      EXTERNAL-IP   PORT(S)          AGE
kubernetes            ClusterIP   10.96.0.1       <none>        443/TCP          22h
kubernetes-bootcamp   NodePort    10.103.59.168   <none>        3000:30460/TCP   111s
``` 
Open this URL in your browser: http://localhost:30460
If it´s not working, check the port number after 3000: it may not be the same as this example.

You shoud now see the famous OWASP Juice shop!

![Alt text](images/tutorial_1_1.png?raw=true "OWASP Juice Shop")

Congratulations you have now deployed your first K8:s application and made it reachable outside your K8:s kluster.

### Tips if you want to create files (called manifest) you can add --output="yaml" like this:
```
kubectl create deployment kubernetes-bootcamp --image=bkimminich/juice-shop:latest --output="yaml"
```
This manifest file could be saved and use to recreate your application on a different kluster. With this command:
```
kubectl apply -f bootcamp-deployment.yaml
```
And for the service.
```
kubectl apply -f bootcamp-service.yaml
```
### One more tip, later on when you could try to run this files automatically with the  [Power Kubernetes Client](../PowerKubernetesClient/) open the solution in VS, set the console project as startup, then run this command.
```
publish bootcamp
```
This will apply the files like the first tip, and it will also open the browser with this url: http://localhost:30460
That step is fired because of the extra PowerCommands specific file has been added to the other manifest files, but it has nothing to do with kubernetes.
As the tutorials will get more and more advanced I am sure that you does not want to do every single step manually.