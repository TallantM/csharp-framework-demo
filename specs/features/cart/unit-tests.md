# Cart Page Object - Unit Tests Specification

## Test Suite Overview

**Test Class**: CartPageUnitTests
**What We're Testing**: CartPage methods in isolation
**Test Type**: Unit Tests
**Allure Suite**: "Unit Tests"
**Allure Feature**: "Cart Page Object"

### Purpose
Verify that CartPage methods call the correct Playwright methods with the correct parameters. Uses mocks instead of real browser for fast execution.

---

## Test Configuration

**Mocking**: Use Moq to mock `IPage`
**Test Framework**: xUnit
**Reporting**: Allure with step descriptions

### Mocking Strategy
Create mock IPage, pass to Page Object, call method, verify mock received expected calls

---

## Test Scenarios

### Test: GetCartItemCountAsync Calls Locator And CountAsync

**What we verify**: GetCartItemCountAsync should count elements with `.cart_item` selector

**Expected Behavior**:
- Locator called with `.cart_item`
- CountAsync called on locator
- Returns the count

**Severity**: Normal
**Tags**: Unit, Cart

---

### Test: GetCartItemNamesAsync Retrieves All Item Names

**What we verify**: GetCartItemNamesAsync should locate all product names and extract text

**Expected Behavior**:
- Locator called with `.inventory_item_name`
- AllTextContentsAsync called on locator
- Returns list of strings

**Severity**: Normal
**Tags**: Unit, Cart

---

### Test: RemoveItemAsync Clicks Correct Remove Button

**What we verify**: RemoveItemAsync should click the remove button with correct data-test attribute

**Test Data**:
- productName: "sauce-labs-backpack"

**Expected Behavior**:
- ClickAsync called with `[data-test='remove-sauce-labs-backpack']`
- Called exactly once

**Severity**: Critical
**Tags**: Unit, Cart

---

### Test: IsItemInCartAsync Checks For Product Name

**What we verify**: IsItemInCartAsync should search for product name in cart items

**Test Data**:
- productName: "Sauce Labs Backpack"

**Expected Behavior**:
- Locator called to find elements with matching text
- Returns boolean based on element existence

**Severity**: Normal
**Tags**: Unit, Cart

---

### Test: GetItemPriceAsync Returns Price For Product

**What we verify**: GetItemPriceAsync should locate product and return its price

**Test Data**:
- productName: "Sauce Labs Backpack"

**Expected Behavior**:
- Locator called to find product by name
- Price element located and text retrieved
- Returns price string

**Severity**: Normal
**Tags**: Unit, Cart

---

### Test: ClickContinueShoppingAsync Clicks Continue Button

**What we verify**: ClickContinueShoppingAsync should click the continue shopping button

**Expected Behavior**:
- ClickAsync called with `[data-test='continue-shopping']`
- Called exactly once

**Severity**: Normal
**Tags**: Unit, Navigation

---

### Test: ClickCheckoutAsync Clicks Checkout Button

**What we verify**: ClickCheckoutAsync should click the checkout button

**Expected Behavior**:
- ClickAsync called with `[data-test='checkout']`
- Called exactly once

**Severity**: Critical
**Tags**: Unit, Checkout

---

### Test: IsCartEmptyAsync Checks For Cart Items

**What we verify**: IsCartEmptyAsync should check if cart items exist

**Expected Behavior**:
- Locator called with `.cart_item`
- CountAsync or IsVisibleAsync called
- Returns boolean based on item count

**Severity**: Normal
**Tags**: Unit, Cart

---

### Test: ClickProductNameAsync Clicks Product Link

**What we verify**: ClickProductNameAsync should find and click the product name link

**Test Data**:
- productName: "Sauce Labs Backpack"

**Expected Behavior**:
- Locator or ClickAsync called to find element with matching text
- Element clicked exactly once

**Severity**: Normal
**Tags**: Unit, Navigation

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
- Button interactions
- Text and price retrieval

**What's NOT covered**:
- Actual browser behavior (that's integration tests)
