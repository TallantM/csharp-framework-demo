# Product Details - End-to-End Workflow Specification

## Test Suite Overview

**Test Class**: ProductDetailsWorkflowTests
**What We're Testing**: Complete product detail page workflows from end to end
**Test Type**: End-to-End Tests
**Allure Suite**: "End-to-End Tests"
**Allure Feature**: "Product Details"

### Purpose
Test complete user journeys involving product detail viewing, cart management from detail page, and navigation flows. These verify the entire product detail experience works correctly.

---

## Test Configuration

**Browser**: Real Playwright browser (Chromium, headless)
**Fixture**: PlaywrightFixture
**Target**: https://www.saucedemo.com/
**Test Users**: standard_user / secret_sauce

---

## Workflow Scenarios

### Workflow: Navigate To Product Details From Inventory

**User Story**: User clicks product name in inventory to view full details

**Steps**:
1. Login and navigate to inventory
2. Click on a product name
3. Verify redirected to product detail page
4. Verify product name, description, price are displayed
5. Verify image is visible
6. Verify Add to Cart button is present

**Test Data**:
- Product: "Sauce Labs Backpack"

**Expected Outcome**:
- URL contains "inventory-item.html?id="
- Product name: "Sauce Labs Backpack"
- Price: "$29.99"
- Description visible
- Image visible

**Severity**: Critical
**Tags**: Smoke, ProductDetails

---

### Workflow: Add Product To Cart From Details Page

**User Story**: User adds product to cart while viewing details

**Steps**:
1. Login and navigate to product detail page
2. Verify cart badge not visible initially
3. Click Add to Cart button
4. Verify button changes to Remove
5. Verify cart badge shows "1"
6. Navigate to cart
7. Verify product appears in cart

**Test Data**:
- Product: "Sauce Labs Backpack"

**Expected Outcome**:
- Button changes to "Remove"
- Cart badge displays "1"
- Product visible in cart page

**Severity**: Critical
**Tags**: Smoke, Cart

---

### Workflow: Remove Product From Cart On Details Page

**User Story**: User removes product from cart while on details page

**Steps**:
1. Login, navigate to product detail, add to cart
2. Verify Remove button visible
3. Click Remove button
4. Verify button changes back to Add to Cart
5. Verify cart badge disappears
6. Navigate to cart
7. Verify product not in cart

**Test Data**:
- Product: "Sauce Labs Backpack"

**Expected Outcome**:
- Button changes to "Add to Cart"
- Cart badge not visible
- Cart is empty

**Severity**: Normal
**Tags**: Regression, Cart

---

### Workflow: Return To Inventory From Details Page

**User Story**: User clicks Back to Products button to return to catalog

**Steps**:
1. Login and navigate to product detail page
2. Click "Back to Products" button
3. Verify redirected to inventory page
4. Verify inventory list is visible
5. Verify can browse other products

**Expected Outcome**:
- URL: inventory.html
- Inventory list displayed
- All products visible

**Severity**: Normal
**Tags**: Regression, Navigation

---

### Workflow: View Multiple Product Details

**User Story**: User browses multiple products by viewing their details

**Steps**:
1. Login and navigate to inventory
2. Click first product name
3. Verify first product details
4. Click back to products
5. Click second product name
6. Verify second product details
7. Verify correct information for each product

**Test Data**:
- Products: "Sauce Labs Backpack", "Sauce Labs Bike Light"

**Expected Outcome**:
- Each product shows unique name, price, description
- Navigation works correctly between products
- Details match inventory information

**Severity**: Normal
**Tags**: Regression, ProductDetails

---

### Workflow: Add Multiple Products From Details Pages

**User Story**: User adds multiple products by visiting each detail page

**Steps**:
1. Login and navigate to first product detail
2. Add to cart and verify badge shows "1"
3. Return to inventory
4. Navigate to second product detail
5. Add to cart and verify badge shows "2"
6. Navigate to cart
7. Verify both products are in cart

**Test Data**:
- Products: "Sauce Labs Backpack", "Sauce Labs Bike Light"

**Expected Outcome**:
- Cart badge increments correctly
- Both products visible in cart
- Correct names and prices displayed

**Severity**: Critical
**Tags**: Smoke, Cart

---

### Workflow: Navigate To Cart From Details Page

**User Story**: User clicks cart icon from product detail page

**Steps**:
1. Login, navigate to product detail, add item
2. Click cart icon
3. Verify redirected to cart page
4. Verify product is in cart

**Test Data**:
- Product: "Sauce Labs Backpack"

**Expected Outcome**:
- URL: cart.html
- Product visible in cart

**Severity**: Normal
**Tags**: Regression, Navigation

---

### Workflow: Verify Product Information Consistency

**User Story**: Product details match what was shown in inventory

**Steps**:
1. Login and navigate to inventory
2. Note product name and price from inventory card
3. Click product to view details
4. Verify name matches inventory
5. Verify price matches inventory

**Test Data**:
- Product: "Sauce Labs Backpack"

**Expected Outcome**:
- Name consistent across pages
- Price consistent across pages
- Information matches inventory display

**Severity**: Normal
**Tags**: Regression, ProductDetails

---

## Reporting

**Allure Steps**: Each workflow step should be wrapped in `AllureApi.Step()` for granular reporting

---

## Coverage

**What's covered**:
- Product detail navigation
- Cart operations from details page
- Back navigation
- Multi-product browsing
- Cart badge updates
- Information consistency
- Cross-page navigation

**Scenarios Tested**:
- ✅ Navigate to product details
- ✅ Add from details page
- ✅ Remove from details page
- ✅ Return to inventory
- ✅ View multiple products
- ✅ Add multiple products
- ✅ Navigate to cart
- ✅ Information consistency

---

## Notes

**External Dependency**: Tests require SauceDemo to be online
**Test Data**: Uses SauceDemo's standard product catalog
