FROM mcr.microsoft.com/dotnet/aspnet:6.0.4-bullseye-slim AS base
WORKDIR /app
EXPOSE 5015:5015

ENV ASPNETCORE_URLS="http://*:5015"
ENV ASPNETCORE_ENVIRONMENT="development"
ENV TZ=America/Chicago

FROM mcr.microsoft.com/dotnet/sdk:6.0.202-bullseye-slim AS build
WORKDIR /src
COPY ./aspnet-core-secrets.csproj aspnet-core-secrets/
RUN dotnet restore ./aspnet-core-secrets/aspnet-core-secrets.csproj
WORKDIR /src/aspnet-core-secrets
COPY . .
RUN dotnet build "aspnet-core-secrets.csproj" -c Release -o /app

FROM build AS publish
RUN find -type d -name bin -prune -exec rm -rf {} \; && find -type d -name obj -prune -exec rm -rf {} \;
RUN dotnet publish "aspnet-core-secrets.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

ENTRYPOINT ["dotnet", "aspnet-core-secrets.dll"]