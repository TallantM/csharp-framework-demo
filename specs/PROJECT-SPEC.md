# C# Playwright Test Framework - Project Specification

## System Purpose

Automated UI testing framework for web applications using Microsoft Playwright, providing reliable, maintainable test coverage across three test layers (unit, integration, end-to-end) with comprehensive Allure reporting and CI/CD integration.

**Primary Goals**:
- Validate web application functionality through browser automation
- Provide fast feedback through layered testing (test pyramid)
- Generate detailed, visual test reports (Allure)
- Run reliably in CI/CD pipelines (Docker + GitHub Actions)
- Maintain clean, scalable test architecture (Page Object Model)

---

## Architectural Constraints

### Technology Stack

| Component | Technology | Version | Purpose |
|-----------|------------|---------|---------|
| **Runtime** | .NET | 8.0 LTS | Modern C# features, long-term support |
| **Browser Automation** | Microsoft.Playwright | >= 1.57.0 | Cross-browser testing (Chromium, Firefox, WebKit) |
| **Test Framework** | xUnit | >= 2.9.0 | Unit/integration/E2E test execution |
| **Mocking** | Moq | >= 4.20.0 | Unit test mocking for IPage interface |
| **Reporting** | Allure.Xunit | >= 2.14.0 | Rich test reports with steps, attachments, history |
| **Code Coverage** | coverlet.collector | >= 6.0.0 | Coverage data collection (not yet configured) |
| **Test SDK** | Microsoft.NET.Test.Sdk | >= 17.11.0 | Test infrastructure |

**Package References** (from `src/csharp_framework_demo.csproj`):
```xml
<PackageReference Include="Microsoft.Playwright" Version="1.57.0" />
<PackageReference Include="xunit" Version="2.9.2" />
<PackageReference Include="Moq" Version="4.20.72" />
<PackageReference Include="Allure.Xunit" Version="2.14.1" />
<PackageReference Include="coverlet.collector" Version="6.0.2" />
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.11.1" />
<PackageReference Include="xunit.runner.visualstudio" Version="2.8.2" />
```

### Design Patterns

#### 1. Page Object Model (POM)

**Definition**: Encapsulate page interactions in reusable classes, separating test logic from UI implementation details.

**Structure**:
- **Location**: `src/Utilities/PageObjects/`
- **Naming**: `{PageName}Page.cs` (e.g., `LoginPage.cs`, `InventoryPage.cs`)
- **Responsibility**: Single page or logical UI component
- **Constructor**: Accepts `IPage` via dependency injection
- **Methods**: All async, return `Task` or `Task<T>`
- **No Assertions**: Page Objects perform actions, tests perform assertions

**Example**:
```csharp
// src/Utilities/PageObjects/LoginPage.cs
public class LoginPage
{
    private readonly IPage _page;

    public LoginPage(IPage page) => _page = page;

    public async Task NavigateToAsync(string url)
        => await _page.GotoAsync(url);

    public async Task LoginAsync(string username, string password)
    {
        await _page.FillAsync("[data-test='username']", username);
        await _page.FillAsync("[data-test='password']", password);
        await _page.ClickAsync("[data-test='login-button']");
    }
}
```

**Benefits**:
- Reusability across tests
- Single point of change when UI changes
- Improved readability (tests use business language)
- Easier maintenance

---

#### 2. Test Pyramid Architecture

**Definition**: Three-layer testing strategy balancing speed, cost, and confidence.

```
         ╱╲
        ╱E2E╲         ← Few, slow, high confidence
       ╱──────╲
      ╱ Integ. ╲      ← More, moderate speed, integration validation
     ╱──────────╲
    ╱   Unit     ╲    ← Many, fast, low-level validation
   ╱──────────────╲
```

**Layer 1: Unit Tests**
- **Purpose**: Validate Page Object methods in isolation
- **Speed**: Very fast (no browser)
- **Tool**: Moq for mocking `IPage`
- **Coverage**: Page Object method logic
- **Example**: `LoginPageUnitTests.cs`
- **Location**: `src/Tests/{PageName}PageUnitTests.cs`
- **Attributes**: `[AllureSuite("Unit Tests")]`, `[AllureFeature("{Feature}")]`

**Layer 2: Integration Tests**
- **Purpose**: Validate Page Object + Playwright integration with real browser
- **Speed**: Moderate (real browser, single-page interactions)
- **Tool**: PlaywrightFixture with real `IPage`
- **Coverage**: Page Object + browser interaction
- **Example**: `LoginPageIntegrationTests.cs`
- **Location**: `src/Tests/{PageName}PageIntegrationTests.cs`
- **Attributes**: `[AllureSuite("Integration Tests")]`, `[AllureFeature("{Feature}")]`

**Layer 3: End-to-End Tests**
- **Purpose**: Validate complete user workflows across multiple pages
- **Speed**: Slow (full browser workflows)
- **Tool**: PlaywrightFixture + multiple Page Objects
- **Coverage**: Multi-page user journeys
- **Example**: `UserWorkflowTests.cs` (authentication workflows)
- **Location**: `src/Tests/{Feature}WorkflowTests.cs`
- **Attributes**: `[AllureSuite("End-to-End Tests")]`, `[AllureFeature("{Feature}")]`

---

#### 3. Fixture Pattern (xUnit IAsyncLifetime)

**Definition**: Manage shared resources (browser, page) across tests with proper setup/teardown.

**Implementation**: `PlaywrightFixture` class

**Location**: `src/Tests/PlaywrightTests.cs` (currently embedded, could be extracted)

**Structure**:
```csharp
public class PlaywrightFixture : IAsyncLifetime
{
    public IPlaywright Playwright { get; private set; } = null!;
    public IBrowser Browser { get; private set; } = null!;
    public IPage Page { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        Browser = await Playwright.Chromium.LaunchAsync(new() { Headless = true });
        Page = await Browser.NewPageAsync();
    }

    public async Task DisposeAsync()
    {
        await Browser.CloseAsync();
        Playwright.Dispose();
    }
}
```

**Usage in Tests**:
```csharp
public class IntegrationTest : IClassFixture<PlaywrightFixture>
{
    private readonly IPage _page;

    public IntegrationTest(PlaywrightFixture fixture)
    {
        _page = fixture.Page;
    }
}
```

**Benefits**:
- Browser lifecycle managed automatically
- Page instance shared across tests in a class
- Proper async initialization and cleanup
- xUnit guarantees fixture disposal

---

#### 4. AAA Pattern (Arrange-Act-Assert)

**Definition**: Structure tests with three clear sections for readability and maintainability.

**Structure**:
```csharp
[Fact]
public async Task TestMethodName()
{
    // Arrange - Set up test data and preconditions
    var loginPage = new LoginPage(_page);
    await loginPage.NavigateToAsync("https://example.com/");

    // Act - Perform the action being tested
    await loginPage.LoginAsync("user", "password");

    // Assert - Verify expected outcomes
    await Assertions.Expect(_page).ToHaveURLAsync("https://example.com/dashboard");
}
```

**For E2E Tests**: Use `AllureApi.Step()` to wrap each section:
```csharp
[Fact]
public async Task E2EWorkflow()
{
    await AllureApi.Step("Arrange: Navigate to login page", async () =>
    {
        await _loginPage.NavigateToAsync("https://example.com/");
    });

    await AllureApi.Step("Act: Login with valid credentials", async () =>
    {
        await _loginPage.LoginAsync("user", "password");
    });

    await AllureApi.Step("Assert: Verify redirect to dashboard", async () =>
    {
        await Assertions.Expect(_page).ToHaveURLAsync("https://example.com/dashboard");
    });
}
```

---

## Cross-Cutting Invariants

### Code Standards

#### Async/Await Requirement
**Invariant**: All Playwright interactions MUST use async/await pattern.

**Rationale**: Playwright API is async-only; blocking calls cause deadlocks.

✅ **Correct**:
```csharp
public async Task ClickButtonAsync()
{
    await _page.ClickAsync("#button");
}
```

❌ **Incorrect**:
```csharp
public void ClickButton()
{
    _page.ClickAsync("#button").Wait(); // NEVER DO THIS
}
```

---

#### Nullable Reference Types
**Invariant**: Nullable reference types enabled in project (`<Nullable>enable</Nullable>`).

**Effect**: Compiler warnings for potential null references.

**Handling**:
- Initialize non-nullable fields: `= null!;` (with guarantee of initialization in constructor/method)
- Use nullable types when appropriate: `string?`, `IPage?`
- Check for null before use: `if (value != null)`

---

#### Implicit Usings
**Invariant**: Implicit usings enabled (`<ImplicitUsings>enable</ImplicitUsings>`).

**Effect**: Common namespaces automatically imported (Xunit, System, etc.).

**Explicit Usings Still Required**:
```csharp
using Microsoft.Playwright;
using Allure.Net.Commons;
using Allure.Xunit.Attributes;
using csharp_framework_demo.Utilities.PageObjects;
```

---

#### Naming Conventions

| Element | Convention | Example |
|---------|------------|---------|
| **Classes** | PascalCase | `LoginPage`, `UserWorkflowTests` |
| **Methods** | PascalCase | `NavigateToAsync`, `SuccessfulLogin` |
| **Test Methods** | PascalCase with underscores for readability | `FailedLogin_InvalidCredentials`, `LoginAsync_WithValidCredentials_Succeeds` |
| **Private Fields** | _camelCase (underscore prefix) | `_page`, `_loginPage`, `_browser` |
| **Parameters** | camelCase | `username`, `password`, `url` |
| **Constants** | PascalCase | `DefaultTimeout`, `BaseUrl` |
| **Namespaces** | PascalCase, dot-separated | `csharp_framework_demo.Utilities.PageObjects` |

---

### Test Standards

#### Test Attributes (Required)

**All Test Classes**:
```csharp
[AllureSuite("Unit Tests" | "Integration Tests" | "End-to-End Tests")]
[AllureFeature("{Feature Name}")]
public class TestClass { }
```

**All Test Methods**:
```csharp
[Fact]
[AllureDescription("Clear description of what this test validates")]
[AllureSeverity(SeverityLevel.critical | normal | minor)]
[AllureOwner("QA Team")]
[AllureTag("Smoke" | "Regression" | "Negative" | etc.)]
public async Task TestMethod() { }
```

**Severity Guidelines**:
- `critical`: Smoke tests, core functionality, blocking issues
- `normal`: Regression tests, standard functionality
- `minor`: Edge cases, minor features

---

#### Allure Steps (E2E Tests)

**Requirement**: E2E tests MUST wrap actions in `AllureApi.Step()`.

**Purpose**: Granular reporting, step-by-step execution visibility.

**Pattern**:
```csharp
await AllureApi.Step("Step description", async () =>
{
    // Step actions and assertions
});
```

**Benefits**:
- Detailed Allure report shows which step failed
- Clear test execution flow
- Screenshots/attachments can be added to specific steps

---

#### Assertions

**xUnit Assertions** (unit tests, simple checks):
```csharp
Assert.True(condition);
Assert.False(condition);
Assert.Equal(expected, actual);
Assert.NotNull(value);
```

**Playwright Assertions** (integration/E2E, browser state):
```csharp
await Assertions.Expect(_page).ToHaveURLAsync(url);
await Assertions.Expect(element).ToBeVisibleAsync();
await Assertions.Expect(element).ToHaveTextAsync(text);
```

**When to Use Each**:
- **xUnit**: Boolean conditions, value comparisons, null checks
- **Playwright**: Page state, element visibility, URL validation, text content

---

#### Test Isolation

**Invariant**: Each test is independent and can run in any order.

**Rules**:
- No test depends on another test running first
- No shared state between tests (except fixture-managed resources)
- Each test can run in isolation
- Tests can run in parallel (when configured)

**Current State**: Tests run sequentially (xUnit default). Parallelization can be enabled in future.

---

#### Browser Configuration

**CI Environment**: Headless mode (no GUI)
```csharp
Browser = await Playwright.Chromium.LaunchAsync(new() { Headless = true });
```

**Local Development**: Can be configured for headed mode for debugging
```csharp
Browser = await Playwright.Chromium.LaunchAsync(new() { Headless = false });
```

**Future Enhancement**: Make headless mode configurable via environment variable.

---

### Page Object Standards

#### Constructor Injection

**Invariant**: Page Objects MUST accept `IPage` in constructor.

```csharp
private readonly IPage _page;

public LoginPage(IPage page)
{
    _page = page;
}
```

**Rationale**: Dependency injection enables unit testing (mocking) and decouples Page Objects from browser lifecycle.

---

#### Method Signatures

**Invariant**: All Playwright interaction methods MUST be async.

**Pattern**:
```csharp
public async Task MethodNameAsync(parameters)
{
    await _page.SomeActionAsync(...);
}
```

**Return Types**:
- `Task`: Actions with no return value (click, fill, navigate)
- `Task<bool>`: Visibility checks (`IsVisibleAsync`)
- `Task<string>`: Text retrieval (`TextContentAsync`, `GetAttributeAsync`)
- `Task<T>`: Other queries returning typed data

---

#### Selectors

**Priority Order**:
1. **data-test attributes** (best - stable, explicit test hooks)
   ```csharp
   await _page.ClickAsync("[data-test='login-button']");
   ```

2. **ID selectors** (good - unique, stable)
   ```csharp
   await _page.ClickAsync("#username");
   ```

3. **CSS selectors** (acceptable - should be stable classes)
   ```csharp
   await _page.ClickAsync(".inventory_list");
   ```

4. **Text selectors** (use sparingly - prone to i18n issues)
   ```csharp
   await _page.ClickAsync("text=Login");
   ```

**Storage**: Currently embedded in methods. Future enhancement: extract to constants or properties.

---

#### No Assertions in Page Objects

**Invariant**: Page Objects perform actions; tests perform assertions.

✅ **Correct** (test file):
```csharp
await _loginPage.LoginAsync("user", "pass");
await Assertions.Expect(_page).ToHaveURLAsync("...inventory.html");
```

❌ **Incorrect** (Page Object):
```csharp
public async Task LoginAsync(string user, string pass)
{
    await _page.FillAsync(...);
    await _page.ClickAsync(...);
    Assert.Equal("...inventory.html", _page.Url); // DON'T DO THIS
}
```

**Rationale**: Separation of concerns, reusability (different tests may have different expectations).

---

#### Return Values

**Guideline**: Return meaningful data that tests can assert on.

**Examples**:
```csharp
// Visibility check
public async Task<bool> IsErrorMessageVisibleAsync()
    => await _page.IsVisibleAsync("[data-test='error']");

// Text retrieval
public async Task<string> GetErrorMessageAsync()
    => await _page.TextContentAsync("[data-test='error']") ?? "";

// Action (no return)
public async Task LoginAsync(string username, string password)
{
    await _page.FillAsync("[data-test='username']", username);
    await _page.FillAsync("[data-test='password']", password);
    await _page.ClickAsync("[data-test='login-button']");
}
```

---

## Quality Gates

### Code Coverage

**Current State**: coverlet.collector installed but not configured in CI.

**Minimum**: Not enforced initially.

**Target**: 80% for Page Objects (via unit tests).

**Future Enhancement**: Configure coverlet to generate coverage reports in CI, add quality gate to enforce minimum coverage.

---

### Test Execution

**Requirements**:
- All tests MUST pass before merge
- No test may exceed 60-second timeout (xUnit default)
- Tests MUST be deterministic (no flakiness)
- All tests run in Docker before merge

**Current Test Count**:
- Unit Tests: 2 (LoginPageUnitTests)
- Integration Tests: 2 (LoginPageIntegrationTests)
- E2E Tests: 6 (UserWorkflowTests - authentication scenarios)

---

### CI/CD Requirements

**Build Steps** (`.github/workflows/ci.yml`):
1. ✅ Build must succeed on Ubuntu (GitHub Actions runner)
2. ✅ Docker image must build successfully
3. ✅ Tests must pass in Docker container
4. ✅ Allure report must generate successfully
5. ✅ GitHub Pages deployment must succeed (main branch only)
6. 🆕 Conformance tests must pass (SDD addition)

**Environment**:
- OS: Ubuntu latest (GitHub Actions)
- .NET: 8.0.x
- Node.js: 20 (for Allure CLI)
- Docker: Playwright .NET image (playwright:v1.57.0-jammy)

---

### Performance

**Page Load Assertions**: < 3 seconds (not currently enforced)

**Test Suite Execution**: < 5 minutes total in CI (current: ~2-3 minutes)

**Future Enhancements**:
- Add performance assertions to tests
- Monitor test execution time trends
- Optimize slow tests

---

## External Dependencies

### Application Under Test

**Target**: https://www.saucedemo.com/

**Type**: External demo site (not controlled by this project)

**Risk**: Tests may fail if site is unavailable or changes

**Credentials**: Test users provided by SauceDemo
- `standard_user` / `secret_sauce` (valid user)
- `locked_out_user` / `secret_sauce` (locked user)
- `invalid_user` / `wrong_password` (invalid credentials)

**Mitigation**: Consider adding health check before tests, or mock backend for critical tests.

---

### CI/CD Services

| Service | Purpose | Dependency Risk |
|---------|---------|----------------|
| **GitHub Actions** | CI/CD pipeline execution | Low (GitHub uptime ~99.9%) |
| **Docker Hub** | Base Playwright image | Low (cached in CI) |
| **GitHub Pages** | Allure report hosting | Low (integrated with GitHub) |
| **NPM** | Allure CLI installation | Medium (npmjs.com availability) |
| **NuGet** | .NET package restoration | Medium (nuget.org availability, mitigated by caching) |

---

## Security Standards

### 1. No Hardcoded Credentials

**Current Violation**: Credentials hardcoded in tests.

**Issue**:
```csharp
await _loginPage.LoginAsync("standard_user", "secret_sauce"); // Hardcoded
```

**Future Requirement**: Move to configuration/environment variables.

**Mitigation**:
```csharp
// Future approach:
var username = Configuration.GetValue<string>("TestUsers:StandardUser:Username");
var password = Configuration.GetValue<string>("TestUsers:StandardUser:Password");
await _loginPage.LoginAsync(username, password);
```

---

### 2. No Sensitive Data in Logs

**Requirement**: Mask passwords in Allure reports and logs.

**Current State**: Passwords visible in Allure step descriptions.

**Future Enhancement**: Implement sensitive data masking in Allure steps.

---

### 3. Dependency Scanning

**Future Consideration**: Add Dependabot or Snyk for vulnerability scanning.

**Current State**: Manual package updates.

---

### 4. Docker Image Security

**Current**: Uses official Microsoft Playwright image (`mcr.microsoft.com/playwright/dotnet:v1.57.0-jammy`)

**Best Practice**: Use official images, scan for vulnerabilities.

**Future Enhancement**: Add container vulnerability scanning in CI.

---

## Error Handling

### 1. Test Failures

**Current**: Tests fail with exception, Allure reports failure.

**Future Enhancement**: Capture screenshots on failure.

**Implementation**:
```csharp
try
{
    // Test code
}
catch (Exception ex)
{
    var screenshot = await _page.ScreenshotAsync();
    AllureApi.AddAttachment("Failure Screenshot", "image/png", screenshot);
    throw;
}
```

---

### 2. Timeout Handling

**Guideline**: Use appropriate timeouts for Playwright operations.

**Defaults**:
- Page load: 30 seconds (Playwright default)
- Element wait: 30 seconds (Playwright default)
- Test timeout: 60 seconds (xUnit default)

**Explicit Timeouts** (when needed):
```csharp
await _page.ClickAsync("#button", new() { Timeout = 10000 }); // 10 seconds
```

---

### 3. Exception Handling

**Guideline**: Let tests fail fast; no try-catch unless testing error scenarios.

✅ **Correct** (testing error scenario):
```csharp
[Fact]
public async Task InvalidLogin_ThrowsException()
{
    var exception = await Assert.ThrowsAsync<PlaywrightException>(async () =>
    {
        await _loginPage.LoginAsync("invalid", "wrong");
    });
}
```

❌ **Incorrect** (hiding failures):
```csharp
[Fact]
public async Task Test()
{
    try
    {
        await _loginPage.LoginAsync("user", "pass");
    }
    catch { } // Swallows errors - DON'T DO THIS
}
```

---

### 4. Logging

**Current State**: Allure step descriptions serve as execution log.

**Future Enhancement**: Add structured logging (Serilog or NLog).

---

## Configuration Management

### Current Configuration Files

| File | Purpose | Format |
|------|---------|--------|
| **allureConfig.json** | Allure framework configuration | JSON |
| **Dockerfile** | Test environment configuration | Dockerfile |
| **ci.yml** | CI/CD pipeline configuration | YAML |
| **csproj** | NuGet dependencies and project settings | XML |

### Missing Configuration

**Need**: `appsettings.json` for centralized test configuration.

**Future Content**:
```json
{
  "BaseUrl": "https://www.saucedemo.com/",
  "BrowserSettings": {
    "Headless": true,
    "Timeout": 30000
  },
  "TestUsers": {
    "StandardUser": {
      "Username": "standard_user",
      "Password": "secret_sauce"
    },
    "LockedUser": {
      "Username": "locked_out_user",
      "Password": "secret_sauce"
    }
  }
}
```

---

## Future Enhancements

### 1. Configuration System
**Goal**: Add `appsettings.json` for test data externalization.
**Benefit**: No hardcoded URLs/credentials, environment-specific configs.

### 2. Code Coverage Reporting
**Goal**: Configure coverlet to generate and publish coverage reports in CI.
**Benefit**: Visibility into test coverage, enforce minimum thresholds.

### 3. Multi-Browser Testing
**Goal**: Test on Chrome, Firefox, WebKit.
**Implementation**: Parameterize PlaywrightFixture with browser type.

### 4. Parallel Execution
**Goal**: Configure xUnit for parallel test execution.
**Benefit**: Faster test suite execution.

### 5. Retry Logic
**Goal**: Add retry policies for flaky tests (Polly library).
**Benefit**: Reduce false failures from transient issues.

### 6. Screenshot Capture
**Goal**: Automatic screenshots on test failure.
**Benefit**: Easier debugging of failures.

### 7. Test Data Management
**Goal**: External JSON/CSV files for test data.
**Benefit**: Data-driven tests, easier test data maintenance.

### 8. API Testing
**Goal**: Add API tests alongside UI tests (RestSharp/HttpClient).
**Benefit**: Faster feedback, validate backend independently.

---

## Summary

This specification defines the architecture, patterns, and standards for the C# Playwright test automation framework:

✅ **Modern Stack**: .NET 8.0, Playwright, xUnit, Allure
✅ **Proven Patterns**: Page Object Model, Test Pyramid, Fixture Pattern, AAA
✅ **Quality Focus**: Three test layers, CI/CD integration, comprehensive reporting
✅ **Future-Ready**: Clear enhancement roadmap, scalable architecture

**Compliance**: All code MUST adhere to the standards defined in this specification. Conformance tests validate alignment.
