# Playwright C# Demo [![CI](https://github.com/TallantM/playwright-csharp-demo/actions/workflows/ci.yml/badge.svg)](https://github.com/TallantM/playwright-csharp-demo/actions/workflows/ci.yml)
A client project demonstration of automated testing using Playwright with C#/.NET, featuring reusable utilities, end-to-end test suites, and a GitHub Actions CI workflow for scalable quality assurance.
## Prerequisites
- .NET SDK 8.0+
- Git
- VS Code with C# extension
- Playwright CLI (install via `dotnet tool install --global Microsoft.Playwright.CLI`)
## Setup
1. Clone the repo: `git clone https://github.com/yourusername/playwright-csharp-demo.git`
2. Navigate to src: `cd src`
3. Restore packages: `dotnet restore`
4. Install Playwright browsers: `playwright install`
## Testing Overview
- **Unit Tests**: Isolated tests for utilities (e.g., `LoginPageUnitTests.cs`) using Moq.
- **Integration Tests**: Component interaction tests (e.g., `LoginPageIntegrationTests.cs`).
- **E2E Tests**: Full browser automation (e.g., `ExampleTests.cs`).

Run all tests: `dotnet test`
## Docker Support
Build the image: `docker build -t playwright-csharp-demo .`
Run tests in container: `docker run playwright-csharp-demo`
## CI/CD
GitHub Actions workflow in `.github/workflows/ci.yml` runs tests on push/pull requests.
