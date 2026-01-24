FROM mcr.microsoft.com/playwright/dotnet:v1.57.0-jammy

WORKDIR /app

# Copy csproj, allureConfig and restore dependencies
COPY src/*.csproj ./src/
COPY src/allureConfig.json ./src/
RUN dotnet restore src/csharp_framework_demo.csproj

# Copy source, build, and publish
COPY src/ ./src/
RUN dotnet build src/csharp_framework_demo.csproj -c Release
RUN dotnet publish src/csharp_framework_demo.csproj -c Release -o out

# Copy allureConfig.json to output directory
COPY src/allureConfig.json ./out/

# Set the final working directory
WORKDIR /app/out

CMD ["dotnet", "test", "csharp_framework_demo.dll", "-c", "Release", "--logger", "console;verbosity=detailed"]