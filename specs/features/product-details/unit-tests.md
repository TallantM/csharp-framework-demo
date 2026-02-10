# Product Details Page Object - Unit Tests Specification

## Test Suite Overview

**Test Class**: ProductDetailsPageUnitTests
**What We're Testing**: ProductDetailsPage methods in isolation
**Test Type**: Unit Tests
**Allure Suite**: "Unit Tests"
**Allure Feature**: "Product Details Page Object"

### Purpose
Verify that ProductDetailsPage methods call the correct Playwright methods with the correct parameters. Uses mocks instead of real browser for fast execution.

---

## Test Configuration

**Mocking**: Use Moq to mock `IPage`
**Test Framework**: xUnit
**Reporting**: Allure with step descriptions

### Mocking Strategy
Create mock IPage, pass to Page Object, call method, verify mock received expected calls

---

## Test Scenarios

### Test: GetProductNameAsync Reads Product Name

**What we verify**: GetProductNameAsync should read text from product name element

**Expected Behavior**:
- Locator called with `.inventory_details_name`
- TextContentAsync or InnerTextAsync called
- Returns product name string

**Severity**: Normal
**Tags**: Unit, ProductDetails

---

### Test: GetProductDescriptionAsync Reads Product Description

**What we verify**: GetProductDescriptionAsync should read text from description element

**Expected Behavior**:
- Locator called with `.inventory_details_desc`
- TextContentAsync or InnerTextAsync called
- Returns description string

**Severity**: Normal
**Tags**: Unit, ProductDetails

---

### Test: GetProductPriceAsync Reads Product Price

**What we verify**: GetProductPriceAsync should read text from price element

**Expected Behavior**:
- Locator called with `.inventory_details_price`
- TextContentAsync or InnerTextAsync called
- Returns price string

**Severity**: Normal
**Tags**: Unit, ProductDetails

---

### Test: AddToCartAsync Clicks Add To Cart Button

**What we verify**: AddToCartAsync should click the add to cart button

**Expected Behavior**:
- ClickAsync called with `[data-test='add-to-cart']`
- Called exactly once

**Severity**: Critical
**Tags**: Unit, Cart

---

### Test: RemoveFromCartAsync Clicks Remove Button

**What we verify**: RemoveFromCartAsync should click the remove button

**Expected Behavior**:
- ClickAsync called with `[data-test='remove']`
- Called exactly once

**Severity**: Critical
**Tags**: Unit, Cart

---

### Test: IsProductInCartAsync Checks Remove Button Visibility

**What we verify**: IsProductInCartAsync should check if remove button is visible

**Expected Behavior**:
- IsVisibleAsync called with `[data-test='remove']`
- Returns boolean result

**Severity**: Normal
**Tags**: Unit, Cart

---

### Test: ClickBackToProductsAsync Clicks Back Button

**What we verify**: ClickBackToProductsAsync should click the back to products button

**Expected Behavior**:
- ClickAsync called with `[data-test='back-to-products']`
- Called exactly once

**Severity**: Normal
**Tags**: Unit, Navigation

---

### Test: GetCartItemCountAsync Reads Cart Badge

**What we verify**: GetCartItemCountAsync should read cart badge text and parse to integer

**Expected Behavior**:
- Locator called with `.shopping_cart_badge`
- TextContentAsync or InnerTextAsync called
- Returns parsed integer

**Severity**: Normal
**Tags**: Unit, Cart

---

### Test: IsImageVisibleAsync Checks Image Visibility

**What we verify**: IsImageVisibleAsync should check if product image is visible

**Expected Behavior**:
- IsVisibleAsync called with `.inventory_details_img`
- Returns boolean result

**Severity**: Normal
**Tags**: Unit, ProductDetails

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
- Visibility checks

**What's NOT covered**:
- Actual browser behavior (that's integration tests)
