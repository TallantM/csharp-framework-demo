# Checkout Page Integration Tests Specification

## Test Suite Overview

**Test Class**: CheckoutPageIntegrationTests
**What We're Testing**: CheckoutPage with real Playwright browser
**Test Type**: Integration Tests
**Allure Suite**: "Integration Tests"
**Allure Feature**: "Checkout Page Object"

### Purpose
Verify CheckoutPage works with a real browser and the actual SauceDemo website. Tests check that selectors are correct and checkout flow behaves as expected.

---

## Test Configuration

**Browser**: Real Playwright browser (Chromium, headless)
**Fixture**: PlaywrightFixture
**Target**: https://www.saucedemo.com/checkout-step-one.html
**Prerequisites**: User must be logged in with items in cart

---

## Test Scenarios

### Test: Fill Checkout Information Populates Form Fields

**What we verify**: FillCheckoutInformationAsync correctly fills all form fields

**Steps**:
1. Login, add item, navigate to checkout
2. Call FillCheckoutInformationAsync
3. Verify fields contain entered values

**Test Data**:
- firstName: "John"
- lastName: "Doe"
- postalCode: "12345"

**Expected**:
- All three fields populated with correct values

**Severity**: Critical
**Tags**: Integration, Checkout

---

### Test: Click Continue Navigates To Step Two

**What we verify**: ClickContinueAsync advances to order review page

**Steps**:
1. Login, add item, navigate to checkout
2. Fill checkout information
3. Call ClickContinueAsync
4. Verify URL is step two

**Expected**:
- URL: "https://www.saucedemo.com/checkout-step-two.html"
- Order summary visible

**Severity**: Critical
**Tags**: Integration, Checkout

---

### Test: Click Cancel Returns To Cart

**What we verify**: ClickCancelAsync navigates back to cart page

**Steps**:
1. Login, add item, navigate to checkout
2. Call ClickCancelAsync
3. Verify URL is cart page

**Expected**:
- URL: "https://www.saucedemo.com/cart.html"

**Severity**: Normal
**Tags**: Integration, Navigation

---

### Test: Get Subtotal Returns Correct Amount

**What we verify**: GetSubtotalAsync retrieves the correct subtotal from summary

**Steps**:
1. Login, add item with known price, complete step one
2. Navigate to step two
3. Call GetSubtotalAsync
4. Verify amount matches item price

**Expected**:
- Subtotal text contains correct price

**Severity**: Normal
**Tags**: Integration, Checkout

---

### Test: Get Tax Returns Tax Amount

**What we verify**: GetTaxAsync retrieves the tax calculation

**Steps**:
1. Login, add item, complete step one
2. Navigate to step two
3. Call GetTaxAsync
4. Verify tax amount is present

**Expected**:
- Tax text contains dollar amount
- Tax is non-zero

**Severity**: Normal
**Tags**: Integration, Checkout

---

### Test: Get Total Returns Final Amount

**What we verify**: GetTotalAsync retrieves the final total with tax

**Steps**:
1. Login, add item, complete step one
2. Navigate to step two
3. Call GetTotalAsync
4. Verify total equals subtotal plus tax

**Expected**:
- Total text contains final amount
- Total is subtotal + tax

**Severity**: Critical
**Tags**: Integration, Checkout

---

### Test: Click Finish Completes Order

**What we verify**: ClickFinishAsync completes purchase and shows confirmation

**Steps**:
1. Login, add item, complete steps one and two
2. Call ClickFinishAsync
3. Verify URL is completion page
4. Verify confirmation message visible

**Expected**:
- URL: "https://www.saucedemo.com/checkout-complete.html"
- Confirmation header visible

**Severity**: Critical
**Tags**: Integration, Checkout

---

### Test: Get Confirmation Message Returns Success Text

**What we verify**: GetConfirmationMessageAsync retrieves confirmation header

**Steps**:
1. Login, add item, complete checkout
2. Call GetConfirmationMessageAsync
3. Verify message indicates success

**Expected**:
- Message: "Thank you for your order!" or similar

**Severity**: Normal
**Tags**: Integration, Checkout

---

### Test: Get Confirmation Details Returns Order Info

**What we verify**: GetConfirmationDetailsAsync retrieves order details

**Steps**:
1. Login, add item, complete checkout
2. Call GetConfirmationDetailsAsync
3. Verify details text is present

**Expected**:
- Details text describes order dispatch

**Severity**: Normal
**Tags**: Integration, Checkout

---

### Test: Click Back To Products Returns To Inventory

**What we verify**: ClickBackToProductsAsync navigates to inventory after completion

**Steps**:
1. Login, add item, complete checkout
2. Call ClickBackToProductsAsync
3. Verify URL is inventory page

**Expected**:
- URL: "https://www.saucedemo.com/inventory.html"

**Severity**: Normal
**Tags**: Integration, Navigation

---

### Test: Missing Information Shows Error

**What we verify**: Validation errors appear when required fields are empty

**Steps**:
1. Login, add item, navigate to checkout
2. Click continue without filling form
3. Call IsErrorVisibleAsync
4. Call GetErrorMessageAsync
5. Verify error indicates missing information

**Expected**:
- IsErrorVisibleAsync returns true
- Error message describes missing field

**Severity**: Critical
**Tags**: Integration, Validation, Negative

---

## Coverage

**What's covered**:
- Page Object + Playwright integration
- Correct selectors
- Multi-step checkout flow
- Form validation
- Order summary calculations
- Navigation flows
- Error handling

**What's NOT covered**:
- Complete user workflows (E2E tests handle that)
