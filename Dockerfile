# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY FizzSeven.slnx ./
COPY Directory.Build.props ./
COPY src/FizzSeven.Api/FizzSeven.Api.csproj src/FizzSeven.Api/
COPY src/FizzSeven.Core/FizzSeven.Core.csproj src/FizzSeven.Core/
COPY src/FizzSeven.ServiceDefaults/FizzSeven.ServiceDefaults.csproj src/FizzSeven.ServiceDefaults/

RUN dotnet restore src/FizzSeven.Api/FizzSeven.Api.csproj

COPY src/ ./src/

RUN dotnet publish src/FizzSeven.Api/FizzSeven.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS final
WORKDIR /app

COPY --from=build /app/publish ./

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "FizzSeven.Api.dll"]
