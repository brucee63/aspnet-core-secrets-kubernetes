## ASP.NET Core Secrets in Kubernetes Sample

Original blog post - [http://anthonychu.ca/post/aspnet-core-appsettings-secrets-kubernetes/](http://anthonychu.ca/post/aspnet-core-appsettings-secrets-kubernetes/)

Forked from Github repo - [https://github.com/anthonychu/aspnet-core-secrets-kubernetes](https://github.com/anthonychu/aspnet-core-secrets-kubernetes)

This is a good working example for Secret file-based binding in Kubernetes. However, the original example is in ASP.NET Core 1.1 and didn't cover ConfigMaps (which may be useful for common/shared config in a given Kubernetes namespace)

## Summary of Changes
 - Upgraded to ASP.NET 6 (rework needed in Startup.ns & Program.cs)
 - Reworked Dockerfile to use latest Debian Bullseye SDK and aspnet images
 - Added docker-compose file for local testing without Kubernetes
 - Added common/base config and environment specific overrides json files
 - Added kubectl commands to create secrets, configmaps, deployment and service/ingress
 - Hard coded internal port to 5015 for local testing, including Kestrel settings in appsettings.json

### Order of Precedent 
Order of precedent is as follows for replacement (lowest to highest)
   - appsettings.common.json	(ConfigMap bind override)
   - appsettings.json
   - appsettings.<ENV>.json
   - appsettings.secrets.json	(Secrets bind override)
