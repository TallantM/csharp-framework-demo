# {PageName} Page Object - Integration Test Specification

<!--
  INSTRUCTIONS:
  - Replace {PageName} with the actual page name (e.g., "Login", "Inventory")
  - Replace {Feature} with the feature name for Allure reporting
  - Define test scenarios that validate Page Object with real browser
  - Use Given/When/Then format for clarity
  - Specify expected browser state after actions
  - This spec maps to: src/Tests/{PageName}PageIntegrationTests.cs
-->

## Test Suite Overview

**Test Suite**: Integration Tests

**Feature**: {Feature Name}

**Target**: {PageName}Page class with real Playwright browser

**Purpose**: Validate {PageName}Page methods interact correctly with actual web pages

**Browser**: Real Chromium browser (headless mode in CI)

**Fixture**: PlaywrightFixture (provides IPage instance)

**Test Framework**: xUnit with Allure attributes

---

## Test Configuration

**AllureSuite**: `"Integration Tests"`

**AllureFeature**: `"{Feature}"`

**Fixture**: `IClassFixture<PlaywrightFixture>`

**Setup**:
```csharp
private readonly IPage _page;

public {PageName}PageIntegrationTests(PlaywrightFixture fixture)
{
    _page = fixture.Page;
}
```

**Dependencies**:
- Playwright (real browser)
- xUnit
- Allure.Xunit
- {PageName}Page

---

## Test Scenarios

### Test 1: {Method}_With{ValidInput}_Succeeds

**Test Method Name**: `{Method}_With{ValidInput}_{ExpectedBehavior}`

**Allure Attributes**:
```csharp
[AllureDescription("Verifies that {method} with valid {input} successfully {action} and {expected state}")]
[AllureSeverity(SeverityLevel.critical | normal)]
[AllureOwner("QA Team")]
[AllureTag("Integration", "Positive")]
```

**Given**:
- Browser is initialized via PlaywrightFixture
- {PageName}Page is instantiated with real IPage
- User navigates to {page URL}
- {Additional preconditions - e.g., "User is logged out"}

**When**:
- `{Method}Async({valid params})` is called

**Then**:
- {Expected browser state 1 - e.g., "Page URL is correct"}
- {Expected element state 2 - e.g., "Element is visible"}
- {Expected data state 3 - e.g., "Text content matches expected"}
- No exceptions are thrown

**Code Structure**:
```csharp
[Fact]
[AllureDescription("Verifies that {method} with valid input succeeds")]
[AllureSeverity(SeverityLevel.critical)]
[AllureOwner("QA Team")]
[AllureTag("Integration", "Positive")]
public async Task {Method}_WithValidInput_Succeeds()
{
    // Arrange
    var {pageName}Page = new {PageName}Page(_page);
    await {pageName}Page.NavigateToAsync("{pageUrl}");

    // Act
    await {pageName}Page.{Method}Async({validParams});

    // Assert
    await Assertions.Expect(_page).ToHaveURLAsync("{expectedUrl}");
    {Additional Playwright assertions}
}
```

**Example (Successful Login)**:
```csharp
[Fact]
[AllureDescription("Verifies that LoginAsync with valid credentials successfully logs in and redirects to inventory")]
[AllureSeverity(SeverityLevel.critical)]
[AllureOwner("QA Team")]
[AllureTag("Integration", "Login", "Positive")]
public async Task LoginAsync_WithValidCredentials_RedirectsToInventory()
{
    // Arrange
    var loginPage = new LoginPage(_page);
    await loginPage.NavigateToAsync("https://www.saucedemo.com/");

    // Act
    await loginPage.LoginAsync("standard_user", "secret_sauce");

    // Assert
    await Assertions.Expect(_page).ToHaveURLAsync("https://www.saucedemo.com/inventory.html");
}
```

---

### Test 2: {Method}_With{InvalidInput}_ShowsError

**Test Method Name**: `{Method}_With{InvalidInput}_Shows{ErrorType}`

**Allure Attributes**:
```csharp
[AllureDescription("Verifies that {method} with {invalid input} displays appropriate error message")]
[AllureSeverity(SeverityLevel.critical | normal)]
[AllureOwner("QA Team")]
[AllureTag("Integration", "Negative", "Validation")]
```

**Given**:
- Browser is initialized
- {PageName}Page is instantiated
- User navigates to {page URL}
- {Additional preconditions}

**When**:
- `{Method}Async({invalid params})` is called

**Then**:
- {Error indicator is visible - e.g., "Error message is displayed"}
- {Error message text matches expected}
- {User remains on current page (no redirect)}
- {Expected error state}

**Code Structure**:
```csharp
[Fact]
[AllureDescription("Verifies that {method} with invalid input shows error")]
[AllureSeverity(SeverityLevel.critical)]
[AllureOwner("QA Team")]
[AllureTag("Integration", "Negative")]
public async Task {Method}_WithInvalidInput_ShowsError()
{
    // Arrange
    var {pageName}Page = new {PageName}Page(_page);
    await {pageName}Page.NavigateToAsync("{pageUrl}");

    // Act
    await {pageName}Page.{Method}Async({invalidParams});

    // Assert
    var errorVisible = await _page.IsVisibleAsync("{errorSelector}");
    Assert.True(errorVisible);

    var errorText = await _page.TextContentAsync("{errorSelector}");
    Assert.Equal("{expectedErrorMessage}", errorText);
}
```

**Example (Invalid Login)**:
```csharp
[Fact]
[AllureDescription("Verifies that LoginAsync with invalid credentials displays error message")]
[AllureSeverity(SeverityLevel.critical)]
[AllureOwner("QA Team")]
[AllureTag("Integration", "Login", "Negative")]
public async Task LoginAsync_WithInvalidCredentials_ShowsErrorMessage()
{
    // Arrange
    var loginPage = new LoginPage(_page);
    await loginPage.NavigateToAsync("https://www.saucedemo.com/");

    // Act
    await loginPage.LoginAsync("invalid_user", "wrong_password");

    // Assert
    var errorVisible = await _page.IsVisibleAsync("[data-test='error']");
    Assert.True(errorVisible);

    var errorMessage = await _page.TextContentAsync("[data-test='error']");
    Assert.Equal("Epic sadface: Username and password do not match any user in this service", errorMessage);
}
```

---

### Test 3: {QueryMethod}_Returns{ExpectedState}

**Test Method Name**: `{QueryMethod}_When{Condition}_Returns{ExpectedValue}`

**Allure Attributes**:
```csharp
[AllureDescription("Verifies that {queryMethod} returns {expected value} when {condition}")]
[AllureSeverity(SeverityLevel.normal)]
[AllureOwner("QA Team")]
[AllureTag("Integration", "Query")]
```

**Given**:
- Browser is initialized
- {PageName}Page is instantiated
- User navigates to {page URL}
- {Specific condition is established}

**When**:
- `{QueryMethod}Async()` is called

**Then**:
- Returned value equals {expected value}
- No exceptions are thrown

**Code Structure**:
```csharp
[Fact]
[AllureDescription("Verifies that {queryMethod} returns expected value")]
[AllureSeverity(SeverityLevel.normal)]
[AllureOwner("QA Team")]
[AllureTag("Integration", "Query")]
public async Task {QueryMethod}_WhenCondition_ReturnsExpectedValue()
{
    // Arrange
    var {pageName}Page = new {PageName}Page(_page);
    await {pageName}Page.NavigateToAsync("{pageUrl}");
    {// Establish condition}

    // Act
    var result = await {pageName}Page.{QueryMethod}Async();

    // Assert
    Assert.Equal({expectedValue}, result);
}
```

**Example (Error Message Visibility)**:
```csharp
[Fact]
[AllureDescription("Verifies that IsErrorMessageVisibleAsync returns true after failed login")]
[AllureSeverity(SeverityLevel.normal)]
[AllureOwner("QA Team")]
[AllureTag("Integration", "Query")]
public async Task IsErrorMessageVisibleAsync_AfterFailedLogin_ReturnsTrue()
{
    // Arrange
    var loginPage = new LoginPage(_page);
    await loginPage.NavigateToAsync("https://www.saucedemo.com/");
    await loginPage.LoginAsync("invalid", "wrong");

    // Act
    var isVisible = await loginPage.IsErrorMessageVisibleAsync();

    // Assert
    Assert.True(isVisible);
}
```

---

### Test 4: NavigateToAsync_LoadsPageCorrectly

**Test Method Name**: `NavigateToAsync_ToValidUrl_LoadsPageSuccessfully`

**Allure Attributes**:
```csharp
[AllureDescription("Verifies that NavigateToAsync successfully loads the {page} page")]
[AllureSeverity(SeverityLevel.critical)]
[AllureOwner("QA Team")]
[AllureTag("Integration", "Navigation")]
```

**Given**:
- Browser is initialized
- {PageName}Page is instantiated

**When**:
- `NavigateToAsync("{pageUrl}")` is called

**Then**:
- Page URL matches expected URL
- Page title contains expected text (if applicable)
- Key page element is visible

**Code Structure**:
```csharp
[Fact]
[AllureDescription("Verifies that NavigateToAsync loads page successfully")]
[AllureSeverity(SeverityLevel.critical)]
[AllureOwner("QA Team")]
[AllureTag("Integration", "Navigation")]
public async Task NavigateToAsync_LoadsPageSuccessfully()
{
    // Arrange
    var {pageName}Page = new {PageName}Page(_page);

    // Act
    await {pageName}Page.NavigateToAsync("{pageUrl}");

    // Assert
    await Assertions.Expect(_page).ToHaveURLAsync("{pageUrl}");
    var {keyElement}Visible = await _page.IsVisibleAsync("{keyElementSelector}");
    Assert.True({keyElement}Visible);
}
```

---

## Additional Test Scenarios

<!-- Add more integration test scenarios as needed -->

### Test: {ScenarioName}

**Given**: {Precondition}
**When**: {Action}
**Then**: {Expected browser state}

**Code Example**:
```csharp
[Fact]
[AllureDescription("{Description}")]
[AllureSeverity(SeverityLevel.{level})]
[AllureOwner("QA Team")]
[AllureTag("Integration", "{AdditionalTags}")]
public async Task {TestMethodName}()
{
    // Arrange
    var {pageName}Page = new {PageName}Page(_page);
    // ...

    // Act
    // ...

    // Assert
    // ...
}
```

---

## Assertion Guidelines

### Playwright Assertions (Preferred for Browser State)

**URL Assertions**:
```csharp
await Assertions.Expect(_page).ToHaveURLAsync("{expectedUrl}");
```

**Element Visibility**:
```csharp
var element = _page.Locator("{selector}");
await Assertions.Expect(element).ToBeVisibleAsync();
```

**Element Text**:
```csharp
var element = _page.Locator("{selector}");
await Assertions.Expect(element).ToHaveTextAsync("{expectedText}");
```

**Element Attribute**:
```csharp
var element = _page.Locator("{selector}");
await Assertions.Expect(element).ToHaveAttributeAsync("{attribute}", "{value}");
```

---

### xUnit Assertions (For Simple Checks)

**Boolean Checks**:
```csharp
Assert.True(condition);
Assert.False(condition);
```

**Value Equality**:
```csharp
Assert.Equal(expected, actual);
Assert.NotEqual(unexpected, actual);
```

**Null Checks**:
```csharp
Assert.NotNull(value);
Assert.Null(value);
```

**String Contains**:
```csharp
Assert.Contains("substring", fullString);
```

---

## Test Data

### Valid Test Data

```csharp
// Valid credentials
const string ValidUsername = "standard_user";
const string ValidPassword = "secret_sauce";

// Valid URLs
const string LoginPageUrl = "https://www.saucedemo.com/";
const string InventoryPageUrl = "https://www.saucedemo.com/inventory.html";
```

### Invalid Test Data

```csharp
// Invalid credentials
const string InvalidUsername = "invalid_user";
const string InvalidPassword = "wrong_password";

// Locked user
const string LockedUsername = "locked_out_user";

// Empty credentials
const string EmptyUsername = "";
const string EmptyPassword = "";
```

**Note**: Future enhancement - externalize test data to appsettings.json

---

## Edge Cases to Test

- [ ] {Edge case 1 - e.g., "Empty input fields"}
- [ ] {Edge case 2 - e.g., "Special characters in input"}
- [ ] {Edge case 3 - e.g., "Very long input (> 255 characters)"}
- [ ] {Edge case 4 - e.g., "Unicode characters"}
- [ ] {Edge case 5 - e.g., "SQL injection attempts (security)"}
- [ ] {Edge case 6 - e.g., "XSS attempts (security)"}

---

## Coverage Goals

**Target**: All public methods of {PageName}Page with both positive and negative scenarios

**Methods to Test**:
- [ ] {Method 1} - Valid input
- [ ] {Method 1} - Invalid input
- [ ] {Method 2} - Valid input
- [ ] {Method 2} - Edge case
- [ ] {Additional methods...}

---

## Mapping to Code

**Generated Class**: `src/Tests/{PageName}PageIntegrationTests.cs`

**Namespace**: `csharp_framework_demo.Tests`

**Class Declaration**:
```csharp
[AllureSuite("Integration Tests")]
[AllureFeature("{Feature}")]
public class {PageName}PageIntegrationTests : IClassFixture<PlaywrightFixture>
{
    private readonly IPage _page;

    public {PageName}PageIntegrationTests(PlaywrightFixture fixture)
    {
        _page = fixture.Page;
    }

    // Test methods...
}
```

**Dependencies**:
- `using Microsoft.Playwright;`
- `using Xunit;`
- `using Allure.Xunit.Attributes;`
- `using csharp_framework_demo.Utilities.PageObjects;`

**Compliance**: Must follow PROJECT-SPEC.md Test Standards

---

## Performance Considerations

**Timeouts**:
- Default Playwright timeout: 30 seconds
- Can override for slow operations: `await _page.ClickAsync("{selector}", new() { Timeout = 60000 });`

**Test Isolation**:
- Each test starts with a fresh page state (via fixture)
- Tests should not depend on each other
- Clean up state if needed (e.g., logout) at end of test

**Parallel Execution** (Future):
- Tests in this class share PlaywrightFixture (sequential execution)
- Different test classes can run in parallel (when configured in xUnit)
