# S3 storage with MinIO
MinIO is a high performance object storage solution that provides an Amazon Web Services S3-compatible API and supports all core S3 features. With this service you can create "Buckets" and in these buckets you can store files.

This buckets can then be accessed from other applications like a Jupyter Notebook, that I am planning to use in an upcoming tutorial where I combine Jupyter Notebook with MinIO S3 storage.

## Installation

### Create namespace
```
kubectl apply -f s3-storage-01-namespace.yaml
```
### Create a PVC 
Persistent volume claim was something we did in an earlier tutorial, we do it again here. The claim will later be used in the deployment configuration file. In this example I have set a claim of 1 gigabyte of disk in the Docker kubernetes cluster.
```
kubectl apply -f s3-storage-02-pvc.yaml
```
### Deploy the MinIO pod
```
kubectl apply -f s3-storage-03-deployment.yaml
```
### Port forwarding
Use the kubectl port-forward command to temporarily forward traffic from the MinIO pod to the local machine:
```
kubectl port-forward pod/minio 9000 9090 -n minio-dev
```
### Connect your Browser to the MinIO Server
Access the MinIO Console by opening a browser on the local machine and navigating to (http://127.0.0.1:9090)[http://127.0.0.1:9090].

Log in to the Console with the credentials minioadmin | minioadmin. These are the default root user credentials.

**Congratulations now you have successfully setup an S3 storage with MinIO!**