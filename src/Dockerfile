# Stage 1 - Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Install dotnet-ef tool globally
RUN dotnet tool install --global dotnet-ef \
    && export PATH="$PATH:/root/.dotnet/tools"

# Copy csproj files and restore dependencies
COPY ./Project.Api/*.csproj ./Project.Api/
COPY ./Project.Application/*.csproj ./Project.Application/
COPY ./Project.Domain/*.csproj ./Project.Domain/
COPY ./Project.Infrastructure/*.csproj ./Project.Infrastructure/
COPY ./Project.Logging/*.csproj ./Project.Logging/
COPY ["tests/Project.Tests.UnitTests/Project.Tests.UnitTests.csproj", "tests/Project.Tests.UnitTests/Project.Tests.UnitTests.csproj"]
COPY ["tests/Project.Tests.Integration/Project.Tests.Integration.csproj", "tests/Project.Tests.Integration/Project.Tests.Integration.csproj"]

RUN dotnet restore ./Project.Api/Project.Api.csproj

# Copy everything and publish
COPY . .
WORKDIR /app/Project.Api
RUN dotnet publish -c Release -o /app/publish

# Stage 2 - Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Make sure the PATH includes dotnet tools (for EF commands if running here)
ENV PATH="$PATH:/root/.dotnet/tools"

ENTRYPOINT ["dotnet", "Project.Api.dll"]