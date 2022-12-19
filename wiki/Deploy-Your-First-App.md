# Deploy a simple webb applikation
This example will guide you trought the steps to deploy one webb applikation in one pod on a Docker Desktop K8:s kluster. As the Docker Desktop K8:s environment only consist of one kluster already prepared for you, the creation of a K8:s kluster is omitted. The code examples is using an [alias](../README.md) for **kubectl**

## Deployoment
### Get the nodes of our K8:s kluster, just to know that your kubernetes kluster is up and running.

``` 
k get nodes
```
Should return something like this on a fresh install.
``` 
NAME             STATUS   ROLES           AGE   VERSION
docker-desktop   Ready    control-plane   22h   v1.25.2
``` 
