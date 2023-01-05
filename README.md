# Kubernetes - Docker Desktop tutorials
My shared playground that I update as I learning K8:s using Docker Desktop on Windows 11.
I am sharing my work as is, I leave no guarantees and I am an experienced developer and architect but far from an expert on kubernetes and learning this while doing it.

## Preparations
[Set up your kubernetes kluster with Docker Desktop on Windows](Docs/Preparations.md)

## Tutorials

[Deploy your first app](Docs/Deploy-Your-First-App.md)

Learn about how to make simple queries against your kubernetes cluster, and apply your deployment.

[Setup a MS SQL database Server](Docs/Percistent-Storage.md)

How to you create storage claims, how to use kubernetes secrets.

[Cron job with .NET Worker service](Docs/Worker-service.md)

How to create kubernetes jobs, using a SQL server (from earlier tutorial) a .NET Worker service and a .NET WebAPI together. In this tutorial I will create two own images and publish them on docker hub. I will also using environment variables and secrets.

[S3 storage with MinIO](Docs/S3-storage.md)

MinIO is a high performance object storage solution that provides an Amazon Web Services S3-compatible API and supports all core S3 features. This storage is to be used by an upcoming tutorial where we setup a Jupyter Notebook instance that stores it´s data in a MinIO "bucket".

[Install Wordpress with Helm and Helm chart](Docs/Wordpress.md)

The package manager for Kubernetes, Helm is the best way to find, share, and use software built for Kubernetes. Before you run this tutorial you need to [install helm](Docs/Helm.md).

# Tools
[Kubernetes Dashboard](Docs/Deploy-Kubernetes-Dashboard.md)

[ArgoCD (GitOps tool to synchronize your repo and K8:s cluster)](Docs/ArgoCD.md)

[Helm (Helm is the best way to find, share, and use software built for Kubernetes.)](Docs/Helm.md)




