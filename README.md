# C# Framework Demo [![CI](https://github.com/TallantM/playwright-csharp-demo/actions/workflows/ci.yml/badge.svg)](https://github.com/TallantM/playwright-csharp-demo/actions/workflows/ci.yml)

A client project demonstration showcasing automated testing in C#/.NET, incorporating tools like Playwright for browser automation, reusable utilities, layered test suites (unit, integration, and end-to-end), and a GitHub Actions CI workflow with Docker containerization for reliable quality assurance. This repository highlights proficiency in C# through structured and maintainable testing practices, making it well-suited for job applications or client presentations.

## Prerequisites
- .NET SDK 8.0+
- Git
- Visual Studio Code with C# extension
- Playwright CLI (install via `dotnet tool install --global Microsoft.Playwright.CLI`)
- Docker (for containerized testing)

Verify prerequisites:
- .NET SDK: `dotnet --version` (should output 8.0 or higher)
- Git: `git --version`
- Playwright CLI: `dotnet tool list --global | grep microsoft.playwright.cli` (or on Windows: `dotnet tool list --global | Select-String "microsoft.playwright.cli"`)
- Docker: `docker --version` (ensure Docker is running with `docker info`)

## Setup
1. Clone the repo: `git clone https://github.com/TallantM/playwright-csharp-demo.git`
2. Navigate to src: `cd src`
3. Restore packages: `dotnet restore`
4. Build the project: `dotnet build` (optional, verifies compilation)
5. Install Playwright browsers: `playwright install`

For Docker setup (if not installed):
- Install Docker from https://docs.docker.com/get-docker/
- Ensure the Docker daemon is running: `docker info`

## Folder Structure
playwright-csharp-demo/
├── .github/
│   └── workflows/
│       └── ci.yml          # GitHub Actions CI workflow
├── src/
│   ├── PlaywrightDemo.csproj  # Project file with dependencies
│   ├── Tests/
│   │   └── ExampleTests.cs    # End-to-end tests
│   │   └── LoginPageUnitTests.cs  # Unit tests with mocking
│   │   └── LoginPageIntegrationTests.cs  # Integration tests
│   └── Utilities/
│       └── PageObjects/
│           └── LoginPage.cs  # Reusable page object utility
├── .gitignore                # Ignores .NET and Playwright artifacts
├── Dockerfile                # Containerized build for testing
├── README.md                 # This documentation
└── PlaywrightDemo.sln        # Solution file

## Testing Overview
This repository demonstrates layered testing to highlight automated testing skills:
- **Unit Tests**: Isolated verification of utilities (e.g., `LoginPageUnitTests.cs`) using Moq for mocking dependencies, ensuring fast and deterministic checks without browser overhead.
- **Integration Tests**: Validation of component interactions (e.g., `LoginPageIntegrationTests.cs`) in a simulated browser context.
- **E2E Tests**: Full browser automation (e.g., `ExampleTests.cs`) for login scenarios on saucedemo.com, showcasing Playwright's capabilities.

## Running Tests
For rapid development, run tests directly on your host (requires host browser installation):
cd src
playwright install  # Install browsers on host
dotnet test PlaywrightDemo.csproj

For consistency with CI, use Docker (recommended before pushing):
docker build -t playwright-csharp-demo .
docker run playwright-csharp-demo

## CI/CD
GitHub Actions workflow in `.github/workflows/ci.yml` builds the Docker image and runs tests exclusively in the container on push/pull requests, ensuring environmental consistency across machines.

## Troubleshooting
- **Browser Launch Failures**: Ensure `playwright install` has run successfully. If using Docker, verify all dependencies are included in the Dockerfile.
- **Dependency Errors**: Run `dotnet restore` to refresh packages. For Docker builds, check for network issues during library installations.
- **Test Timeouts**: Increase timeouts in Playwright options if network latency affects external sites like saucedemo.com.
- **CI Failures**: Review workflow logs for specific errors; caching may need invalidation if dependencies change.

## Contributing
Contributions are welcome to enhance the demo. Please fork the repository, create a feature branch, and submit a pull request with clear descriptions of changes.