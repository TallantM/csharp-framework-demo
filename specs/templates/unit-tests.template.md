# {PageName} Page Object - Unit Tests Specification

<!--
INSTRUCTIONS:
- Replace {PageName} with your page name
- Replace {Feature} with the Allure feature name
- List test scenarios for each Page Object method
- Focus on WHAT gets tested, not HOW to code it
- Use Given/When/Then to describe test behavior
-->

## Test Suite Overview

**Test Class**: {PageName}PageUnitTests
**What We're Testing**: {PageName}Page methods in isolation
**Test Type**: Unit Tests
**Allure Suite**: "Unit Tests"
**Allure Feature**: "{Feature}"

### Purpose
Verify that {PageName}Page methods call the correct Playwright methods with the correct parameters. Uses mocks instead of real browser for fast execution.

---

## Test Configuration

**Mocking**: Use Moq to mock `IPage`
**Test Framework**: xUnit
**Reporting**: Allure with step descriptions

### Mocking Strategy
Create mock IPage, pass to Page Object, call method, verify mock received expected calls

---

## Test Scenarios

### Test: {MethodName} Calls {PlaywrightMethod}

**What we verify**: {MethodName} should call {PlaywrightMethod} with correct parameters

**Test Data**:
- {param1}: {value}
- {param2}: {value}

**Expected Behavior**:
- {PlaywrightMethod} is called exactly once
- Called with correct selector/parameters

**Severity**: {Critical/Normal}
**Tags**: Unit, {Area}

---

### Test: {AnotherMethod} Calls Multiple Methods

**What we verify**: {AnotherMethod} should call {Method1}, {Method2}, {Method3} in sequence

**Test Data**:
- {test data}

**Expected Behavior**:
- All methods called exactly once
- Called with correct parameters
- Called in correct order (if order matters)

**Severity**: {Critical/Normal}
**Tags**: Unit, {Area}

---

## Expected Outcomes

**When tests pass**: Confirms Page Object correctly delegates to Playwright
**When tests fail**: Indicates wrong selectors, wrong methods, or wrong parameters

---

## Coverage

**What's covered**:
- Method delegation
- Parameter passing
- Selector usage

**What's NOT covered**:
- Actual browser behavior (that's integration tests)
