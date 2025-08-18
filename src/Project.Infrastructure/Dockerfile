ARG DOTNET_RUNTIME=mcr.microsoft.com/dotnet/aspnet:6.0
ARG DOTNET_SDK=mcr.microsoft.com/dotnet/sdk:6.0

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
ENV ASPNETCORE_URLS=http://+:7105
WORKDIR /home/app
EXPOSE 7105

# Base for build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS buildbase
WORKDIR /src

COPY ["Project.sln", "./"]
COPY ["Project.Domain/Project.Domain.csproj", "Project.Domain/"]
COPY ["Project.Infrastructure/Project.Infrastructure.csproj", "Project.Infrastructure/"]
COPY ["Project.Application/Project.Application.csproj", "Project.Application/"]
COPY ["Project.Api/Project.Api.csproj", "Project.Api/"]
COPY ["tests/Project.Tests.Unit/Project.Tests.Unit.csproj", "tests/Project.Tests.Unit/Project.Tests.Unit.csproj"]
COPY ["tests/Project.Tests.Integration/Project.Tests.Integration.csproj", "tests/Project.Tests.Integration/Project.Tests.Integration.csproj"]

RUN dotnet restore Project.sln

COPY . .

## Run migrations
FROM buildbase as migrations
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"
ENTRYPOINT dotnet-ef database update --project ./Project.Infrastructure --startup-project ./Project.Api