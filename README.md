# KubernetesTutorials
My shared playground that I update as I learning K8:s using Docker Desktop on Windows 11.
I am sharing my work as is, I leave no garantuees and I am learning this while doing it.

## Preparation
You need the following:

**You neeed the following:**
- [ ] Docker Desktop, with WSL2
- [ ] Visual Code

### Docker Desktop
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
### Visual Code
https://code.visualstudio.com/download

### Now you are prepared!

# Tutorials
[Deploy your first app](wiki/Deploy-Your-First-App.md)




