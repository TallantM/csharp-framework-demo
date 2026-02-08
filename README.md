# C# Framework Demo [![CI](https://github.com/TallantM/csharp-framework-demo/actions/workflows/ci.yml/badge.svg)](https://github.com/TallantM/csharp-framework-demo/actions/workflows/ci.yml) [![Allure Report](https://img.shields.io/badge/Allure-Report-blue)](https://tallantm.github.io/csharp-framework-demo/)


A client project demonstration showcasing automated testing in C#/.NET, incorporating tools like Playwright for browser automation, reusable utilities, layered test suites (unit, integration, and end-to-end), and a GitHub Actions CI workflow with Docker containerization for reliable quality assurance.

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
1. Clone the repo: `git clone https://github.com/TallantM/csharp-framework-demo.git`
2. Navigate to src: `cd src`
3. Restore packages: `dotnet restore`
4. Build the project: `dotnet build` (optional, verifies compilation)
5. Install Playwright browsers: `playwright install`

For Docker setup (if not installed):
- Install Docker from https://docs.docker.com/get-docker/
- Ensure the Docker daemon is running: `docker info`

## Folder Structure
```text
csharp-framework-demo/
├── .github/
│   └── workflows/
│       └── ci.yml          # GitHub Actions CI workflow
├── src/
│   ├── csharp_framework_demo.csproj  # Project file with dependencies
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
└── csharp_framework_demo.sln        # Solution file
```

## Testing Overview
- **Unit Tests**: Isolated verification of utilities (e.g., `LoginPageUnitTests.cs`) using Moq for mocking dependencies, ensuring fast and deterministic checks without browser overhead.
- **Integration Tests**: Validation of component interactions (e.g., `LoginPageIntegrationTests.cs`) in a simulated browser context.
- **E2E Tests**: Full browser automation (e.g., `ExampleTests.cs`) for login scenarios on saucedemo.com, showcasing Playwright's capabilities.

## Running Tests
For rapid development, run tests directly on your host (requires host browser installation):
```bash
cd src
playwright install  # Install browsers on host
dotnet test 
```

For consistency with CI, use Docker (recommended before pushing):
```bash
docker build -t csharp_framework_demo .
docker run csharp_framework_demo
```

## CI/CD
GitHub Actions workflow in `.github/workflows/ci.yml` builds the Docker image and runs tests exclusively in the container on push/pull requests, ensuring environmental consistency across machines.

## Spec-Driven Development

This project uses **Spec-Driven Development (SDD)** where specifications guide implementation:

### What is SDD?

1. **Specifications** - Written in `specs/` directory define expected behavior before code
2. **AI Code Generation** - AI agents (like Claude Code) generate initial code from specs
3. **Human Approval** - Developers review, refine, and approve generated code
4. **Synchronized Maintenance** - Both specs and code are maintained together
5. **Conformance Testing** - Automated tests validate spec-code alignment

### Spec Directory Structure

```
specs/
├── META-FRAMEWORK.md          # SDD process definition and workflow
├── PROJECT-SPEC.md            # Framework architecture and standards
├── templates/                 # Spec templates for new features
│   ├── page-objects.template.md
│   ├── unit-tests.template.md
│   ├── integration-tests.template.md
│   └── workflows.template.md
└── features/                  # Feature specifications (added as features are developed)
    └── authentication/        # Example feature (future)
        ├── page-objects.md
        ├── unit-tests.md
        ├── integration-tests.md
        └── workflows.md
```

### Getting Started with SDD

**Read the Specifications:**
- [`specs/META-FRAMEWORK.md`](specs/META-FRAMEWORK.md) - Understand the SDD workflow and process
- [`specs/PROJECT-SPEC.md`](specs/PROJECT-SPEC.md) - Learn the architectural constraints and standards

**Creating New Features (Spec-First):**
1. Copy templates from `specs/templates/` to `specs/features/{feature-name}/`
2. Fill out the spec templates with feature details
3. Ask AI (Claude Code) to generate code from specs
4. Review and refine generated code
5. Update specs if code refinements differ from original spec
6. Run conformance tests: `dotnet test --filter "FullyQualifiedName~SpecConformanceTests"`
7. Commit both specs and code together

**Modifying Existing Features:**
- If changing code → Update corresponding spec
- If changing spec → Update or regenerate code
- Always keep specs and code synchronized
- Conformance tests validate alignment

### Conformance Testing

Run conformance tests to validate spec-code alignment:
```bash
cd src
dotnet test --filter "FullyQualifiedName~SpecConformanceTests"
```

These meta-tests ensure:
- All spec files exist and are well-formed
- Spec templates are available
- SpecValidation utilities are present
- (Future) Generated code matches specs

## Troubleshooting
- **Browser Launch Failures**: Ensure `playwright install` has run successfully. If using Docker, verify all dependencies are included in the Dockerfile.
- **Dependency Errors**: Run `dotnet restore` to refresh packages. For Docker builds, check for network issues during library installations.
- **Test Timeouts**: Increase timeouts in Playwright options if network latency affects external sites like saucedemo.com.
- **CI Failures**: Review workflow logs for specific errors; caching may need invalidation if dependencies change.

## Contributing
Contributions are welcome to enhance the demo. Please fork the repository, create a feature branch, and submit a pull request with clear descriptions of changes.
