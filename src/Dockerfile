# Stage 1 - Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copia csproj e restaura dependências
COPY ./Project.Api/*.csproj ./Project.Api/
COPY ./Project.Application/*.csproj ./Project.Application/
COPY ./Project.Domain/*.csproj ./Project.Domain/
COPY ./Project.Infrastructure/*.csproj ./Project.Infrastructure/
RUN dotnet restore ./Project.Api/Project.Api.csproj

# Copia tudo e builda release
COPY . .
WORKDIR /app/Project.Api
RUN dotnet publish -c Release -o /app/publish

# Stage 2 - Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Project.Api.dll"]
