# {Feature} - End-to-End Workflow Specification

<!--
  INSTRUCTIONS:
  - Replace {Feature} with the feature name (e.g., "Authentication", "Shopping Cart", "Checkout")
  - Define complete user workflows that span multiple pages
  - Use Given/When/Then format with multiple steps
  - Each step should be wrapped in AllureApi.Step()
  - Include positive and negative scenarios
  - This spec maps to: src/Tests/{Feature}WorkflowTests.cs
-->

## Test Suite Overview

**Test Suite**: End-to-End Tests

**Feature**: {Feature Name}

**Purpose**: Validate complete user workflows for {feature description}

**Scope**: Multi-page user journeys from start to finish

**Browser**: Real Chromium browser (headless mode in CI)

**Fixture**: PlaywrightFixture (provides IPage instance)

**Test Framework**: xUnit with Allure attributes and step reporting

---

## Test Configuration

**AllureSuite**: `"End-to-End Tests"`

**AllureFeature**: `"{Feature}"`

**Fixture**: `IClassFixture<PlaywrightFixture>`

**Setup**:
```csharp
private readonly IPage _page;

public {Feature}WorkflowTests(PlaywrightFixture fixture)
{
    _page = fixture.Page;
}
```

**Dependencies**:
- Playwright (real browser)
- xUnit
- Allure.Net.Commons (for AllureApi.Step)
- Allure.Xunit
- Multiple Page Object classes

---

## Workflow Scenarios

### Workflow 1: {MainSuccessScenario}

**Test Method Name**: `{DescriptiveWorkflowName}`

**Business Goal**: {What business value this workflow provides}

**User Story**: As a {user type}, I want to {action}, so that {benefit}

**Allure Attributes**:
```csharp
[AllureDescription("Verifies that {user} can {complete workflow} successfully")]
[AllureSeverity(SeverityLevel.critical)]
[AllureOwner("QA Team")]
[AllureTag("Smoke", "E2E", "{Feature}")]
```

**Workflow Steps**:

#### Step 1: {InitialAction}
**Given**: {Initial condition}
**Action**: {What user does}
**Expected**: {What should happen}

#### Step 2: {NextAction}
**Given**: {Condition from Step 1}
**Action**: {What user does next}
**Expected**: {What should happen}

#### Step 3: {VerificationAction}
**Given**: {Condition from Step 2}
**Action**: {Final verification}
**Expected**: {Final expected state}

**Code Structure**:
```csharp
[Fact]
[AllureDescription("Verifies complete {feature} workflow")]
[AllureSeverity(SeverityLevel.critical)]
[AllureOwner("QA Team")]
[AllureTag("Smoke", "E2E")]
public async Task {WorkflowName}()
{
    var {page1} = new {Page1}Page(_page);
    var {page2} = new {Page2}Page(_page);

    await AllureApi.Step("Step 1: {Action description}", async () =>
    {
        await {page1}.{Method}Async({params});
        // Assertions for this step
    });

    await AllureApi.Step("Step 2: {Action description}", async () =>
    {
        await {page2}.{Method}Async({params});
        // Assertions for this step
    });

    await AllureApi.Step("Step 3: {Verification description}", async () =>
    {
        // Final assertions
    });
}
```

**Example (Authentication Workflow)**:
```csharp
[Fact]
[AllureDescription("Verifies that user can log in and see inventory page")]
[AllureSeverity(SeverityLevel.critical)]
[AllureOwner("QA Team")]
[AllureTag("Smoke", "Authentication", "E2E")]
public async Task SuccessfulLoginWorkflow()
{
    var loginPage = new LoginPage(_page);

    await AllureApi.Step("Navigate to login page", async () =>
    {
        await loginPage.NavigateToAsync("https://www.saucedemo.com/");
    });

    await AllureApi.Step("Enter valid credentials and login", async () =>
    {
        await loginPage.LoginAsync("standard_user", "secret_sauce");
    });

    await AllureApi.Step("Verify inventory list is visible", async () =>
    {
        var inventoryVisible = await _page.IsVisibleAsync(".inventory_list");
        Assert.True(inventoryVisible);
    });

    await AllureApi.Step("Verify URL redirects to inventory page", async () =>
    {
        await Assertions.Expect(_page).ToHaveURLAsync("https://www.saucedemo.com/inventory.html");
    });
}
```

---

### Workflow 2: {AlternativeSuccessScenario}

**Test Method Name**: `{AlternativeWorkflowName}`

**Business Goal**: {Alternative path business value}

**User Story**: As a {user type}, I want to {alternative action}, so that {benefit}

**Allure Attributes**:
```csharp
[AllureDescription("Verifies that {user} can {alternative workflow} successfully")]
[AllureSeverity(SeverityLevel.critical | normal)]
[AllureOwner("QA Team")]
[AllureTag("Regression", "E2E", "{Feature}")]
```

**Workflow Steps**:

#### Step 1: {Step description}
**Given**: {Condition}
**Action**: {Action}
**Expected**: {Expected result}

#### Step 2: {Step description}
**Given**: {Condition}
**Action**: {Action}
**Expected**: {Expected result}

#### Step 3: {Step description}
**Given**: {Condition}
**Action**: {Action}
**Expected**: {Expected result}

**Code Structure**:
```csharp
[Fact]
[AllureDescription("{Description}")]
[AllureSeverity(SeverityLevel.{level})]
[AllureOwner("QA Team")]
[AllureTag("{Tags}")]
public async Task {AlternativeWorkflowName}()
{
    await AllureApi.Step("Step 1: {Description}", async () =>
    {
        // Step 1 actions and assertions
    });

    await AllureApi.Step("Step 2: {Description}", async () =>
    {
        // Step 2 actions and assertions
    });

    await AllureApi.Step("Step 3: {Description}", async () =>
    {
        // Step 3 actions and assertions
    });
}
```

**Example (Login and Navigate)**:
```csharp
[Fact]
[AllureDescription("Verifies user can login and navigate to product details")]
[AllureSeverity(SeverityLevel.normal)]
[AllureOwner("QA Team")]
[AllureTag("Regression", "Navigation")]
public async Task LoginAndNavigateToProductDetails()
{
    var loginPage = new LoginPage(_page);

    await AllureApi.Step("Navigate to login page", async () =>
    {
        await loginPage.NavigateToAsync("https://www.saucedemo.com/");
    });

    await AllureApi.Step("Login with valid credentials", async () =>
    {
        await loginPage.LoginAsync("standard_user", "secret_sauce");
    });

    await AllureApi.Step("Verify inventory page is displayed", async () =>
    {
        await Assertions.Expect(_page).ToHaveURLAsync("https://www.saucedemo.com/inventory.html");
    });

    await AllureApi.Step("Click on first product", async () =>
    {
        await _page.ClickAsync(".inventory_item:first-child .inventory_item_name");
    });

    await AllureApi.Step("Verify product details page is displayed", async () =>
    {
        var productDetailsVisible = await _page.IsVisibleAsync(".inventory_details");
        Assert.True(productDetailsVisible);
    });
}
```

---

### Workflow 3: {ErrorScenario}

**Test Method Name**: `{ErrorWorkflowName}`

**Business Goal**: Validate error handling for {error condition}

**User Story**: As a {user type}, when I {invalid action}, I should see {error feedback}

**Allure Attributes**:
```csharp
[AllureDescription("Verifies that {error scenario} displays appropriate error")]
[AllureSeverity(SeverityLevel.critical | normal)]
[AllureOwner("QA Team")]
[AllureTag("Negative", "Validation", "E2E")]
```

**Workflow Steps**:

#### Step 1: {Initial action}
**Given**: {Initial state}
**Action**: {User action}
**Expected**: {Expected state}

#### Step 2: {Error-triggering action}
**Given**: {Condition}
**Action**: {Invalid action performed}
**Expected**: {Error should be shown}

#### Step 3: {Error verification}
**Given**: {Error state}
**Action**: {Verify error details}
**Expected**: {Correct error message, user remains on page, etc.}

**Code Structure**:
```csharp
[Fact]
[AllureDescription("Verifies error handling for {scenario}")]
[AllureSeverity(SeverityLevel.critical)]
[AllureOwner("QA Team")]
[AllureTag("Negative", "Validation")]
public async Task {ErrorWorkflowName}()
{
    await AllureApi.Step("Step 1: {Description}", async () =>
    {
        // Navigate to starting point
    });

    await AllureApi.Step("Step 2: {Trigger error}", async () =>
    {
        // Perform invalid action
    });

    await AllureApi.Step("Step 3: {Verify error}", async () =>
    {
        // Assert error is displayed correctly
    });
}
```

**Example (Failed Login)**:
```csharp
[Fact]
[AllureDescription("Verifies that login fails with invalid credentials and displays error")]
[AllureSeverity(SeverityLevel.critical)]
[AllureOwner("QA Team")]
[AllureTag("Smoke", "Negative", "Validation")]
public async Task FailedLogin_InvalidCredentials()
{
    var loginPage = new LoginPage(_page);

    await AllureApi.Step("Navigate to login page", async () =>
    {
        await loginPage.NavigateToAsync("https://www.saucedemo.com/");
    });

    await AllureApi.Step("Attempt login with invalid credentials", async () =>
    {
        await loginPage.LoginAsync("invalid_user", "wrong_password");
    });

    await AllureApi.Step("Verify error message is displayed", async () =>
    {
        var errorMessage = await _page.TextContentAsync("[data-test='error']");
        Assert.Equal("Epic sadface: Username and password do not match any user in this service", errorMessage);
    });

    await AllureApi.Step("Verify user remains on login page", async () =>
    {
        await Assertions.Expect(_page).ToHaveURLAsync("https://www.saucedemo.com/");
    });
}
```

---

### Workflow 4: {EdgeCaseScenario}

**Test Method Name**: `{EdgeCaseWorkflowName}`

**Business Goal**: {What edge case this validates}

**Allure Attributes**:
```csharp
[AllureDescription("Verifies {edge case scenario}")]
[AllureSeverity(SeverityLevel.normal | minor)]
[AllureOwner("QA Team")]
[AllureTag("Regression", "Edge Case")]
```

**Workflow Steps**:

#### Step 1: {Setup edge condition}
**Given**: {Initial state}
**Action**: {Setup action}
**Expected**: {Edge condition established}

#### Step 2: {Test edge case}
**Given**: {Edge condition}
**Action**: {Perform action}
**Expected**: {System handles edge case correctly}

#### Step 3: {Verify result}
**Given**: {Result state}
**Action**: {Verification}
**Expected**: {Expected behavior}

**Code Structure**:
```csharp
[Fact]
[AllureDescription("{Edge case description}")]
[AllureSeverity(SeverityLevel.normal)]
[AllureOwner("QA Team")]
[AllureTag("Regression", "Edge Case")]
public async Task {EdgeCaseWorkflowName}()
{
    await AllureApi.Step("Step 1: {Setup}", async () =>
    {
        // Setup edge condition
    });

    await AllureApi.Step("Step 2: {Test edge case}", async () =>
    {
        // Perform action
    });

    await AllureApi.Step("Step 3: {Verify}", async () =>
    {
        // Assert correct behavior
    });
}
```

**Example (Empty Credentials)**:
```csharp
[Fact]
[AllureDescription("Verifies that login fails when no credentials are provided")]
[AllureSeverity(SeverityLevel.normal)]
[AllureOwner("QA Team")]
[AllureTag("Regression", "Validation", "Negative")]
public async Task FailedLogin_EmptyCredentials()
{
    var loginPage = new LoginPage(_page);

    await AllureApi.Step("Navigate to login page", async () =>
    {
        await loginPage.NavigateToAsync("https://www.saucedemo.com/");
    });

    await AllureApi.Step("Click login button without entering credentials", async () =>
    {
        await _page.ClickAsync("[data-test='login-button']");
    });

    await AllureApi.Step("Verify username required error is displayed", async () =>
    {
        var errorMessage = await _page.TextContentAsync("[data-test='error']");
        Assert.Equal("Epic sadface: Username is required", errorMessage);
    });
}
```

---

## Additional Workflows

<!-- Define additional E2E workflows as needed -->

### Workflow: {AdditionalScenario}

**Test Method Name**: `{TestMethodName}`

**Business Goal**: {Goal}

**User Story**: As a {role}, I want to {action}, so that {benefit}

**Workflow Steps**:
1. {Step 1}
2. {Step 2}
3. {Step 3}

**Code Example**:
```csharp
[Fact]
[AllureDescription("{Description}")]
[AllureSeverity(SeverityLevel.{level})]
[AllureOwner("QA Team")]
[AllureTag("{Tags}")]
public async Task {TestMethodName}()
{
    await AllureApi.Step("Step 1: {Description}", async () => { });
    await AllureApi.Step("Step 2: {Description}", async () => { });
    await AllureApi.Step("Step 3: {Description}", async () => { });
}
```

---

## Allure Step Guidelines

### Basic Step Pattern

```csharp
await AllureApi.Step("Step description", async () =>
{
    // Actions
    // Assertions
});
```

### Nested Steps (Optional)

```csharp
await AllureApi.Step("Main step", async () =>
{
    await AllureApi.Step("Sub-step 1", async () =>
    {
        // Sub-step actions
    });

    await AllureApi.Step("Sub-step 2", async () =>
    {
        // Sub-step actions
    });
});
```

### Step Naming Best Practices

✅ **Good - Descriptive**:
- "Navigate to login page"
- "Enter valid credentials and submit"
- "Verify user is redirected to dashboard"

❌ **Bad - Vague**:
- "Do login"
- "Check stuff"
- "Verify"

---

## Test Data

### Application URLs

```csharp
private const string BaseUrl = "https://www.saucedemo.com/";
private const string LoginUrl = "https://www.saucedemo.com/";
private const string InventoryUrl = "https://www.saucedemo.com/inventory.html";
```

### Test Users

```csharp
// Valid user
private const string StandardUser = "standard_user";
private const string ValidPassword = "secret_sauce";

// Locked user
private const string LockedUser = "locked_out_user";

// Invalid user
private const string InvalidUser = "invalid_user";
private const string InvalidPassword = "wrong_password";
```

**Future Enhancement**: Externalize to appsettings.json

---

## Workflow Coverage Checklist

### Positive Scenarios
- [ ] {Main happy path workflow}
- [ ] {Alternative success path 1}
- [ ] {Alternative success path 2}
- [ ] {Complete end-to-end flow (all features)}

### Negative Scenarios
- [ ] {Error scenario 1 - e.g., "Invalid input"}
- [ ] {Error scenario 2 - e.g., "Unauthorized access"}
- [ ] {Error scenario 3 - e.g., "Missing required fields"}

### Edge Cases
- [ ] {Edge case 1 - e.g., "Empty inputs"}
- [ ] {Edge case 2 - e.g., "Special characters"}
- [ ] {Edge case 3 - e.g., "Boundary values"}

### User Journeys
- [ ] {Journey 1 - e.g., "New user first-time experience"}
- [ ] {Journey 2 - e.g., "Returning user workflow"}
- [ ] {Journey 3 - e.g., "User with saved preferences"}

---

## Performance Considerations

**Timeout Configuration**:
- Default Playwright timeout: 30 seconds
- Default xUnit test timeout: 60 seconds (can be increased if needed)

**Test Execution Time**:
- E2E tests are slower than unit/integration tests
- Target: < 30 seconds per workflow
- Optimize by:
  - Minimizing unnecessary waits
  - Using stable selectors
  - Avoiding flaky assertions

**Flakiness Prevention**:
- Use Playwright's built-in waiting (auto-wait for elements)
- Avoid hardcoded `Task.Delay()` - use `page.WaitForSelectorAsync()` instead
- Use stable selectors (data-test attributes preferred)
- Ensure test isolation (each test independent)

---

## Debugging Tips

**Run Tests Headed** (locally):
```csharp
Browser = await Playwright.Chromium.LaunchAsync(new() { Headless = false });
```

**Slow Down Execution** (for debugging):
```csharp
Browser = await Playwright.Chromium.LaunchAsync(new() { Headless = false, SlowMo = 500 });
```

**Capture Screenshot on Failure** (future enhancement):
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

**Playwright Inspector** (pause execution):
```csharp
await _page.PauseAsync(); // Opens Playwright Inspector
```

---

## Mapping to Code

**Generated Class**: `src/Tests/{Feature}WorkflowTests.cs`

**Namespace**: `csharp_framework_demo.Tests`

**Class Declaration**:
```csharp
[AllureSuite("End-to-End Tests")]
[AllureFeature("{Feature}")]
public class {Feature}WorkflowTests : IClassFixture<PlaywrightFixture>
{
    private readonly IPage _page;

    public {Feature}WorkflowTests(PlaywrightFixture fixture)
    {
        _page = fixture.Page;
    }

    // Workflow test methods...
}
```

**Dependencies**:
- `using Microsoft.Playwright;`
- `using Xunit;`
- `using Allure.Net.Commons;`
- `using Allure.Xunit.Attributes;`
- `using csharp_framework_demo.Utilities.PageObjects;`

**Compliance**: Must follow PROJECT-SPEC.md Test Standards

---

## Notes

- E2E tests represent real user workflows, not isolated actions
- Each workflow should tell a complete user story
- Use AllureApi.Step() for granular reporting (critical for debugging failures)
- Tests should be self-contained and independent
- Consider data cleanup if tests modify application state
- Future enhancement: Add screenshots/videos on failure for better diagnostics
