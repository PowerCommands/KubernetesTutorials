# Helm
The package manager for Kubernetes, Helm is the best way to find, share, and use software built for Kubernetes.

##Installation 
### Homebrew on Linux
First you need to install an installer package manager, I first tried **Snap** but no success just headache, so I went for **homebrew** instead, remember... We are talking about Linux now, nothing is simple when it comes to Linux, Google is your best friend.

Instructions are here: https://brew.sh/

First startup Wsl, open a cmd shell on Windows:
```
wsl
```
Then run this command
```
/bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"
```
After you run that you will get instructions to run three commands, you can not run the example code as your Linux instance probably has another name then mine (wslmodermodem2). 
```
echo '# Set PATH, MANPATH, etc., for Homebrew.' >> /home/wslmodermodem2/.profile
```
```
echo 'eval "$(/home/linuxbrew/.linuxbrew/bin/brew shellenv)"' >> /home/wslmodermodem2/.profile
```
```
eval "$(/home/linuxbrew/.linuxbrew/bin/brew shellenv)"
```
You can check that the environment variable is in place, first run this command.
```
cd ~
```
Now this:
```
nano .profile
```
See that this lines has been added at the bottom.
```
# Set PATH, MANPATH, etc., for Homebrew.
eval "$(/home/linuxbrew/.linuxbrew/bin/brew shellenv)"
```
Now you can install helm.
### Install helm
```
brew install helm
```
Congratulations you now have Helm installed, with this tool you can use **helmcharts** which will make bigger installations much easier.