# Cron job with .NET Worker service
## Use case
We setup a cron job that runs every 5 minutes, it starts a .NET Worker service that calls a .NET WebAPI method using the .NET HttpClient and that WebApi method will make sure that the post is persisted in the blog database on a MS SQL database server (also running as an Docker container). For this to work we need the following components.
- .NET Worker service **dockerdoktor/myworkerservice:latest**
- .NET WebAPI **dockerdoktor/workerservicetutorialwebapi:latest**
- MS Sql Database server **mcr.microsoft.com/mssql/server:2022-latest**

We are going to use Entity Framework and code first strategy to create the database and the table. I will not dig into the details about that. In this tutorial I am using two custom made containers one for the worker service and one for the WebAPI they will be running in two separate pods. I have published both of them on Docker hub so it is no big difference from the earlier tutorials. 

But if you are curios about that, the Visual Studio project are within this repo so you could look at it closer and see just how easy it is to create your own containers and publish them. Sooner or later you will start creating your own containers. 

## Database
In this tutorial we use the MS SQL database setup that was used in the [Persistent storage, setup a MS SQL Server](Docs/Percistent-Storage.md) tutorial. If you did not do that or if you have cleared your kubernetes kluster, you could apply the files in this [Folder](../src/persistent-storage/) first, just apply them like this:
```
k apply -f persistent-storage-01-pvc.yaml
k apply -f persistent-storage-02-secret.yaml
k apply -f persistent-storage-03-sqlserver-deploy.yaml
k apply -f persistent-storage-04-sqlserver-svc.yaml
```
Or you could simply use the PowerCommands kubernetes client and run:
```
publish --name persistent-storage
```
## Namespace
We will use a separate namespace for this tutorial named **worker-service** you create it with the first file in this [directory](../src/worker-service/), so first make sure you are in that folder with your favorite command line client. Then run this command: 
```
k apply -f worker-service-01-namespace.yaml
```
## Kubernetes cron job
To create a cron job is almost as creating a kubernetes app, the code looks like this:
```
apiVersion: batch/v1
kind: CronJob
metadata:
  name: my-worker-service-job
  namespace: worker-service  
spec:
  schedule: "*/5 * * * *" # Runs every 5 minutes
  jobTemplate:
    spec:
      template:
        spec:
          containers:
          - name: my-worker-service-job01
            image: dockerdoktor/myworkerservice:latest
            env:
            - name: WebApiUrl
              value: "http://192.168.0.16:31188/api"
          restartPolicy: OnFailure
```
The **schedule:** property is using a cron schedule syntax to set a schedule for reoccurring jobs, like a backup, a weekly mail or something like that. Read more about that [here](https://kubernetes.io/docs/concepts/workloads/controllers/cron-jobs/).

What this kubernetes job is going to do is to spin up the specified container every five minute, the container contains a .NET Worker service that will run a simple task and then quit, when that task is done the container will be taken down again.

## .NET Worker Service

Before creating this tutorial for my self and others I have never used .NET Worker Service so I have not so much to say about that other then for this kind of use case it is an good option. Lets look at the code, that is really simple as it can be, the code exists in this [WorkerServiceTutorial directory](../src/worker-service-WebProjects/WorkerServiceTutorial/).

```
public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    public Worker(ILogger<Worker> logger) => _logger = logger;
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        var socketsHttpHandler = new SocketsHttpHandler() { PooledConnectionIdleTimeout = TimeSpan.FromMinutes(1) };
        using var httpClient = new HttpClient(socketsHttpHandler);
        var uri = $"{Environment.GetEnvironmentVariable("WebApiUrl")}/blog";
        var post = new Post { Caption = "Worker service did it again!", MainBody = "Lorum ipsum", PostID = Guid.NewGuid() };
        var request = new HttpRequestMessage(HttpMethod.Post, uri) { Content = new StringContent(JsonSerializer.Serialize(post), Encoding.UTF8, "application/json") };
        await httpClient.SendAsync(request, stoppingToken);
    }
}
```
One class that implements the **BackgroundService** and overrides the method **ExecuteAsync(CancellationToken stoppingToken)**, the method implementation code is just logging and using a HttpClient to call a WebApi method to add a blog post. The method is using a environment variable that I will explain how you use that with Kubernetes.

### Environment variables and localhost
Lets have a closer look at some of the content in the kubernetes configuration for the cron job.
```
spec:
          containers:
          - name: my-worker-service-job01
            image: dockerdoktor/myworkerservice:latest
            env:
            - name: WebApiUrl
              value: "http://192.168.0.16:31188/api"
```
Above you can see the syntax for adding environment variables, it starts with **env:** followed byt the **- name:** and **value:** it is as simple as that.
But what about the IP address, where does that IP address come from? In this case when you are running everything on one host, your own machine it is that IP address that are used in this case, in the Visual Studio project localhost will work just as fine but remember, localhost in a container environment will not work, the WebAPI is not hosted in the same container or in the same pod as the Worker Service.

So for make this example work on your machine you must change the 192.168.0.16 part as you probably do not have the same IP address as I have locally.
So change ```http://192.168.0.16:31188/api``` to ```http://your-ip-adress:31188/api```, use **ipconfig** to find out your ip address.

When you have change the IP address to your IP address then you could apply this to the kubernetes kluster with this code:
```
k apply -f worker-service-02-cron-job.yaml
```
*Note that the work will fail as long as the WebAPI container is up and running.*

## .NET WebAPI
I will not get into details about it, in this [WorkerServiceTutorial directory](../src/worker-service-WebProjects/WorkerServiceTutorial/) you have the VS solution with the WebAPI and the Worker Service projects. You could just open that with Visual Studio and examine the code if you want. The WebApi has one a controller named BlogController and there is a method that creates a blog post in the table Blog in the MS Sql database. If the database does not exist it will be created, same for the Blog table. One thing though we do need to handle and that is the connection string, in this case we will use an environment variable once again combined with the kubernetes secrets functionality.

### Kubernetes secret and the Connection String?
Please not that this tutorial is not about secure coding, in a real world scenario you would probably use some kind of Vault solution to store your secrets in a secure way. Kubernetes functionality for handling sensitive data is not really good enough. But it is better than storing the password for the database user in clear text.

First we need to create the secret in our kubernetes kluster (again), we have already done that earlier when we created the MS Sql Server database, but it is an another namespace, and for this tutorial, lets forget it, lets create a new secret just for this WebAPI, this how a secret is declared. 
```
kind: Secret
apiVersion: v1
metadata:
  name: mssql-sample-secret
  namespace: worker-service  
data:  
  SA_PASSWORD: UEBzc3dvcmQxJA==
type: Opaque
```
And now apply it to the kubernetes kluster.
```
k apply -f worker-service-03-secret.yaml
```
Alright, we now have created this secret, how can the WebAPI use this secret using a environment variable?
This how to do that in the file **worker-service-04-deployment.yaml**:
```
env:
        - name: SA_PASSWORD
          valueFrom:
            secretKeyRef:
              name: mssql-sample-secret
              key: SA_PASSWORD
```
This will make sure that the container that the WebAPI runs within will have the SA_PASSWORD environment variable available.
In the **appsettings.json** class it looks like this:
```
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=$DB_SERVER$; Initial Catalog=BlogDBLocal; Persist Security Info=True; User Id=SA; Password=$PASSWORD$; TrustServerCertificate=True"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```
Notice that I am using placeholders for ```$DB_SERVER$``` and ```$PASSWORD$``` and in the **program.cs** file I just replace them with the environment variables.
```
builder.Services.AddDbContext<BlogDBContext>(options => options.UseSqlServer(GetConnectionString(builder.Configuration)));
static string GetConnectionString(ConfigurationManager? configurationManager)
{
    if (configurationManager == null) return "";
    var cnString = $"{configurationManager.GetConnectionString("DefaultConnection")}";
    var retVal = cnString.Replace("$PASSWORD$", Environment.GetEnvironmentVariable("SA_PASSWORD"));
    retVal = retVal.Replace("$DB_SERVER$", Environment.GetEnvironmentVariable("DB_SERVER"));
    return retVal;
}
```
Yes it a bit ugly, you can do this much nicer... but I leave that to you, this tutorial is about kubernetes. You now understand how you could configure environment variables in clear text or as secrets for your kubernetes cluster and use them in your ASP.NET application.

Now it is time to add your WebApi Application to the kubernetes cluster like this:
```
k apply -f worker-service-04-deployment.yaml
```
We are now almost done, last step is to create the service for the WebAPI application, once again I am using a NodePort so that the WebAPI will be reachable from the Worker Service application. I hade some problem at first to get this working using https, so I used un encrypted communication with http instead. Using certificate is something to explore later on in upcoming tutorials, I will not dig deeper in to that problem here.

Good for you to now about this last file **worker-service-05-service.yaml** is that it is here the port that is used by the WorkerService ```http://your-ip-adress:31188/api``` is set.

```
ports:
  - nodePort: 31188
    port: 80
    protocol: TCP
    targetPort: 80
```
So lets apply the last configuration file 
```
k apply -f worker-service-05-service.yaml
```
when you have done that, the database **BlogDBLocal** will be created in the Database container instance, and post will be added every five minutes, looking something like this:
```
PostID	Caption	MainBody	Created
03CC627B-D09A-4C48-8216-0449E22759C4	Worker service did it again!	Lorum ipsum	0001-01-01 00:00:00.0000000
58804B25-979A-4657-B2D8-1AC5FA552985	Worker service did it again!	Lorum ipsum	0001-01-01 00:00:00.0000000
5E59D75E-B817-4696-BBE0-243B78D3A424	Worker service did it again!	Lorum ipsum	0001-01-01 00:00:00.0000000
E6D5E6CD-2B4B-4F14-BA7D-2576C83B4E9F	Worker service did it again!	Lorum ipsum	0001-01-01 00:00:00.0000000
A0E39832-8EE1-4092-9375-3083CDF94858	Worker service did it again!	Lorum ipsum	0001-01-01 00:00:00.0000000
```