# Inventory Page Object - Unit Tests Specification

## Test Suite Overview

**Test Class**: InventoryPageUnitTests
**What We're Testing**: InventoryPage methods in isolation
**Test Type**: Unit Tests
**Allure Suite**: "Unit Tests"
**Allure Feature**: "Inventory Page Object"

### Purpose
Verify that InventoryPage methods call the correct Playwright methods with the correct parameters. Uses mocks instead of real browser for fast execution.

---

## Test Configuration

**Mocking**: Use Moq to mock `IPage`
**Test Framework**: xUnit
**Reporting**: Allure with step descriptions

### Mocking Strategy
Create mock IPage, pass to Page Object, call method, verify mock received expected calls

---

## Test Scenarios

### Test: GetProductCountAsync Calls Locator And CountAsync

**What we verify**: GetProductCountAsync should count elements with `.inventory_item` selector

**Expected Behavior**:
- Locator called with `.inventory_item`
- CountAsync called on locator
- Returns the count

**Severity**: Normal
**Tags**: Unit, Inventory

---

### Test: GetProductNamesAsync Retrieves All Product Names

**What we verify**: GetProductNamesAsync should locate all product name elements and extract text

**Expected Behavior**:
- Locator called with `.inventory_item_name`
- AllTextContentsAsync called on locator
- Returns list of strings

**Severity**: Normal
**Tags**: Unit, Inventory

---

### Test: AddToCartAsync Clicks Correct Button

**What we verify**: AddToCartAsync should click the add-to-cart button with correct data-test attribute

**Test Data**:
- productName: "sauce-labs-backpack"

**Expected Behavior**:
- ClickAsync called with `[data-test='add-to-cart-sauce-labs-backpack']`
- Called exactly once

**Severity**: Critical
**Tags**: Unit, Cart

---

### Test: RemoveFromCartAsync Clicks Correct Button

**What we verify**: RemoveFromCartAsync should click the remove button with correct data-test attribute

**Test Data**:
- productName: "sauce-labs-bike-light"

**Expected Behavior**:
- ClickAsync called with `[data-test='remove-sauce-labs-bike-light']`
- Called exactly once

**Severity**: Critical
**Tags**: Unit, Cart

---

### Test: GetCartItemCountAsync Reads Badge Text

**What we verify**: GetCartItemCountAsync should read the cart badge text and parse to integer

**Expected Behavior**:
- Locator called with `.shopping_cart_badge`
- TextContentAsync or InnerTextAsync called
- Returns parsed integer

**Severity**: Normal
**Tags**: Unit, Cart

---

### Test: ClickProductAsync Clicks Product Name Link

**What we verify**: ClickProductAsync should find and click the product name link

**Test Data**:
- productName: "Sauce Labs Backpack"

**Expected Behavior**:
- Locator or ClickAsync called to find element with matching text
- Element clicked exactly once

**Severity**: Normal
**Tags**: Unit, Navigation

---

### Test: NavigateToCartAsync Clicks Cart Link

**What we verify**: NavigateToCartAsync should click the cart icon link

**Expected Behavior**:
- ClickAsync called with `.shopping_cart_link`
- Called exactly once

**Severity**: Normal
**Tags**: Unit, Navigation

---

### Test: SortProductsAsync Selects Dropdown Option

**What we verify**: SortProductsAsync should select the specified option from sort dropdown

**Test Data**:
- sortOption: "lohi"

**Expected Behavior**:
- SelectOptionAsync called with `.product_sort_container` and value "lohi"
- Called exactly once

**Severity**: Normal
**Tags**: Unit, Sorting

---

### Test: IsProductInCartAsync Checks Button Visibility

**What we verify**: IsProductInCartAsync should check if remove button is visible

**Test Data**:
- productName: "sauce-labs-backpack"

**Expected Behavior**:
- IsVisibleAsync called with `[data-test='remove-sauce-labs-backpack']`
- Returns boolean result

**Severity**: Normal
**Tags**: Unit, Cart

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
- Text retrieval

**What's NOT covered**:
- Actual browser behavior (that's integration tests)
