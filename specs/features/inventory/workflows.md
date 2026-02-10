# Inventory - End-to-End Workflow Specification

## Test Suite Overview

**Test Class**: InventoryWorkflowTests
**What We're Testing**: Complete inventory and shopping workflows from end to end
**Test Type**: End-to-End Tests
**Allure Suite**: "End-to-End Tests"
**Allure Feature**: "Inventory"

### Purpose
Test complete user journeys involving product browsing, cart management, and navigation. These verify the entire shopping flow works correctly.

---

## Test Configuration

**Browser**: Real Playwright browser (Chromium, headless)
**Fixture**: PlaywrightFixture
**Target**: https://www.saucedemo.com/
**Test Users**: standard_user / secret_sauce

---

## Workflow Scenarios

### Workflow: Browse Products After Login

**User Story**: User logs in and views the product catalog

**Steps**:
1. Navigate to login page
2. Login with valid credentials
3. Verify inventory page loads
4. Verify all products are displayed
5. Verify product details are visible (name, price, description)

**Test Data**:
- Username: "standard_user"
- Password: "secret_sauce"

**Expected Outcome**:
- 6 products displayed
- Each product shows name, price, description, and Add to Cart button

**Severity**: Critical
**Tags**: Smoke, Inventory

---

### Workflow: Add Single Product To Cart

**User Story**: User adds one product to cart and cart badge updates

**Steps**:
1. Login and navigate to inventory
2. Add a product to cart
3. Verify cart badge shows "1"
4. Verify button changes to "Remove"
5. Click cart to verify product appears

**Test Data**:
- Product: "Sauce Labs Backpack"

**Expected Outcome**:
- Cart badge displays "1"
- Product appears in cart page
- Button label is "Remove"

**Severity**: Critical
**Tags**: Smoke, Cart

---

### Workflow: Add Multiple Products To Cart

**User Story**: User adds multiple products and cart count reflects total

**Steps**:
1. Login and navigate to inventory
2. Add three different products to cart
3. Verify cart badge shows "3"
4. Navigate to cart
5. Verify all three products are listed

**Test Data**:
- Products: "Sauce Labs Backpack", "Sauce Labs Bike Light", "Sauce Labs Bolt T-Shirt"

**Expected Outcome**:
- Cart badge shows "3"
- All three products visible in cart
- Correct names and prices displayed

**Severity**: Critical
**Tags**: Smoke, Cart

---

### Workflow: Remove Product From Cart

**User Story**: User removes a product from cart and count updates

**Steps**:
1. Login and navigate to inventory
2. Add two products to cart
3. Remove one product
4. Verify cart badge shows "1"
5. Verify button changes back to "Add to Cart"

**Test Data**:
- Add: "Sauce Labs Backpack", "Sauce Labs Bike Light"
- Remove: "Sauce Labs Backpack"

**Expected Outcome**:
- Cart badge shows "1"
- Only Bike Light remains in cart
- Backpack button shows "Add to Cart"

**Severity**: Normal
**Tags**: Regression, Cart

---

### Workflow: Navigate To Product Details

**User Story**: User clicks product name to view details

**Steps**:
1. Login and navigate to inventory
2. Click on a product name
3. Verify redirected to product detail page
4. Verify product information is displayed
5. Verify Back button exists

**Test Data**:
- Product: "Sauce Labs Backpack"

**Expected Outcome**:
- URL contains "inventory-item.html?id="
- Product name, description, price visible
- Add to Cart button available

**Severity**: Normal
**Tags**: Regression, Navigation

---

### Workflow: Sort Products By Price Low To High

**User Story**: User sorts products to see cheapest items first

**Steps**:
1. Login and navigate to inventory
2. Select "Price (low to high)" from sort dropdown
3. Verify products are sorted correctly
4. Verify first product has lowest price

**Expected Outcome**:
- Products arranged in ascending price order
- Sort dropdown shows "Price (low to high)"

**Severity**: Normal
**Tags**: Regression, Sorting

---

### Workflow: Sort Products By Name Z to A

**User Story**: User sorts products alphabetically in reverse

**Steps**:
1. Login and navigate to inventory
2. Select "Name (Z to A)" from sort dropdown
3. Verify products are sorted reverse alphabetically
4. Verify product order matches expectation

**Expected Outcome**:
- Products arranged Z to A
- Sort dropdown shows "Name (Z to A)"

**Severity**: Normal
**Tags**: Regression, Sorting

---

### Workflow: Add Product And Checkout

**User Story**: User adds product and proceeds to checkout flow

**Steps**:
1. Login and navigate to inventory
2. Add a product to cart
3. Click cart icon
4. Verify product in cart
5. Click checkout button
6. Verify checkout form appears

**Test Data**:
- Product: "Sauce Labs Backpack"

**Expected Outcome**:
- Cart contains product
- Checkout page loads with form fields
- URL is checkout-step-one.html

**Severity**: Critical
**Tags**: Smoke, Checkout

---

## Reporting

**Allure Steps**: Each workflow step should be wrapped in `AllureApi.Step()` for granular reporting

---

## Coverage

**What's covered**:
- Product browsing
- Cart additions/removals
- Badge updates
- Navigation between pages
- Sorting functionality
- Checkout initiation

**Scenarios Tested**:
- ✅ View product catalog
- ✅ Add single product
- ✅ Add multiple products
- ✅ Remove products
- ✅ Product details navigation
- ✅ Sorting options
- ✅ Checkout flow start

---

## Notes

**External Dependency**: Tests require SauceDemo to be online
**Test Data**: Uses SauceDemo's standard product catalog
