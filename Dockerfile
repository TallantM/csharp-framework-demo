FROM mcr.microsoft.com/playwright/dotnet:v1.57.0-jammy

WORKDIR /app

# Copy csproj and restore dependencies
COPY src/*.csproj ./src/
RUN dotnet restore src/PlaywrightDemo.csproj

# Copy source and publish
COPY src/ ./src/
RUN dotnet publish src/PlaywrightDemo.csproj -c Release -o out

# Set the final working directory
WORKDIR /app/out

ENTRYPOINT ["dotnet", "test", "PlaywrightDemo.dll"]