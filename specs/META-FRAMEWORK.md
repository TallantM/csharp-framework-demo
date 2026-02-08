# Spec-Driven Development Meta-Framework

## Overview

This document defines how **Spec-Driven Development (SDD)** works within this C# Playwright test automation framework.

### What is SDD for This Project?

Spec-Driven Development is a development paradigm where:
1. **Specifications define expected behavior** - Detailed markdown specs describe what code should do
2. **AI agents generate initial code** - Tools like Claude Code transform specs into executable C# code
3. **Humans review and approve** - Developers validate, refine, and approve generated code
4. **Both artifacts are maintained** - Specs and code stay synchronized through conformance testing
5. **Drift detection prevents divergence** - Automated tests ensure spec-code alignment

### Role of Specs vs. Code

**Specifications (Specs)**:
- **Design artifacts** - Document intent before implementation
- **Source of truth for behavior** - Define what should happen
- **Living documentation** - Maintained alongside code
- **AI generation input** - Provide context for code generation

**Code**:
- **Executable implementation** - The running artifact
- **Source of truth after approval** - Once reviewed and approved, code is authoritative
- **Validated against specs** - Must align with spec contracts
- **Maintained with specs** - Changes to code should update specs (and vice versa)

### Hybrid Workflow

This framework uses a **pragmatic hybrid approach** that balances spec-as-truth and code-as-truth:

```
Initial Development (Spec → Code):
1. Write spec → 2. AI generates code → 3. Review/refine → 4. Approve → 5. Both synchronized

Maintenance (Bidirectional Sync):
Code changes? → Update spec to match
Spec changes? → Update/regenerate code
Conformance tests enforce alignment
```

---

## Spec-to-Code Mapping Rules

### 1. Page Object Specs → Page Object Classes

**Spec Format**: `specs/features/{feature}/page-objects.md`

Defines Page Object behavioral contracts:
- Class name and responsibility
- Methods with inputs, outputs, and behavior
- Locators and selectors
- Invariants and preconditions

**Maps To**: `src/Utilities/PageObjects/{PageName}Page.cs`

**Example Mapping**:

| Spec Element | Code Element | Requirement |
|--------------|--------------|-------------|
| Page Object Class: `LoginPage` | `public class LoginPage` | Exact name match |
| Method: `NavigateToAsync(url)` | `public async Task NavigateToAsync(string url)` | Signature match, async pattern |
| Method: `LoginAsync(username, password)` | `public async Task LoginAsync(string username, string password)` | Parameter names/types match |
| Locator: `[data-test='username']` | Embedded in method or as private const | Must be used correctly |
| Dependency: Requires IPage | `public LoginPage(IPage page)` | Constructor injection |

**Required Elements**:
- Namespace: `csharp_framework_demo.Utilities.PageObjects`
- Constructor: Accepts `IPage` parameter
- Methods: All Playwright interactions are `async`
- Return types: `Task` for actions, `Task<T>` for queries
- Naming: `{PageName}Page` (PascalCase with "Page" suffix)

---

### 2. Unit Test Specs → Unit Test Classes

**Spec Format**: `specs/features/{feature}/unit-tests.md`

Defines unit test scenarios using Given/When/Then format:
- Test scenario name and description
- Setup (Given) - Initial conditions
- Action (When) - Method being tested
- Verification (Then) - Expected behavior
- Mocking strategy

**Maps To**: `src/Tests/{PageName}PageUnitTests.cs`

**Example Mapping**:

| Spec Element | Code Element | Requirement |
|--------------|--------------|-------------|
| Suite: "Unit Tests" | `[AllureSuite("Unit Tests")]` | Exact attribute value |
| Feature: "Login Page Object" | `[AllureFeature("Login Page Object")]` | Matches spec feature name |
| Scenario: "NavigateToAsync navigates to URL" | `[Fact] public async Task NavigateToAsync_NavigatesToUrl()` | Method name reflects scenario |
| Given: Mock IPage | `var mockPage = new Mock<IPage>();` | Uses Moq |
| When: Call NavigateToAsync | `await loginPage.NavigateToAsync(url);` | Calls method under test |
| Then: Verify GotoAsync called | `mockPage.Verify(p => p.GotoAsync(url, null), Times.Once);` | Assertion matches expected behavior |

**Required Elements**:
- Namespace: `csharp_framework_demo.Tests`
- Class attributes: `[AllureSuite("Unit Tests")]`, `[AllureFeature("{Feature}")]`
- Test methods: `[Fact]`, `[AllureDescription]`, `[AllureSeverity]`, `[AllureOwner]`, `[AllureTag]`
- Mocking: Uses `Moq` library, creates `Mock<IPage>`
- Verification: Uses `Mock.Verify()` to assert method calls
- No browser: Unit tests do not use PlaywrightFixture

---

### 3. Integration Test Specs → Integration Test Classes

**Spec Format**: `specs/features/{feature}/integration-tests.md`

Defines integration test scenarios with real browser:
- Test scenario name and description
- Setup (Given) - Browser/page state
- Actions (When) - Page Object method calls
- Verification (Then) - Actual browser state
- Expected behavior

**Maps To**: `src/Tests/{PageName}PageIntegrationTests.cs`

**Example Mapping**:

| Spec Element | Code Element | Requirement |
|--------------|--------------|-------------|
| Suite: "Integration Tests" | `[AllureSuite("Integration Tests")]` | Exact attribute value |
| Feature: "Login Page Object" | `[AllureFeature("Login Page Object")]` | Matches spec feature name |
| Scenario: "Successful login redirects to inventory" | `[Fact] public async Task SuccessfulLogin_RedirectsToInventory()` | Descriptive method name |
| Given: Navigate to login page | `await _loginPage.NavigateToAsync(url);` | Uses Page Object |
| When: Login with valid credentials | `await _loginPage.LoginAsync("user", "pass");` | Calls Page Object method |
| Then: URL is inventory page | `await Assertions.Expect(_page).ToHaveURLAsync("...inventory.html");` | Playwright assertion |

**Required Elements**:
- Namespace: `csharp_framework_demo.Tests`
- Class attributes: `[AllureSuite("Integration Tests")]`, `[AllureFeature("{Feature}")]`
- Fixture: `IClassFixture<PlaywrightFixture>`
- Constructor: Accepts `PlaywrightFixture fixture`, stores `IPage _page`
- Test methods: `[Fact]`, Allure attributes
- Real browser: Uses actual Playwright Page instance
- Page Object usage: Creates and uses Page Objects with injected `IPage`
- Assertions: Mix of xUnit `Assert` and Playwright `Assertions.Expect`

---

### 4. Workflow Specs → E2E Test Classes

**Spec Format**: `specs/features/{feature}/workflows.md`

Defines end-to-end user workflow scenarios:
- Workflow name and business goal
- Multi-step user journey (Given/When/Then for each step)
- Multiple Page Object interactions
- Expected end state
- Edge cases and error scenarios

**Maps To**: `src/Tests/{Feature}WorkflowTests.cs`

**Example Mapping**:

| Spec Element | Code Element | Requirement |
|--------------|--------------|-------------|
| Suite: "End-to-End Tests" | `[AllureSuite("End-to-End Tests")]` | Exact attribute value |
| Feature: "Authentication" | `[AllureFeature("Authentication")]` | Matches spec feature name |
| Workflow: "User logs in and browses products" | `[Fact] public async Task UserLoginAndBrowseProducts()` | Descriptive workflow name |
| Step: "Navigate to login page" | `await AllureApi.Step("Navigate to login page", async () => { ... });` | Wrapped in AllureApi.Step |
| Step: "Login with valid credentials" | `await AllureApi.Step("Login with valid credentials", async () => { ... });` | Each action is a step |
| Step: "Verify inventory visible" | `await AllureApi.Step("Verify inventory visible", async () => { ... });` | Assertions within steps |

**Required Elements**:
- Namespace: `csharp_framework_demo.Tests`
- Class attributes: `[AllureSuite("End-to-End Tests")]`, `[AllureFeature("{Feature}")]`
- Fixture: `IClassFixture<PlaywrightFixture>`
- Test methods: `[Fact]`, Allure attributes with severity, owner, tags
- Allure steps: All actions wrapped in `AllureApi.Step("description", async () => { ... })`
- Multiple Page Objects: Uses several Page Object classes in one test
- Complete flows: Tests full user journeys from start to finish
- Direct selectors allowed: E2E tests may use direct selectors for elements not yet in Page Objects

---

## Generation Templates

### Template: Page Object Class

```csharp
using Microsoft.Playwright;

namespace csharp_framework_demo.Utilities.PageObjects;

/// <summary>
/// Page Object for {PageName} page
/// Encapsulates interactions with {page description}
/// </summary>
public class {PageName}Page
{
    private readonly IPage _page;

    /// <summary>
    /// Initializes a new instance of {PageName}Page
    /// </summary>
    /// <param name="page">Playwright IPage instance</param>
    public {PageName}Page(IPage page)
    {
        _page = page;
    }

    // TODO: Add methods from spec behavioral contracts
    // Example:
    // public async Task NavigateToAsync(string url)
    // {
    //     await _page.GotoAsync(url);
    // }
}
```

### Template: Unit Test Class

```csharp
using Moq;
using Microsoft.Playwright;
using Xunit;
using Allure.Xunit.Attributes;
using csharp_framework_demo.Utilities.PageObjects;

namespace csharp_framework_demo.Tests;

/// <summary>
/// Unit tests for {PageName}Page class
/// Validates Page Object methods using mocked IPage
/// </summary>
[AllureSuite("Unit Tests")]
[AllureFeature("{Feature}")]
public class {PageName}PageUnitTests
{
    // TODO: Add unit test methods from spec scenarios
    // Example:
    // [Fact]
    // [AllureDescription("Verifies that NavigateToAsync calls GotoAsync on IPage")]
    // public async Task NavigateToAsync_CallsGotoAsync()
    // {
    //     // Arrange
    //     var mockPage = new Mock<IPage>();
    //     var loginPage = new LoginPage(mockPage.Object);
    //     var url = "https://example.com/";
    //
    //     // Act
    //     await loginPage.NavigateToAsync(url);
    //
    //     // Assert
    //     mockPage.Verify(p => p.GotoAsync(url, null), Times.Once);
    // }
}
```

### Template: Integration Test Class

```csharp
using Microsoft.Playwright;
using Xunit;
using Allure.Xunit.Attributes;
using csharp_framework_demo.Utilities.PageObjects;

namespace csharp_framework_demo.Tests;

/// <summary>
/// Integration tests for {PageName}Page class
/// Validates Page Object with real Playwright browser
/// </summary>
[AllureSuite("Integration Tests")]
[AllureFeature("{Feature}")]
public class {PageName}PageIntegrationTests : IClassFixture<PlaywrightFixture>
{
    private readonly IPage _page;

    public {PageName}PageIntegrationTests(PlaywrightFixture fixture)
    {
        _page = fixture.Page;
    }

    // TODO: Add integration test methods from spec scenarios
    // Example:
    // [Fact]
    // [AllureDescription("Verifies successful login with valid credentials")]
    // public async Task LoginAsync_WithValidCredentials_Succeeds()
    // {
    //     // Arrange
    //     var loginPage = new LoginPage(_page);
    //     await loginPage.NavigateToAsync("https://www.saucedemo.com/");
    //
    //     // Act
    //     await loginPage.LoginAsync("standard_user", "secret_sauce");
    //
    //     // Assert
    //     await Assertions.Expect(_page).ToHaveURLAsync("https://www.saucedemo.com/inventory.html");
    // }
}
```

### Template: E2E Workflow Test Class

```csharp
using Microsoft.Playwright;
using Xunit;
using Allure.Net.Commons;
using Allure.Xunit.Attributes;
using csharp_framework_demo.Utilities.PageObjects;

namespace csharp_framework_demo.Tests;

/// <summary>
/// End-to-end workflow tests for {Feature} feature
/// Validates complete user journeys across multiple pages
/// </summary>
[AllureSuite("End-to-End Tests")]
[AllureFeature("{Feature}")]
public class {Feature}WorkflowTests : IClassFixture<PlaywrightFixture>
{
    private readonly IPage _page;

    public {Feature}WorkflowTests(PlaywrightFixture fixture)
    {
        _page = fixture.Page;
    }

    // TODO: Add E2E workflow methods from spec scenarios
    // Example:
    // [Fact]
    // [AllureDescription("Verifies complete login-to-inventory workflow")]
    // [AllureSeverity(SeverityLevel.critical)]
    // [AllureOwner("QA Team")]
    // [AllureTag("Smoke", "E2E")]
    // public async Task UserLoginWorkflow()
    // {
    //     var loginPage = new LoginPage(_page);
    //
    //     await AllureApi.Step("Navigate to login page", async () =>
    //     {
    //         await loginPage.NavigateToAsync("https://www.saucedemo.com/");
    //     });
    //
    //     await AllureApi.Step("Login with valid credentials", async () =>
    //     {
    //         await loginPage.LoginAsync("standard_user", "secret_sauce");
    //     });
    //
    //     await AllureApi.Step("Verify inventory page is displayed", async () =>
    //     {
    //         await Assertions.Expect(_page).ToHaveURLAsync("https://www.saucedemo.com/inventory.html");
    //         var inventoryVisible = await _page.IsVisibleAsync(".inventory_list");
    //         Assert.True(inventoryVisible);
    //     });
    // }
}
```

---

## Conformance Validation Rules

Conformance tests validate that generated code aligns with specifications. These meta-tests run in CI on every commit.

### Structural Validation

1. **Class Exists**
   - For each Page Object spec, verify `{PageName}Page.cs` exists in `src/Utilities/PageObjects/`
   - For each test spec, verify `{PageName}Page{TestType}Tests.cs` exists in `src/Tests/`

2. **Naming Convention**
   - Class name matches spec-defined name exactly
   - Namespace follows convention: `csharp_framework_demo.Utilities.PageObjects` or `csharp_framework_demo.Tests`
   - File name matches class name

3. **Method Signatures**
   - Public methods defined in spec exist in code
   - Method names match (case-sensitive)
   - Parameter types and names match
   - Return types match (`Task`, `Task<T>`, etc.)
   - All Playwright methods are async

4. **Attributes**
   - Required Allure attributes present: `[AllureSuite]`, `[AllureFeature]`
   - Attribute values match spec-defined values
   - Test methods have `[Fact]` attribute
   - Test methods have descriptive Allure attributes

5. **Dependencies**
   - Page Objects have constructor accepting `IPage` parameter
   - Test classes using browser have `IClassFixture<PlaywrightFixture>`
   - Correct using statements for Playwright, xUnit, Allure, Moq

### Behavioral Validation

1. **Test Coverage**
   - Each spec scenario has a corresponding test method
   - Test method name reflects scenario name
   - No spec scenarios are missing tests
   - No orphaned tests (tests without spec scenarios)

2. **Allure Steps (E2E Only)**
   - E2E test actions wrapped in `AllureApi.Step("description", async () => { ... })`
   - Step descriptions match spec step descriptions
   - Nested steps used appropriately

3. **Assertions**
   - Tests contain assertions matching spec expectations
   - Correct assertion type used (xUnit `Assert` vs. Playwright `Assertions.Expect`)
   - Assertion messages are clear and descriptive

4. **Test Attributes**
   - All tests have `[Fact]` (not Theory unless spec specifies parameterization)
   - `[AllureDescription]` matches spec scenario description
   - `[AllureSeverity]` appropriate (critical for smoke tests, normal for regression)
   - `[AllureOwner]` defined (default: "QA Team")
   - `[AllureTag]` includes relevant tags from spec

### Pattern Validation

1. **Async/Await**
   - All Playwright interactions use `await`
   - All methods calling Playwright return `Task` or `Task<T>`
   - No blocking calls (`.Result`, `.Wait()`)

2. **Page Object Pattern**
   - Unit/Integration tests use Page Objects for page interactions
   - E2E tests may use Page Objects or direct selectors (allowed for flexibility)
   - No business logic in Page Objects (only interactions)
   - Assertions in test classes, not Page Objects

3. **Fixture Usage**
   - Integration/E2E tests inherit `IClassFixture<PlaywrightFixture>`
   - Constructor accepts `PlaywrightFixture fixture`
   - `IPage _page` stored and reused across tests in class

4. **Mocking Pattern (Unit Tests)**
   - Uses `Moq` library: `var mockPage = new Mock<IPage>();`
   - Page Object instantiated with `mockPage.Object`
   - Verification uses `mockPage.Verify(p => p.MethodAsync(...), Times.Once)`

---

## Drift Detection Strategy

Drift occurs when specifications and code diverge. This framework uses multiple mechanisms to detect and prevent drift.

### Detection Mechanisms

1. **Conformance Tests (Primary)**
   - `SpecConformanceTests.cs` runs in CI on every commit
   - Validates spec files exist and are well-formed
   - Validates code structure matches spec contracts (when features exist)
   - Fails build if drift detected

2. **Manual Review (Secondary)**
   - PR checklist includes "Code matches specs" item
   - Code reviewers verify spec-code alignment
   - Changes to code require spec updates (and vice versa)

3. **Automated Alerts (Future)**
   - Conformance test failures block merge
   - GitHub Actions comments on PR with drift details
   - Slack/email notifications for conformance failures

### Drift Scenarios

| Scenario | Detection | Resolution |
|----------|-----------|------------|
| **Code changed without spec update** | Conformance test fails (method signature mismatch, missing scenario) | Update spec to reflect code changes |
| **Spec changed without code update** | Conformance test fails (missing method, scenario not implemented) | Regenerate or manually update code |
| **Manual code refinement (approved)** | Conformance test may fail if refinement differs from spec | Update spec to document approved refinement |
| **New spec scenario added** | Conformance test fails (scenario not implemented) | Generate/write code for new scenario |
| **Spec deleted** | Manual review (orphaned code detected) | Remove corresponding code |

### Sync Process

#### Initial Generation (Spec → Code)
1. **Write spec** - Create detailed specification in `specs/features/{feature}/`
2. **AI generates code** - Use Claude Code: "Generate code from spec at specs/features/{feature}/{spec-type}.md"
3. **Review** - Human reviews generated code for correctness, security, best practices
4. **Refine** - Make manual adjustments if AI-generated code needs improvement
5. **Update spec if needed** - If refinements differ from spec, update spec to match approved code
6. **Run conformance tests** - Verify spec-code alignment
7. **Commit both** - Commit spec and code together in same PR

#### Maintenance (Bidirectional Sync)

**When Code Changes:**
1. Developer modifies code (e.g., adds parameter to method)
2. Run conformance tests locally → Test fails (spec mismatch)
3. Update spec to reflect code change
4. Run conformance tests again → Test passes
5. Commit both code and spec changes

**When Spec Changes:**
1. Update spec (e.g., add new scenario)
2. Regenerate code from spec OR manually implement
3. Review and refine generated code
4. Run conformance tests → Verify alignment
5. Commit both spec and code changes

**Key Principle**: Never commit code without corresponding spec, or spec without corresponding code.

---

## Approval Workflow

### For New Features (Spec-First Development)

```
┌─────────────────────────────────────────────────────────────────┐
│ 1. Write Spec                                                   │
│    Create specs/features/{feature}/{spec-type}.md               │
│    Define behavioral contracts, scenarios, expectations         │
└───────────────────────────┬─────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────────┐
│ 2. AI Code Generation                                           │
│    Ask Claude Code: "Generate code from spec at ..."            │
│    AI reads spec and generates C# classes/tests                 │
└───────────────────────────┬─────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────────┐
│ 3. Human Review                                                 │
│    Review for: correctness, security, performance, readability  │
│    Check: proper error handling, edge cases, best practices     │
└───────────────────────────┬─────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────────┐
│ 4. Refine (If Needed)                                           │
│    Make manual improvements to generated code                   │
│    Add error handling, logging, optimizations                   │
│    Fix security issues, improve naming, add comments            │
└───────────────────────────┬─────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────────┐
│ 5. Update Spec to Match Approved Code                           │
│    If refinements differ from spec, update spec                 │
│    Ensure spec accurately documents final approved behavior     │
└───────────────────────────┬─────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────────┐
│ 6. Run Conformance Tests                                        │
│    dotnet test --filter "FullyQualifiedName~SpecConformanceTests"│
│    Verify all conformance tests pass                            │
└───────────────────────────┬─────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────────┐
│ 7. Approve and Merge                                            │
│    Create PR with both spec and code                            │
│    PR checklist: specs updated, conformance tests pass          │
│    Merge when approved and CI passes                            │
└─────────────────────────────────────────────────────────────────┘
```

### For Existing Features (Code-First, Retrospective Spec)

When adding specs to existing code:

1. **Analyze code** - Review implementation, understand behavior
2. **Write spec** - Document current behavior in spec format
3. **Validate** - Ensure spec accurately describes code
4. **Run conformance tests** - Verify spec-code alignment
5. **Commit spec** - Add spec to repository
6. **Future changes** - Use bidirectional sync process

---

## CI/CD Integration

### Build Pipeline

**Current CI workflow** (`.github/workflows/ci.yml`):
1. Checkout code
2. Setup .NET 8.0
3. Restore dependencies
4. Build project
5. Run tests in Docker
6. Generate Allure report
7. Deploy report to GitHub Pages

**SDD Enhancement**:

Add step after main tests:

```yaml
- name: Run Conformance Tests
  if: always()
  run: |
    dotnet test src/csharp_framework_demo.csproj \
      --filter "FullyQualifiedName~SpecConformanceTests" \
      --logger "console;verbosity=detailed"
```

### Quality Gates

**Existing**:
- Build must succeed
- All unit/integration/E2E tests must pass
- Docker build must succeed
- Allure report must generate

**SDD Addition**:
- **All conformance tests must pass** ← New requirement
- Spec-code alignment validated
- No merge without synchronized specs and code

### PR Checklist

Template (`.github/pull_request_template.md`):

```markdown
## Description
<!-- Describe changes -->

## Checklist
- [ ] Code changes are reflected in specs (or vice versa)
- [ ] Conformance tests pass locally
- [ ] Allure attributes updated
- [ ] Tests pass in Docker
- [ ] Documentation updated if needed

## SDD Compliance
- [ ] New specs created for new features (if applicable)
- [ ] Generated code reviewed and approved
- [ ] No spec-code drift detected
- [ ] Both spec and code committed together
```

---

## Summary

This meta-framework establishes Spec-Driven Development for the C# Playwright test automation framework:

✅ **Hybrid approach** - AI generates, humans approve, both artifacts maintained
✅ **Clear mapping rules** - Specs map deterministically to code
✅ **Conformance testing** - Automated validation prevents drift
✅ **Bidirectional sync** - Changes to either spec or code keep both aligned
✅ **CI integration** - Quality gates enforce spec-code synchronization
✅ **Practical workflow** - Balances automation with human oversight

**Next Steps**:
1. Create feature specs using templates (e.g., authentication)
2. Generate code from specs using AI
3. Expand conformance tests to validate feature implementations
4. Iterate and refine the SDD process based on experience
