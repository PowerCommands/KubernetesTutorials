# Volumes & Persistent Volumes
Containers are ephemeral by definition, which means that anything that it is stored at running time is lost when the container is stopped. This might cause problems with containers that need to persist their data, like database containers.

## Use case - Install MS SQL Server 
So in this tutorial we are setting up a database container with persistent storage "outside" the container it self. When the container goes down, the data is still there ready to be used when the container is up again. For this tutorial we will use files and use the command **apply** to apply our changes to the kubernetes cluster. All this files are stored in the **[src/persistent-storage](../src/persistent-storage/)** directory.

### Create a Persistent Volume
Persistent volume claim is needed to store SQL Server data and yaml snippet to create a 5 GB storage is displayed below. The deployment file is going to mount files to this storage claim.

 **[persistent-storage-01-pvc.yaml](../src/persistent-storage/persistent-storage-01-pvc.yaml)**
```
apiVersion: v1
kind: PersistentVolumeClaim
metadata:
  name: mssql-sample-data-claim
spec:
  accessModes:
  - ReadWriteOnce
  resources:
   requests:
    storage: 5Gi
```
Make sure that your command environment is browsing the directory with the yaml configuration files for this tutorial, they are located here **[src/persistent-storage](../src/persistent-storage/)**.

Run this command:
```
k apply -f persistent-storage-01-pvc.yaml
```
### Create a Kubernetes Secret
The SQL Server instance needs a password, we provide that with the use of a Kubernetes secret.

**[persistent-storage-02-secret.yaml](../src/persistent-storage/persistent-storage-02-secret.yaml)**
```
kind: Secret
apiVersion: v1
metadata:
  name: mssql-sample-secret
  namespace: default
data:
  # Password is P@ssword1$ so update it with password of your choice  
  SA_PASSWORD: UEBzc3dvcmQxJA==
type: Opaque
```
If you want to change the password (and you really should) you could use the **[PowerCommands.KubernetesCommands](../PowerCommandsClient/)** command **base64**.
```
base64 --encode your-new-password
```
Or use your Linux Ubuntu that you installed with WSL2 if you are familiar with Linux.

Run this command:
```
k apply -f persistent-storage-02-secret.yaml
```

### Deployment

**[persistent-storage-03-sqlserver-deploy.yaml](../src/persistent-storage/persistent-storage-03-sqlserver-deploy.yaml)** 

```
apiVersion: apps/v1
kind: Deployment
metadata:
  name: mssql-sample-deployment
  namespace: default
spec:
  replicas: 1
  selector:
    matchLabels:
      app: mssql-sample
  template:
    metadata:
      labels:
        app: mssql-sample
    spec:
      terminationGracePeriodSeconds: 10
      containers:
      - name: mssql
        image: mcr.microsoft.com/mssql/server:2022-latest
        ports:
        - containerPort: 1433
        env:
        - name: ACCEPT_EULA
          value: "Y"        
        - name: SA_PASSWORD
          valueFrom:
            secretKeyRef:
              name: mssql-sample-secret
              key: SA_PASSWORD
        volumeMounts:
        - name: mssql-persistent-storage
          mountPath: /var/opt/mssql
      volumes:
      - name: mssql-persistent-storage
        persistentVolumeClaim:
          claimName: mssql-sample-data-claim
```
Run this command:
```
k apply -f persistent-storage-03-sqlserver-deploy.yaml
```

### Service

**[persistent-storage-04-sqlserver-svc.yaml](../src/persistent-storage/persistent-storage-04-sqlserver-svc.yaml)**
```
apiVersion: v1
kind: Service
metadata:
  name: mssql-sample-service
  namespace: default
spec:
  clusterIP: 10.96.166.44
  clusterIPs:
  - 10.96.166.44
  externalTrafficPolicy: Cluster
  internalTrafficPolicy: Cluster
  ipFamilies:
  - IPv4
  ipFamilyPolicy: SingleStack  
  ports:
  - nodePort: 30200
    port: 1433
    protocol: TCP
    targetPort: 1433  
  selector:
    app: mssql-sample
  type: NodePort
```
Run this command:
```
k apply -f persistent-storage-04-sqlserver-svc.yaml
```

### Login with Management Studio
Find out your IP-address with 
```
ipconfig
```
Connect to server: **local ip address**,30200
## Heads up
The connection to a MS SQL server has breaking changes which default to always use encrypted connections, make sure that you uncheck that option like this.

![Alt text](images/tutorial_3_1.png?raw=true "Uncheck encrypt connection")