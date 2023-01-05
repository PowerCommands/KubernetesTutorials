# Wordpress using Helm
For this tutorial you first need to install Helm, that is described here:

[Install Helm](Helm.md)

Once you have Helm ready, you can add a chart repository. We start with adding the bitnami repository.
```
helm repo add bitnami https://charts.bitnami.com/bitnami
```
Check out the content of this repo with:
```
helm search repo bitnami

bitnami/airflow                                 14.0.6          2.5.0           Apache Airflow is a tool to express and execute...
bitnami/apache                                  9.2.9           2.4.54          Apache HTTP Server is an open-source HTTP serve...
bitnami/appsmith                                0.1.5           1.8.13          Appsmith is an open source platform for buildin...
bitnami/argo-cd                                 4.3.8           2.5.5           Argo CD is a continuous delivery tool for Kuber...
bitnami/argo-workflows                          5.1.0           3.4.4           Argo Workflows is meant to orchestrate Kubernet...
bitnami/aspnet-core                             4.0.1           7.0.1           ASP.NET Core is an open-source framework for we...
bitnami/cassandra                               9.7.7           4.0.7           Apache Cassandra is an open source distributed ...
bitnami/cert-manager                            0.8.10          1.10.1          cert-manager is a Kubernetes add-on to automate...
bitnami/clickhouse                              2.2.0           22.12.1         ClickHouse is an open-source column-oriented OL...
```
## Install the Wordpress Chart
Before we install Wordpress on our Kubernetes cluster, lets find out what is customizable for the chart.
```
helm show values bitnami/wordpress
```
Well... it´s a lot, I will not go through all the stuff, but I put the content in this [wordpress/values.yaml](../manifests/wordpress/values.yaml) file so that you could explore the content and change it if you want (or dare).

I did no changes but before I installed I created a separate namespace for this tutorial named wordpress, like this.
```
apiVersion: v1
kind: Namespace
metadata:  
  labels:
    kubernetes.io/metadata.name: wordpress
  name: wordpress  
spec:
  finalizers:
  - kubernetes
```
And after that I changed the context to my new created namespace with this command:
```
kubectl config set-context --current --namespace=wordpress
```

Finally we are ready to install this helmchart!

```
helm install happy-panda bitnami/wordpress
```
There is some strange instructions about how to this wordpress pod should be reachable I tried to run the commands but all I got was a couple of errors. But...
If I just open the http://localhost/ in a browser I can confirm that the Wordpress application is up and running!

Read more about using Helm and helmcharts here: https://helm.sh/docs/intro/using_helm/

## Alright that was fun but how do I remove this stuff?
That´s easy just run this command:
```
helm uninstall happy-panda
```

And, bye bye happy-panda!