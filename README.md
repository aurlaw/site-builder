# Site Builder


Hosts file
```
127.0.0.1       red-container.local
127.0.0.1       blue-container.local
127.0.0.1       green-container.local
127.0.0.1       yellow-container.local

```

## Setup

```
docker network create nginx-proxy
docker volume create pgdata

```


## Test Local Instances

```
docker-compose -f docker-compose-local.yml up
```

## Test With .NET CORE

```
docker-compose up -d
cd src/ConsoleTest
dotnet run
```

## ASPNET Core

TBD