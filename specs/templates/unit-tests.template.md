# {PageName} Page Object - Unit Test Specification

<!--
  INSTRUCTIONS:
  - Replace {PageName} with the actual page name (e.g., "Login", "Inventory")
  - Replace {Feature} with the feature name for Allure reporting (e.g., "Login Page Object", "Inventory Management")
  - Define test scenarios for each Page Object method
  - Use Given/When/Then format for clarity
  - Specify mocking strategy (Mock<IPage>)
  - This spec maps to: src/Tests/{PageName}PageUnitTests.cs
-->

## Test Suite Overview

**Test Suite**: Unit Tests

**Feature**: {Feature Name}

**Target**: {PageName}Page class

**Purpose**: Validate {PageName}Page methods in isolation using mocked IPage interface

**Mocking Strategy**: Use Moq to mock `IPage` and verify correct Playwright API calls

**Test Framework**: xUnit with Allure attributes

---

## Test Configuration

**AllureSuite**: `"Unit Tests"`

**AllureFeature**: `"{Feature}"`

**Fixture**: None (unit tests do not use PlaywrightFixture)

**Dependencies**:
- Moq (for mocking IPage)
- xUnit
- Allure.Xunit

---

## Test Scenarios

<!-- Define each test scenario following Given/When/Then format -->

### Test 1: {MethodName}_CallsCorrectPlaywrightMethod

**Test Method Name**: `{MethodName}_Calls{PlaywrightMethod}`

**Allure Attributes**:
```csharp
[AllureDescription("Verifies that {MethodName} calls {PlaywrightMethod} on IPage with correct parameters")]
[AllureSeverity(SeverityLevel.normal)]
[AllureOwner("QA Team")]
[AllureTag("Unit", "Page Object")]
```

**Given**:
- Mock IPage is created
- {PageName}Page is instantiated with mocked IPage
- {Test data is prepared (e.g., URL, username, password)}

**When**:
- `{MethodName}Async({parameters})` is called on the Page Object

**Then**:
- Verify that `Mock<IPage>.{PlaywrightMethod}` was called exactly once
- Verify that correct parameters were passed to Playwright method
- No exceptions are thrown

**Code Structure**:
```csharp
[Fact]
[AllureDescription("Verifies that {MethodName} calls {PlaywrightMethod} on IPage")]
public async Task {MethodName}_Calls{PlaywrightMethod}()
{
    // Arrange
    var mockPage = new Mock<IPage>();
    var {pageName}Page = new {PageName}Page(mockPage.Object);
    {var testData = ...;}

    // Act
    await {pageName}Page.{MethodName}Async({parameters});

    // Assert
    mockPage.Verify(p => p.{PlaywrightMethod}({expectedParams}), Times.Once);
}
```

**Example (NavigateToAsync)**:
```csharp
[Fact]
[AllureDescription("Verifies that NavigateToAsync calls GotoAsync on IPage")]
public async Task NavigateToAsync_CallsGotoAsync()
{
    // Arrange
    var mockPage = new Mock<IPage>();
    var loginPage = new LoginPage(mockPage.Object);
    var url = "https://www.example.com/";

    // Act
    await loginPage.NavigateToAsync(url);

    // Assert
    mockPage.Verify(p => p.GotoAsync(url, null), Times.Once);
}
```

---

### Test 2: {MethodName}_With{Condition}_CallsExpectedMethods

**Test Method Name**: `{MethodName}_With{Condition}_Calls{Methods}`

**Allure Attributes**:
```csharp
[AllureDescription("Verifies that {MethodName} calls multiple Playwright methods in correct order when {condition}")]
[AllureSeverity(SeverityLevel.normal)]
[AllureOwner("QA Team")]
[AllureTag("Unit", "Page Object", "{Condition}")]
```

**Given**:
- Mock IPage is created
- {PageName}Page is instantiated
- {Specific condition or test data}

**When**:
- `{MethodName}Async({parameters})` is called

**Then**:
- Verify that {PlaywrightMethod1} was called with {params1}
- Verify that {PlaywrightMethod2} was called with {params2}
- Verify that {PlaywrightMethod3} was called with {params3}
- Verify correct call order (if order matters)

**Code Structure**:
```csharp
[Fact]
[AllureDescription("Verifies that {MethodName} calls multiple methods in sequence")]
public async Task {MethodName}_CallsMultipleMethods()
{
    // Arrange
    var mockPage = new Mock<IPage>();
    var {pageName}Page = new {PageName}Page(mockPage.Object);
    {var testData = ...;}

    // Act
    await {pageName}Page.{MethodName}Async({parameters});

    // Assert
    mockPage.Verify(p => p.{Method1}({params1}), Times.Once);
    mockPage.Verify(p => p.{Method2}({params2}), Times.Once);
    mockPage.Verify(p => p.{Method3}({params3}), Times.Once);
}
```

**Example (LoginAsync)**:
```csharp
[Fact]
[AllureDescription("Verifies that LoginAsync calls FillAsync for username, password, and ClickAsync for button")]
public async Task LoginAsync_CallsFillAndClick()
{
    // Arrange
    var mockPage = new Mock<IPage>();
    var loginPage = new LoginPage(mockPage.Object);
    var username = "test_user";
    var password = "test_pass";

    // Act
    await loginPage.LoginAsync(username, password);

    // Assert
    mockPage.Verify(p => p.FillAsync("[data-test='username']", username, null), Times.Once);
    mockPage.Verify(p => p.FillAsync("[data-test='password']", password, null), Times.Once);
    mockPage.Verify(p => p.ClickAsync("[data-test='login-button']", null), Times.Once);
}
```

---

### Test 3: {QueryMethod}_Returns{ExpectedValue}

**Test Method Name**: `{QueryMethod}_Returns{ExpectedValue}`

**Allure Attributes**:
```csharp
[AllureDescription("Verifies that {QueryMethod} returns {expected value} when element is {state}")]
[AllureSeverity(SeverityLevel.normal)]
[AllureOwner("QA Team")]
[AllureTag("Unit", "Query")]
```

**Given**:
- Mock IPage is configured to return {mockValue} from {PlaywrightMethod}
- {PageName}Page is instantiated

**When**:
- `{QueryMethod}Async()` is called

**Then**:
- Verify that {PlaywrightMethod} was called on IPage
- Verify that returned value equals {expected value}

**Code Structure**:
```csharp
[Fact]
[AllureDescription("Verifies that {QueryMethod} returns expected value")]
public async Task {QueryMethod}_ReturnsExpectedValue()
{
    // Arrange
    var mockPage = new Mock<IPage>();
    mockPage.Setup(p => p.{PlaywrightMethod}({selector})).ReturnsAsync({mockValue});
    var {pageName}Page = new {PageName}Page(mockPage.Object);

    // Act
    var result = await {pageName}Page.{QueryMethod}Async();

    // Assert
    Assert.Equal({expectedValue}, result);
    mockPage.Verify(p => p.{PlaywrightMethod}({selector}), Times.Once);
}
```

**Example (IsErrorMessageVisibleAsync)**:
```csharp
[Fact]
[AllureDescription("Verifies that IsErrorMessageVisibleAsync returns true when error is visible")]
public async Task IsErrorMessageVisibleAsync_WhenVisible_ReturnsTrue()
{
    // Arrange
    var mockPage = new Mock<IPage>();
    mockPage.Setup(p => p.IsVisibleAsync("[data-test='error']", null)).ReturnsAsync(true);
    var loginPage = new LoginPage(mockPage.Object);

    // Act
    var result = await loginPage.IsErrorMessageVisibleAsync();

    // Assert
    Assert.True(result);
    mockPage.Verify(p => p.IsVisibleAsync("[data-test='error']", null), Times.Once);
}
```

---

### Test 4: {MethodName}_With{InvalidInput}_ThrowsException (Optional)

**Test Method Name**: `{MethodName}_With{InvalidInput}_Throws{ExceptionType}`

**Allure Attributes**:
```csharp
[AllureDescription("Verifies that {MethodName} throws {ExceptionType} when {invalid condition}")]
[AllureSeverity(SeverityLevel.minor)]
[AllureOwner("QA Team")]
[AllureTag("Unit", "Negative", "Validation")]
```

**Given**:
- Mock IPage is created
- {PageName}Page is instantiated
- {Invalid input data}

**When**:
- `{MethodName}Async({invalidParams})` is called

**Then**:
- Verify that {ExceptionType} is thrown
- Verify exception message contains {expectedMessage}

**Code Structure**:
```csharp
[Fact]
[AllureDescription("Verifies that {MethodName} throws exception with invalid input")]
public async Task {MethodName}_WithInvalidInput_ThrowsException()
{
    // Arrange
    var mockPage = new Mock<IPage>();
    mockPage.Setup(p => p.{Method}({params})).ThrowsAsync(new PlaywrightException("Error"));
    var {pageName}Page = new {PageName}Page(mockPage.Object);

    // Act & Assert
    await Assert.ThrowsAsync<PlaywrightException>(async () =>
        await {pageName}Page.{MethodName}Async({invalidParams}));
}
```

---

## Additional Test Scenarios

<!-- Add more test scenarios as needed for comprehensive coverage -->

### Test: {ScenarioName}

**Given**: {Precondition}
**When**: {Action}
**Then**: {Verification}

**Code Example**:
```csharp
[Fact]
[AllureDescription("{Description}")]
public async Task {TestMethodName}()
{
    // Arrange
    // ...

    // Act
    // ...

    // Assert
    // ...
}
```

---

## Mocking Guidelines

### Basic Mock Setup

```csharp
var mockPage = new Mock<IPage>();
```

### Mock Method Return Value

```csharp
mockPage.Setup(p => p.MethodAsync({params}))
        .ReturnsAsync({returnValue});
```

### Mock Method Throws Exception

```csharp
mockPage.Setup(p => p.MethodAsync({params}))
        .ThrowsAsync(new PlaywrightException("Error message"));
```

### Verify Method Called

```csharp
mockPage.Verify(p => p.MethodAsync({expectedParams}), Times.Once);
```

### Verify Method Never Called

```csharp
mockPage.Verify(p => p.MethodAsync(It.IsAny<string>()), Times.Never);
```

### Verify Method Called With Any Parameters

```csharp
mockPage.Verify(p => p.MethodAsync(It.IsAny<string>()), Times.Once);
```

---

## Coverage Goals

**Target**: 100% method coverage for {PageName}Page class

**Methods to Test**:
- [ ] {Method 1}
- [ ] {Method 2}
- [ ] {Method 3}
- [ ] {Additional methods...}

**Edge Cases**:
- [ ] {Edge case 1 - e.g., "Empty string parameters"}
- [ ] {Edge case 2 - e.g., "Null parameters (if applicable)"}
- [ ] {Edge case 3 - e.g., "Special characters in input"}

---

## Mapping to Code

**Generated Class**: `src/Tests/{PageName}PageUnitTests.cs`

**Namespace**: `csharp_framework_demo.Tests`

**Class Attributes**:
- `[AllureSuite("Unit Tests")]`
- `[AllureFeature("{Feature}")]`

**Test Method Attributes**:
- `[Fact]`
- `[AllureDescription("...")]`
- `[AllureSeverity(SeverityLevel.normal)]`
- `[AllureOwner("QA Team")]`
- `[AllureTag("Unit", "Page Object")]`

**Dependencies**:
- `using Moq;`
- `using Microsoft.Playwright;`
- `using Xunit;`
- `using Allure.Xunit.Attributes;`
- `using csharp_framework_demo.Utilities.PageObjects;`

**Compliance**: Must follow PROJECT-SPEC.md Test Standards
