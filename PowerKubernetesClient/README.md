# What is PowerCommands?
PowerCommands is a create your own CLI application starter kit! I have created it and somehow always ending up using it for some purpose. For this kubernetes tutorials I creating automation at the same time with PowerCommands.

The bootcamp sample and the dashboard sample can be created using the files in the **[src](../src/)** folder and PowerCommands in combination. The files in the **[src](../src/)** can be used to apply the tutorials also of course.

With PowerCommand you could create the bootcamp sample like this:
``` 
publish --name bootcamp
```
This will be the same as applying the files in the **[src/dashboard](../src/dashboard/)** directory with this commands.
```
kubectl apply -f bootcamp-01-deployment.yaml
kubectl apply -f bootcamp-01-deployment.yaml
```
You could do the same thing with the Dashboard tutorial, using the same command, but different name (which reflects the directory name).
``` 
publish --name dashboard
```
When you done that you can start your kubernetes Dashboard with this command:
``` 
dashboard
```
First time you will need a bearer token for the admin-user, you can get that with this command:
``` 
token --username admin-user
```
Copy the token that is generated to login to the dashboard opened earlier.

## Remember!
PowerCommands is not a professional tool for managing Kubernetes and Docker, it is an open framework to create command line applications really fast. I use it just to automate the creation just for my own sake, I want to insure my self that the tutorials can be recreated at any time. I use Docker Desktop to re-create the kubernetes cluster over and over again.

You could read more about PowerCommands here:

https://github.com/PowerCommands/PowerCommands2022