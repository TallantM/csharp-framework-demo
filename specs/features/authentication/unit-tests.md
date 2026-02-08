# Login Page Unit Tests Specification

## Test Suite Overview

**Test Class**: LoginPageUnitTests
**What We're Testing**: LoginPage Page Object methods in isolation
**Test Type**: Unit Tests
**Allure Suite**: "Unit Tests"
**Allure Feature**: "Login Page Object"

### Purpose
Verify that LoginPage methods call the correct Playwright methods with the correct parameters. These tests use mocks instead of a real browser, so they run fast and don't depend on the SauceDemo website.

---

## Test Configuration

**Mocking**: Use Moq to mock `IPage` so tests don't need a real browser
**Test Framework**: xUnit
**Reporting**: Allure with step-by-step descriptions

### Mocking Strategy
Create a mock IPage, pass it to LoginPage, call the Page Object method, then verify the mock received the expected calls.

---

## Test Scenarios

### Test 1: NavigateToAsync Calls GotoAsync With Correct URL

**What we verify**: NavigateToAsync should call Playwright's GotoAsync with the URL you provide

**Test Data**:
- URL: "https://www.saucedemo.com/"

**Expected Behavior**:
- GotoAsync is called exactly once
- GotoAsync receives the correct URL

**Severity**: Normal
**Tags**: Unit, Navigation

---

### Test 2: LoginAsync Fills Fields And Clicks Correct Elements

**What we verify**: LoginAsync should fill the username field, password field, and click the login button using the correct selectors

**Test Data**:
- Username: "test_user"
- Password: "test_pass"

**Expected Behavior**:
- FillAsync is called for selector `[data-test='username']` with the username
- FillAsync is called for selector `[data-test='password']` with the password
- ClickAsync is called for selector `[data-test='login-button']`
- Each method is called exactly once

**Severity**: Critical
**Tags**: Unit, Login

---

## Expected Outcomes

**When tests pass**: Confirms that LoginPage correctly delegates to Playwright methods
**When tests fail**: Indicates Page Object is using wrong selectors, calling wrong methods, or not calling methods at all

---

## Coverage

**What's covered**:
- Method delegation to Playwright
- Parameter passing
- Selector usage

**What's NOT covered**:
- Actual browser behavior (that's integration tests)
- Whether login actually works (that's integration tests)
