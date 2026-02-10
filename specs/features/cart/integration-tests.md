# Cart Page Integration Tests Specification

## Test Suite Overview

**Test Class**: CartPageIntegrationTests
**What We're Testing**: CartPage with real Playwright browser
**Test Type**: Integration Tests
**Allure Suite**: "Integration Tests"
**Allure Feature**: "Cart Page Object"

### Purpose
Verify CartPage works with a real browser and the actual SauceDemo website. Tests check that selectors are correct and cart operations behave as expected.

---

## Test Configuration

**Browser**: Real Playwright browser (Chromium, headless)
**Fixture**: PlaywrightFixture
**Target**: https://www.saucedemo.com/cart.html
**Prerequisites**: User must be logged in, items added to cart

---

## Test Scenarios

### Test: Get Cart Item Count Returns Correct Number

**What we verify**: GetCartItemCountAsync returns the actual number of items in cart

**Steps**:
1. Login and add 2 products to cart
2. Navigate to cart page
3. Call GetCartItemCountAsync
4. Verify count is 2

**Expected**:
- Returns 2

**Severity**: Normal
**Tags**: Integration, Cart

---

### Test: Get Cart Item Names Returns All Names

**What we verify**: GetCartItemNamesAsync retrieves all product names in cart

**Steps**:
1. Login and add specific products to cart
2. Navigate to cart page
3. Call GetCartItemNamesAsync
4. Verify list contains expected names

**Test Data**:
- Products: "Sauce Labs Backpack", "Sauce Labs Bike Light"

**Expected**:
- List contains both product names

**Severity**: Normal
**Tags**: Integration, Cart

---

### Test: Remove Item Removes Product From Cart

**What we verify**: RemoveItemAsync removes the product and updates the display

**Steps**:
1. Login and add 2 products to cart
2. Navigate to cart page
3. Call RemoveItemAsync for one product
4. Verify item is removed from cart
5. Verify cart count decreased

**Test Data**:
- productName: "sauce-labs-backpack"

**Expected**:
- Product no longer visible in cart
- Cart count shows 1

**Severity**: Critical
**Tags**: Integration, Cart

---

### Test: Is Item In Cart Returns True For Added Items

**What we verify**: IsItemInCartAsync correctly identifies items in the cart

**Steps**:
1. Login and add product to cart
2. Navigate to cart page
3. Call IsItemInCartAsync for added product
4. Verify returns true

**Test Data**:
- productName: "Sauce Labs Backpack"

**Expected**:
- Returns true for items in cart
- Returns false for items not in cart

**Severity**: Normal
**Tags**: Integration, Cart

---

### Test: Get Item Price Returns Correct Price

**What we verify**: GetItemPriceAsync returns the correct price for a cart item

**Steps**:
1. Login and add product to cart
2. Navigate to cart page
3. Call GetItemPriceAsync for the product
4. Verify price matches expected value

**Test Data**:
- productName: "Sauce Labs Backpack"

**Expected**:
- Returns "$29.99"

**Severity**: Normal
**Tags**: Integration, Cart

---

### Test: Click Continue Shopping Navigates To Inventory

**What we verify**: ClickContinueShoppingAsync navigates back to inventory page

**Steps**:
1. Login and navigate to cart page
2. Call ClickContinueShoppingAsync
3. Verify URL is inventory page

**Expected**:
- URL: "https://www.saucedemo.com/inventory.html"

**Severity**: Normal
**Tags**: Integration, Navigation

---

### Test: Click Checkout Navigates To Checkout Form

**What we verify**: ClickCheckoutAsync navigates to checkout page

**Steps**:
1. Login, add item to cart, navigate to cart
2. Call ClickCheckoutAsync
3. Verify URL is checkout step one

**Expected**:
- URL: "https://www.saucedemo.com/checkout-step-one.html"

**Severity**: Critical
**Tags**: Integration, Checkout

---

### Test: Is Cart Empty Returns True For Empty Cart

**What we verify**: IsCartEmptyAsync correctly identifies empty cart state

**Steps**:
1. Login and navigate to cart with no items
2. Call IsCartEmptyAsync
3. Verify returns true

**Expected**:
- Returns true when cart is empty
- Returns false when items exist

**Severity**: Normal
**Tags**: Integration, Cart

---

### Test: Click Product Name Navigates To Details

**What we verify**: ClickProductNameAsync navigates to product detail page

**Steps**:
1. Login, add product, navigate to cart
2. Call ClickProductNameAsync
3. Verify URL is product detail page

**Test Data**:
- productName: "Sauce Labs Backpack"

**Expected**:
- URL contains "inventory-item.html?id="

**Severity**: Normal
**Tags**: Integration, Navigation

---

## Coverage

**What's covered**:
- Page Object + Playwright integration
- Correct selectors
- Item removal
- Cart state verification
- Navigation flows
- Price retrieval

**What's NOT covered**:
- Complete user workflows (E2E tests handle that)
