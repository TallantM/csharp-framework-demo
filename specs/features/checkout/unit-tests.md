# Checkout Page Object - Unit Tests Specification

## Test Suite Overview

**Test Class**: CheckoutPageUnitTests
**What We're Testing**: CheckoutPage methods in isolation
**Test Type**: Unit Tests
**Allure Suite**: "Unit Tests"
**Allure Feature**: "Checkout Page Object"

### Purpose
Verify that CheckoutPage methods call the correct Playwright methods with the correct parameters. Uses mocks instead of real browser for fast execution.

---

## Test Configuration

**Mocking**: Use Moq to mock `IPage`
**Test Framework**: xUnit
**Reporting**: Allure with step descriptions

### Mocking Strategy
Create mock IPage, pass to Page Object, call method, verify mock received expected calls

---

## Test Scenarios

### Test: FillCheckoutInformationAsync Fills All Three Fields

**What we verify**: FillCheckoutInformationAsync should fill firstName, lastName, and postalCode fields

**Test Data**:
- firstName: "John"
- lastName: "Doe"
- postalCode: "12345"

**Expected Behavior**:
- FillAsync called with `[data-test='firstName']` and "John"
- FillAsync called with `[data-test='lastName']` and "Doe"
- FillAsync called with `[data-test='postalCode']` and "12345"
- Each field filled exactly once

**Severity**: Critical
**Tags**: Unit, Checkout

---

### Test: ClickContinueAsync Clicks Continue Button

**What we verify**: ClickContinueAsync should click the continue button

**Expected Behavior**:
- ClickAsync called with `[data-test='continue']`
- Called exactly once

**Severity**: Critical
**Tags**: Unit, Checkout

---

### Test: ClickCancelAsync Clicks Cancel Button

**What we verify**: ClickCancelAsync should click the cancel button

**Expected Behavior**:
- ClickAsync called with `[data-test='cancel']`
- Called exactly once

**Severity**: Normal
**Tags**: Unit, Checkout

---

### Test: GetSubtotalAsync Reads Subtotal Label

**What we verify**: GetSubtotalAsync should read text from subtotal element

**Expected Behavior**:
- Locator called with `.summary_subtotal_label`
- TextContentAsync or InnerTextAsync called
- Returns text content

**Severity**: Normal
**Tags**: Unit, Checkout

---

### Test: GetTaxAsync Reads Tax Label

**What we verify**: GetTaxAsync should read text from tax element

**Expected Behavior**:
- Locator called with `.summary_tax_label`
- TextContentAsync or InnerTextAsync called
- Returns text content

**Severity**: Normal
**Tags**: Unit, Checkout

---

### Test: GetTotalAsync Reads Total Label

**What we verify**: GetTotalAsync should read text from total element

**Expected Behavior**:
- Locator called with `.summary_total_label`
- TextContentAsync or InnerTextAsync called
- Returns text content

**Severity**: Normal
**Tags**: Unit, Checkout

---

### Test: ClickFinishAsync Clicks Finish Button

**What we verify**: ClickFinishAsync should click the finish button

**Expected Behavior**:
- ClickAsync called with `[data-test='finish']`
- Called exactly once

**Severity**: Critical
**Tags**: Unit, Checkout

---

### Test: GetConfirmationMessageAsync Reads Completion Header

**What we verify**: GetConfirmationMessageAsync should read confirmation header text

**Expected Behavior**:
- Locator called with `.complete-header`
- TextContentAsync or InnerTextAsync called
- Returns confirmation message

**Severity**: Normal
**Tags**: Unit, Checkout

---

### Test: GetConfirmationDetailsAsync Reads Completion Text

**What we verify**: GetConfirmationDetailsAsync should read confirmation details text

**Expected Behavior**:
- Locator called with `.complete-text`
- TextContentAsync or InnerTextAsync called
- Returns details text

**Severity**: Normal
**Tags**: Unit, Checkout

---

### Test: ClickBackToProductsAsync Clicks Back Button

**What we verify**: ClickBackToProductsAsync should click the back to products button

**Expected Behavior**:
- ClickAsync called with `[data-test='back-to-products']`
- Called exactly once

**Severity**: Normal
**Tags**: Unit, Navigation

---

### Test: IsErrorVisibleAsync Checks Error Element Visibility

**What we verify**: IsErrorVisibleAsync should check if error message is visible

**Expected Behavior**:
- IsVisibleAsync called with `[data-test='error']`
- Returns boolean result

**Severity**: Normal
**Tags**: Unit, Validation

---

### Test: GetErrorMessageAsync Reads Error Text

**What we verify**: GetErrorMessageAsync should read error message text

**Expected Behavior**:
- Locator called with `[data-test='error']`
- TextContentAsync or InnerTextAsync called
- Returns error text

**Severity**: Normal
**Tags**: Unit, Validation

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
- Form filling
- Button clicks
- Text retrieval

**What's NOT covered**:
- Actual browser behavior (that's integration tests)
