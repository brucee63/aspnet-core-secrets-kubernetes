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
 - Added Kustomize patching/overlay support to conditionally add secrets and map environment variable values

### Order of Precedent 
Order of precedent is as follows for replacement (lowest to highest)
   - appsettings.common.json	(ConfigMap bind override)
   - appsettings.json
   - appsettings.<ENV>.json
   - appsettings.secrets.json	(Secrets bind override)

## Kustomize
Kustomize support is built into the kubectl command in Kubernetes. It's a simple patching/overlay framework for yaml.

Install the Kustomize binary locally so you can verify resultant yaml [Install Kustomize](https://kubectl.docs.kubernetes.io/installation/kustomize/)

### Folder structure
```
└───kustomize
    ├───base
    │   └───config
    └───overlays
        ├───development
        ├───local
        ├───production
        │   └───config
        └───qualityassurance
```

To preview results, navigate to the base or an environment overlay folder and run the following command -
```
kustomize build .
```

I'm adding the appsettings.common.yaml as a base file configset injection, so this overrides the Cache.ConnectionString settings for all environments.
The appsettings.secret.yaml file is only being injected for the production overlay.

Secrets are base64 encoded in kubernetes. To decode the value you can use the following shell command -
```bash
echo encodedvalue | base64 --decode
```

Configmaps which store file contents are represented as hexidecimal strings with a preceding byte order mark (BOM). In this case, with a value of \uFEFF for UTF-16 (LE).
[Byte Order Mark - BOM](https://en.wikipedia.org/wiki/Byte_order_mark)

You can use pythen, xxd, etc to decode. Below is an example using python, just replace the value hexencodedstring with the output of Kustomize for the configmap file -

```python
binascii.unhexlify(binascii.b2a_hex(b'hexencodedstring'))
```

You can also use an online converter, just be sure nothing sensitive is encoded - [onlinestringtools.com](https://onlinestringtools.com/convert-hexadecimal-to-string)
