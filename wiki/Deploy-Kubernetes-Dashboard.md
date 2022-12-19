# Kubernetes Dashboard UI

## Deploy the Dashboard

The Dashboard UI is not deployed by default. To deploy it, run the following command:
```
kubectl apply -f https://raw.githubusercontent.com/kubernetes/dashboard/v2.6.1/aio/deploy/recommended.yaml
``` 
The dashboard is then available on your local machine:
http://localhost:8001/api/v1/namespaces/kubernetes-dashboard/services/https:kubernetes-dashboard:/proxy/

Please note that the current version 2.6.1 is going to be something else next month or maybe tomorrow. 

## Before you can use the Dashboard...

To use the Dashboard you first need a user with valid privileges, this is described here:
https://github.com/kubernetes/dashboard/blob/master/docs/user/access-control/creating-sample-user.md

**IMPORTANT:** Make sure that you know what you are doing before proceeding. Granting admin privileges to Dashboard's Service Account might be a security risk.
