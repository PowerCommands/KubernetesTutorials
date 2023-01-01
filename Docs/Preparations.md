# Preparation
You need the following:

**You neeed the following:**
- [ ] WSL2
- [ ] Docker Desktop, with WSL2
- [ ] Visual Code

## WSL2
Docker Desktop works best on Windows using WSL2, I first try to install it using the Hyper-V option as I already using Hyper-V for virtual machines. But for some unknown reason I did not get this to work on my Windows 11 machine so I retried with the recommended WSL2 option instead. (I know understand why it is the recommended choice)

Docker will enable the WSL2 feature on your machine for you BUT! You should really install WSL2 first before you even install Docker Desktop. 

Just follow the instructions here: https://learn.microsoft.com/en-us/windows/wsl/install

The Docker installation messed up my WSL2 installation a bit because I installed Docker Desktop first, no big deal but Docker Desktop set´s the wrong default Linux Distribution for WSL2.
You can check this easaly with this command.
```
wsl -l
```
This is how it should look like, the Docker-Desktop distributaions is not WSL distributions and should not be set as default.
```
Windows-subsystem for Linux-distributions:
Ubuntu-22.04 (default)
docker-desktop-data
docker-desktop
```
It is easy to change default distribution, just like this.
```
wsl -s Ubuntu-22.04
```
Check that WSL2 is workning well, just type wsl and hit enter with your favorite cmd applikcation.

## Docker Desktop
Install Docker Desktop, follow instructions here fore Windows use WSL2 configuration.

https://docs.docker.com/desktop/install/windows-install/

 - Activate Kubernetes, open settings, navigate to Kubernetes and Enable Kubernetes
 - Add a path to the Path environment variables to the installation/bin folder so that you kan run **kubectl** commands.
 

 **Alias**
 Setup a alias for **kubectl** with a cmd file with this content:
 ```
@echo off
doskey k=kubectl $*
```
Create a register file, name is unimportant but it must have the **.reg** as file extension, this is the content.
```
[HKEY_CURRENT_USER\Software\Microsoft\Command Processor]
"AutoRun"="c:\\\"Program Files\"\\k8s\\alias.cmd"
```
## Use PowerCommands 
You can use PowerCommands tool as your command-line environment instead of build in **cmd** or **powershell**, just open the VS solution [PowerCommandsClient](PowerCommandsClient) within this repo, set the PoserCommandsConsole project as startup project, hit [F5] and start typing your kubectl commands using the alias k (no need for the cmd and *.reg file configuration).

There could be some limitations if some interactions is required for instance login to a kubernetes environment with a password, the power command just passes through what you type to the kubectl.exe that was installed with Docker Desktop. 

The PowerCommands also includes automation functionality to publish the tutorials automatically, applying the files from [src](src/) folder using the sort order of the file names. You could also startup the Kubernetes Dashboard or the ArgoCD Administration UI with decoding of the initial ArgoCD admin password that otherwise could be a bit tricky.

## Visual Code
https://code.visualstudio.com/download

**Now you are prepared!**

Next step could be to try one of these tutorials...

# Tutorials

[Deploy your first app](Deploy-Your-First-App.md)

Learn about how to make simple queries against your kubernetes kluster, and apply your deployment.

[Persistent storage, setup a MS SQL Server](Percistent-Storage.md)

How to you create storage claims, how to use kubernetes secrets.

[Install Wordpress with Helm and Helm chart](Wordpress.md)

The package manager for Kubernetes, Helm is the best way to find, share, and use software built for Kubernetes.

[Cron job with .NET Worker service](Worker-service.md)

How to create kubernetes jobs, using a SQL server (from earlier tutorial) a .NET Worker service and a .NET WebAPI together. In this tutorial I will create two own images and publish them on docker hub. I will also using environment variables and secrets.
