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
│       └── ci.yml                    # GitHub Actions CI workflow with Allure reporting
├── docs/                             # Project documentation
│   ├── agent-patterns.md             # Reusable AI agent patterns for SDD
│   ├── retrospective-parallel-tests.md  # Detailed retrospective of parallel test refactoring
│   └── test-architecture-spec.md     # Mandatory test architecture specification
├── specs/                            # Spec-Driven Development specifications
│   ├── META-FRAMEWORK.md             # SDD process definition and workflow
│   ├── PROJECT-SPEC.md               # Framework architecture and standards
│   └── templates/                    # Spec templates for new features
│       ├── page-objects.template.md
│       ├── unit-tests.template.md
│       ├── integration-tests.template.md
│       └── workflows.template.md
├── src/                              # Source code
│   ├── csharp_framework_demo.csproj  # Project file with dependencies
│   ├── allureConfig.json             # Allure reporting configuration
│   ├── Tests/                        # Test suites (78 tests total)
│   │   ├── PlaywrightTests.cs        # PlaywrightFixture and authentication E2E tests
│   │   ├── *PageUnitTests.cs         # Unit tests with Moq mocking
│   │   ├── *Tests.cs                 # Integration tests (real browser)
│   │   ├── *WorkflowTests.cs         # E2E workflow tests
│   │   └── SpecConformanceTests.cs   # SDD conformance validation
│   └── Utilities/
│       ├── PageObjects/              # Page Object Model classes
│       │   ├── LoginPage.cs
│       │   ├── InventoryPage.cs
│       │   ├── BurgerMenuPage.cs
│       │   ├── CartPage.cs
│       │   ├── CheckoutPage.cs
│       │   └── ProductDetailsPage.cs
│       └── SpecValidation/           # SDD conformance utilities
│           ├── SpecParser.cs
│           ├── SpecModels.cs
│           ├── CodeValidator.cs
│           └── ValidationResult.cs
├── .gitignore                        # Ignores .NET and Playwright artifacts
├── Dockerfile                        # Containerized build for testing
├── README.md                         # This documentation
└── csharp_framework_demo.sln         # Solution file
```

## Project Organization

This framework implements a **three-tier test architecture** with **Spec-Driven Development (SDD)** and **parallel test execution**. Here's how everything fits together:

### 1. Specifications (`specs/`)
**Purpose**: Define expected behavior before code implementation

- **[META-FRAMEWORK.md](specs/META-FRAMEWORK.md)**: SDD process definition
  - Spec-to-code mapping rules
  - Generation templates (Page Objects, Unit Tests, Integration Tests, E2E Tests)
  - Conformance validation rules
  - Drift detection strategy
  - Approval workflow

- **[PROJECT-SPEC.md](specs/PROJECT-SPEC.md)**: Framework architecture specification
  - Technology stack and constraints
  - Design patterns (Page Object Model, Test Pyramid)
  - Cross-cutting invariants (code standards, test standards)
  - Quality gates and external dependencies

- **[templates/](specs/templates/)**: Spec templates for new features
  - `page-objects.template.md`: Page Object behavioral contracts
  - `unit-tests.template.md`: Unit test scenarios (Moq-based)
  - `integration-tests.template.md`: Integration test scenarios (real browser)
  - `workflows.template.md`: E2E workflow scenarios (multi-step)

### 2. Source Code (`src/`)
**Purpose**: Implementation following spec definitions

#### Page Objects (`src/Utilities/PageObjects/`)
**Pattern**: Encapsulate page interactions using Page Object Model
- Each page/component has dedicated class (`LoginPage.cs`, `InventoryPage.cs`, etc.)
- Constructor accepts `IPage` via dependency injection
- All methods are async (`async Task` or `async Task<T>`)
- Handle timing internally (no hardcoded delays in tests)
- No direct assertions (separation of concerns)

#### Tests (`src/Tests/`)
**Three-tier architecture**:

1. **Unit Tests** (`*PageUnitTests.cs`)
   - Mock `IPage` using Moq
   - Validate Page Object method behavior
   - Fast, no browser required
   - Example: [LoginPageUnitTests.cs](src/Tests/LoginPageUnitTests.cs)

2. **Integration Tests** (`*Tests.cs`)
   - Real browser via Playwright
   - Validate Page Object + browser integration
   - Isolated browser contexts per test
   - Example: [BurgerMenuTests.cs](src/Tests/BurgerMenuTests.cs)

3. **E2E Workflow Tests** (`*WorkflowTests.cs`)
   - Complete user journeys
   - Multiple Page Objects
   - Allure step-by-step reporting
   - Example: [UserWorkflowTests](src/Tests/PlaywrightTests.cs)

#### Conformance Tests (`src/Tests/SpecConformanceTests.cs`)
**Purpose**: Validate spec-code alignment (SDD meta-tests)
- Verify spec files exist and are well-formed
- Validate templates are available
- (Future) Check generated code matches specs

#### Test Infrastructure (`src/Tests/PlaywrightTests.cs`)
**Components**:
- `PlaywrightFixture`: Manages browser lifecycle, provides factory method
- `PageContext`: Wraps `IBrowserContext` + `IPage` with auto-cleanup
- **Pattern**: Isolated browser contexts per test method (no race conditions)
- See [Test Architecture Spec](docs/test-architecture-spec.md) for details

### 3. Documentation (`docs/`)
**Purpose**: Capture learnings and architectural decisions

- **[agent-patterns.md](docs/agent-patterns.md)**: Reusable AI agent patterns
  - Spec Generation Agent (analyze code → generate specs)
  - Code Generation Agent (analyze specs → generate code)
  - Prompts and workflows for AI-assisted development

- **[retrospective-parallel-tests.md](docs/retrospective-parallel-tests.md)**: Detailed retrospective
  - Problem: CI failures due to shared page race conditions
  - Solution: Isolated browser contexts per test
  - Timeline, bugs fixed, lessons learned
  - 30+ failures → 0 failures

- **[test-architecture-spec.md](docs/test-architecture-spec.md)**: Mandatory architecture specification
  - Fixture pattern (PlaywrightFixture, PageContext)
  - Test class pattern (CreatePageContextAsync)
  - Page Object best practices (wait strategies, visibility checks)
  - Anti-patterns to avoid
  - Validation checklist for code review

### 4. CI/CD (`.github/workflows/ci.yml`)
**Purpose**: Automated testing and reporting

**Pipeline**:
1. Build Docker image with Playwright
2. Run all 78 tests in containerized environment
3. Generate Allure report with detailed test results
4. Run conformance tests (validate spec-code alignment)
5. Deploy Allure report to GitHub Pages (main branch only)
6. Upload report as artifact (all branches)

**Quality Gates**:
- All tests must pass before merge
- Conformance tests validate SDD compliance
- Docker ensures consistent environment across machines

### 5. Allure Reporting
**Purpose**: Rich, interactive test reports

**Features**:
- Test execution history and trends
- Step-by-step E2E test breakdown
- Test categorization (suites, features, tags)
- Screenshots and attachments (future)
- Hosted on GitHub Pages: [View Report](https://tallantm.github.io/csharp-framework-demo/)

## Testing Overview
- **Unit Tests**: Isolated verification of Page Object methods using Moq for mocking `IPage`, ensuring fast and deterministic checks without browser overhead. No browser required.
- **Integration Tests**: Validation of Page Object + Playwright browser interactions in isolated browser contexts. Each test gets its own `IBrowserContext` and `IPage` for parallel execution without race conditions.
- **E2E Tests**: Complete user workflow scenarios using multiple Page Objects with `AllureApi.Step()` for granular reporting. Tests real-world user journeys on [saucedemo.com](https://www.saucedemo.com/).

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

### Key Architectural Decisions

This framework was built with several critical architectural decisions that ensure reliability, maintainability, and scalability:

#### 1. Parallel Test Execution with Isolated Contexts
**Problem Solved**: Race conditions from shared browser pages causing 30+ CI failures

**Solution**: Each test method gets its own `IBrowserContext` and `IPage`
```csharp
// Every test follows this pattern:
await using var pageContext = await _fixture.CreatePageContextAsync();
var page = pageContext.Page;
// Test runs in complete isolation
```

**Benefits**:
- True parallelization (xUnit runs tests concurrently)
- No race conditions (each test has isolated page state)
- Automatic cleanup (`await using` ensures context closes)
- Shared browser process (performance) with isolated contexts (safety)

**Reference**: See [Test Architecture Spec](docs/test-architecture-spec.md) for full details

#### 2. Three-Tier Test Pyramid
**Strategy**: Balance speed, coverage, and confidence

```
        E2E Tests (24 tests)
       ↗ Complete workflows
      ↗  Real browser, multiple pages
     ↗   Slow but high confidence
    ────────────────────────────
   Integration Tests (42 tests)
  ↗ Page Object + browser
 ↗  Real browser, single feature
↗   Medium speed, good coverage
────────────────────────────────
     Unit Tests (12 tests)
    ↗ Mock IPage with Moq
   ↗  No browser, fast
  ↗   High speed, low confidence
─────────────────────────────────
```

**Why This Matters**:
- Unit tests catch logic errors fast (no browser startup)
- Integration tests validate browser interactions work correctly
- E2E tests ensure complete workflows succeed
- Total: 78 tests running in ~2-3 minutes in CI

#### 3. Page Object Model (POM)
**Pattern**: Encapsulate page interactions in reusable classes

**Benefits**:
- **DRY**: Login logic in one place (LoginPage.cs), used by 20+ tests
- **Maintainable**: Selector change = one file update, not 20 tests
- **Testable**: Unit test Page Objects with mocks before browser testing
- **Readable**: `await loginPage.LoginAsync(user, pass)` vs. multiple Fill/Click calls

**Standard**:
- Constructor accepts `IPage` via dependency injection
- All methods are async
- Handle timing internally (WaitForLoadState, WaitForSelector)
- Return meaningful data (bool for checks, string for text, void for actions)

#### 4. Spec-Driven Development (SDD)
**Workflow**: Specs → AI generates code → Human reviews → Sync maintenance

**Why SDD**:
- **Design Artifact**: Specs force thinking before coding
- **AI Leverage**: Claude Code generates code from specs (faster)
- **Documentation**: Specs stay synchronized with code (conformance tests)
- **Quality**: Human review catches AI errors, refines implementation

**Hybrid Approach**:
- AI generates initial implementation from specs
- Human reviews, approves, refines
- Code becomes source of truth (executable)
- Specs remain synchronized (conformance tests validate)

**Example Flow**:
1. Write `specs/features/authentication/page-objects.md`
2. Ask Claude Code: "Generate LoginPage.cs from spec"
3. Review generated code, make refinements
4. Update spec to match approved code
5. Commit both spec and code together
6. CI runs conformance tests to validate alignment

#### 5. Allure Reporting with Granular Steps
**Pattern**: Use `AllureApi.Step()` for detailed E2E test reporting

```csharp
await AllureApi.Step("Navigate to login page", async () =>
{
    await loginPage.NavigateToAsync("https://www.saucedemo.com/");
});

await AllureApi.Step("Enter valid credentials and login", async () =>
{
    await loginPage.LoginAsync("standard_user", "secret_sauce");
});
```

**Benefits**:
- **Debugging**: See exactly which step failed
- **Visualization**: Allure shows step-by-step execution flow
- **History**: Track test trends over time
- **Categorization**: Suite/Feature/Tag organization

**Access Report**: [Live Allure Report](https://tallantm.github.io/csharp-framework-demo/)

### Lessons Learned

Key insights from building this framework (see [Retrospective](docs/retrospective-parallel-tests.md) for full details):

1. **xUnit IClassFixture**: Creates ONE instance per test class, not per test method
   - ❌ Don't expose shared `IPage` in fixture
   - ✅ Expose factory method `CreatePageContextAsync()`

2. **Wait Strategies**: `LoadState.NetworkIdle` most flexible for Page Object methods
   - Works for both success (navigation) and failure (error message)
   - Avoid `WaitForURLAsync` when navigation is conditional

3. **Visibility Checks**: Check interactive elements (buttons), not containers
   - Containers may be in DOM but CSS-hidden
   - Buttons only visible when actually accessible

4. **Mock Alignment**: When changing Page Object implementation, update unit test mocks immediately
   - Prevents NullReferenceException in unit tests
   - Run unit tests locally before committing

5. **AI Agent Patterns**: Task tool with general-purpose agent highly effective for bulk refactoring
   - Refactored 78 test methods across 12 files consistently
   - Faster than manual (minutes vs. hours)
   - Human review still critical

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

### Quick Reference for Agents and Developers

**Implementing New Tests?** Follow these resources in order:

1. **[Test Architecture Spec](docs/test-architecture-spec.md)** (Mandatory)
   - Required fixture pattern (PlaywrightFixture, PageContext)
   - Test class template with `CreatePageContextAsync()`
   - Page Object best practices
   - Validation checklist

2. **[Agent Patterns](docs/agent-patterns.md)** (AI-Assisted Development)
   - Spec Generation Agent (code → specs)
   - Code Generation Agent (specs → code)
   - Reusable prompts and workflows

3. **[META-FRAMEWORK.md](specs/META-FRAMEWORK.md)** (SDD Process)
   - Spec-to-code mapping rules
   - Generation templates
   - Conformance validation

4. **[Retrospective](docs/retrospective-parallel-tests.md)** (Context and Lessons)
   - Why isolated contexts matter
   - Common pitfalls and solutions
   - 30+ failures → 0 failures journey

**Common Commands**:
```bash
# Run all tests
dotnet test

# Run specific test category
dotnet test --filter "FullyQualifiedName~UnitTests"
dotnet test --filter "FullyQualifiedName~IntegrationTests"
dotnet test --filter "FullyQualifiedName~WorkflowTests"

# Run conformance tests (SDD validation)
dotnet test --filter "FullyQualifiedName~SpecConformanceTests"

# Build and run in Docker (matches CI environment)
docker build -t csharp_framework_demo .
docker run csharp_framework_demo

# Generate Allure report locally
allure generate allure-results -o allure-report
allure open allure-report
```

**Current Status** (as of 2026-02-10):
- ✅ **78 tests passing** in CI
- ✅ **Parallel execution** working (isolated contexts)
- ✅ **SDD meta-framework** established (specs, templates, conformance tests)
- ✅ **Documentation** complete (architecture, retrospective, agent patterns)
- ✅ **CI/CD** with Allure reporting on GitHub Pages
- 🚧 **Feature specs** (authentication, cart, checkout) - to be created using templates

## Troubleshooting
- **Browser Launch Failures**: Ensure `playwright install` has run successfully. If using Docker, verify all dependencies are included in the Dockerfile.
- **Dependency Errors**: Run `dotnet restore` to refresh packages. For Docker builds, check for network issues during library installations.
- **Test Timeouts**: Increase timeouts in Playwright options if network latency affects external sites like saucedemo.com.
- **CI Failures**: Review workflow logs for specific errors; caching may need invalidation if dependencies change.

## Contributing
Contributions are welcome to enhance the demo. Please fork the repository, create a feature branch, and submit a pull request with clear descriptions of changes.
