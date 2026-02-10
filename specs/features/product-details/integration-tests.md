# Product Details Page Integration Tests Specification

## Test Suite Overview

**Test Class**: ProductDetailsPageIntegrationTests
**What We're Testing**: ProductDetailsPage with real Playwright browser
**Test Type**: Integration Tests
**Allure Suite**: "Integration Tests"
**Allure Feature**: "Product Details Page Object"

### Purpose
Verify ProductDetailsPage works with a real browser and the actual SauceDemo website. Tests check that selectors are correct and product detail interactions behave as expected.

---

## Test Configuration

**Browser**: Real Playwright browser (Chromium, headless)
**Fixture**: PlaywrightFixture
**Target**: https://www.saucedemo.com/inventory-item.html?id=4
**Prerequisites**: User must be logged in

---

## Test Scenarios

### Test: Get Product Name Returns Correct Name

**What we verify**: GetProductNameAsync returns the actual product name

**Steps**:
1. Login and navigate to specific product detail page
2. Call GetProductNameAsync
3. Verify name matches expected product

**Test Data**:
- Product ID: 4 (Sauce Labs Backpack)

**Expected**:
- Returns "Sauce Labs Backpack"

**Severity**: Normal
**Tags**: Integration, ProductDetails

---

### Test: Get Product Description Returns Full Text

**What we verify**: GetProductDescriptionAsync returns complete description

**Steps**:
1. Login and navigate to product detail page
2. Call GetProductDescriptionAsync
3. Verify description is non-empty and contains expected text

**Expected**:
- Description contains product information
- Text is non-empty

**Severity**: Normal
**Tags**: Integration, ProductDetails

---

### Test: Get Product Price Returns Correct Price

**What we verify**: GetProductPriceAsync returns the correct price

**Steps**:
1. Login and navigate to product detail page
2. Call GetProductPriceAsync
3. Verify price matches known product price

**Test Data**:
- Product: Sauce Labs Backpack

**Expected**:
- Returns "$29.99"

**Severity**: Normal
**Tags**: Integration, ProductDetails

---

### Test: Add To Cart Updates Button And Badge

**What we verify**: AddToCartAsync adds product and updates UI

**Steps**:
1. Login and navigate to product detail page
2. Call AddToCartAsync
3. Verify button changes to "Remove"
4. Verify cart badge shows "1"

**Expected**:
- Remove button visible
- Add to Cart button not visible
- Cart badge displays "1"

**Severity**: Critical
**Tags**: Integration, Cart

---

### Test: Remove From Cart Updates Button And Badge

**What we verify**: RemoveFromCartAsync removes product and updates UI

**Steps**:
1. Login, navigate to product detail, add to cart
2. Call RemoveFromCartAsync
3. Verify button changes back to "Add to Cart"
4. Verify cart badge disappears

**Expected**:
- Add to Cart button visible
- Remove button not visible
- Cart badge not visible

**Severity**: Critical
**Tags**: Integration, Cart

---

### Test: Is Product In Cart Returns True After Adding

**What we verify**: IsProductInCartAsync correctly identifies cart state

**Steps**:
1. Login and navigate to product detail page
2. Verify IsProductInCartAsync returns false initially
3. Add product to cart
4. Verify IsProductInCartAsync returns true

**Expected**:
- Returns false before adding
- Returns true after adding

**Severity**: Normal
**Tags**: Integration, Cart

---

### Test: Click Back To Products Navigates To Inventory

**What we verify**: ClickBackToProductsAsync returns to inventory page

**Steps**:
1. Login and navigate to product detail page
2. Call ClickBackToProductsAsync
3. Verify URL is inventory page

**Expected**:
- URL: "https://www.saucedemo.com/inventory.html"

**Severity**: Normal
**Tags**: Integration, Navigation

---

### Test: Get Cart Item Count Returns Accurate Count

**What we verify**: GetCartItemCountAsync returns correct cart badge number

**Steps**:
1. Login and navigate to product detail page
2. Add product to cart
3. Call GetCartItemCountAsync
4. Verify count is 1

**Expected**:
- Returns 1 after adding one item
- Returns 0 when badge not visible

**Severity**: Normal
**Tags**: Integration, Cart

---

### Test: Is Image Visible Returns True

**What we verify**: IsImageVisibleAsync confirms product image is displayed

**Steps**:
1. Login and navigate to product detail page
2. Call IsImageVisibleAsync
3. Verify returns true

**Expected**:
- Returns true for valid product page

**Severity**: Normal
**Tags**: Integration, ProductDetails

---

## Coverage

**What's covered**:
- Page Object + Playwright integration
- Correct selectors
- Product information retrieval
- Cart operations
- Button state changes
- Navigation flows

**What's NOT covered**:
- Complete user workflows (E2E tests handle that)
