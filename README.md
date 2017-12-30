# Site Builder


Hosts file
```
127.0.0.1       red-container.local
127.0.0.1       blue-container.local
127.0.0.1       green-container.local
127.0.0.1       yellow-container.local

```

## Test Local Instances

```
docker network create nginx-proxy
docker-compose -f docker-compose-local.yml up
```

## Test With .NET CORE

```
docker-compose up -d
cd src/ConsoleTest
dotnet run
```