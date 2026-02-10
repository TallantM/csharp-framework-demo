# Inventory Page Integration Tests Specification

## Test Suite Overview

**Test Class**: InventoryPageIntegrationTests
**What We're Testing**: InventoryPage with real Playwright browser
**Test Type**: Integration Tests
**Allure Suite**: "Integration Tests"
**Allure Feature**: "Inventory Page Object"

### Purpose
Verify InventoryPage works with a real browser and the actual SauceDemo website. Tests check that selectors are correct and inventory interactions behave as expected.

---

## Test Configuration

**Browser**: Real Playwright browser (Chromium, headless)
**Fixture**: PlaywrightFixture
**Target**: https://www.saucedemo.com/inventory.html
**Prerequisites**: User must be logged in

---

## Test Scenarios

### Test: Get Product Count Returns Correct Number

**What we verify**: GetProductCountAsync returns the actual number of products displayed

**Steps**:
1. Login and navigate to inventory page
2. Call GetProductCountAsync
3. Verify count matches expected product count

**Expected**:
- Returns 6 (standard product count on SauceDemo)

**Severity**: Normal
**Tags**: Integration, Inventory

---

### Test: Get Product Names Returns All Names

**What we verify**: GetProductNamesAsync retrieves all visible product names

**Steps**:
1. Login and navigate to inventory page
2. Call GetProductNamesAsync
3. Verify list contains expected product names

**Expected**:
- List contains "Sauce Labs Backpack", "Sauce Labs Bike Light", etc.
- All 6 product names present

**Severity**: Normal
**Tags**: Integration, Inventory

---

### Test: Add To Cart Updates Badge Count

**What we verify**: Adding a product to cart updates the cart badge

**Steps**:
1. Login and navigate to inventory page
2. Call AddToCartAsync with product name
3. Verify cart badge shows "1"
4. Verify button changes to "Remove"

**Test Data**:
- productName: "sauce-labs-backpack"

**Expected**:
- Cart badge displays "1"
- Add button replaced with Remove button

**Severity**: Critical
**Tags**: Integration, Cart

---

### Test: Remove From Cart Updates Badge Count

**What we verify**: Removing a product from cart updates the cart badge

**Steps**:
1. Login and navigate to inventory page
2. Add a product to cart
3. Call RemoveFromCartAsync with same product
4. Verify cart badge disappears or shows decremented count

**Test Data**:
- productName: "sauce-labs-backpack"

**Expected**:
- Cart badge not visible or shows "0"
- Remove button replaced with Add to Cart button

**Severity**: Critical
**Tags**: Integration, Cart

---

### Test: Click Product Navigates To Details

**What we verify**: Clicking a product name navigates to product detail page

**Steps**:
1. Login and navigate to inventory page
2. Call ClickProductAsync with product name
3. Verify URL changes to product detail page

**Test Data**:
- productName: "Sauce Labs Backpack"

**Expected**:
- URL contains "inventory-item.html?id="
- Product detail page loads

**Severity**: Normal
**Tags**: Integration, Navigation

---

### Test: Navigate To Cart Opens Cart Page

**What we verify**: NavigateToCartAsync navigates to the cart page

**Steps**:
1. Login and navigate to inventory page
2. Call NavigateToCartAsync
3. Verify URL is cart page

**Expected**:
- URL: "https://www.saucedemo.com/cart.html"

**Severity**: Normal
**Tags**: Integration, Navigation

---

### Test: Sort Products Changes Display Order

**What we verify**: SortProductsAsync reorders products based on selected option

**Steps**:
1. Login and navigate to inventory page
2. Get initial product order
3. Call SortProductsAsync with sort option
4. Verify products are reordered correctly

**Test Data**:
- sortOption: "lohi" (price low to high)

**Expected**:
- Products sorted by price ascending
- First product has lowest price

**Severity**: Normal
**Tags**: Integration, Sorting

---

### Test: Is Product In Cart Returns True After Adding

**What we verify**: IsProductInCartAsync correctly identifies products in cart

**Steps**:
1. Login and navigate to inventory page
2. Add product to cart
3. Call IsProductInCartAsync
4. Verify returns true

**Test Data**:
- productName: "sauce-labs-backpack"

**Expected**:
- Returns true after adding to cart
- Returns false before adding to cart

**Severity**: Normal
**Tags**: Integration, Cart

---

## Coverage

**What's covered**:
- Page Object + Playwright integration
- Correct selectors
- Cart operations
- Navigation flows
- Sorting functionality
- State verification

**What's NOT covered**:
- Complete user workflows (E2E tests handle that)
