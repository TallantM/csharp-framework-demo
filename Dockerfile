FROM mcr.microsoft.com/playwright/dotnet:v1.57.0-jammy

WORKDIR /app

# Copy project files
COPY src/*.csproj ./src/
COPY src/allureConfig.json ./src/

# Restore dependencies
RUN dotnet restore src/csharp_framework_demo.csproj

# Copy everything else
COPY src/ ./src/

# Build the project
RUN dotnet build src/csharp_framework_demo.csproj -c Release

# Install Playwright browsers
# RUN dotnet tool install --global Microsoft.Playwright.CLI
# ENV PATH="${PATH}:/root/.dotnet/tools"


# Run tests and generate allure results
CMD ["dotnet", "test", "src/csharp_framework_demo.csproj", "-c", "Release", "--logger", "console;verbosity=detailed"]