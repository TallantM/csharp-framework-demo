# Cart - End-to-End Workflow Specification

## Test Suite Overview

**Test Class**: CartWorkflowTests
**What We're Testing**: Complete shopping cart workflows from end to end
**Test Type**: End-to-End Tests
**Allure Suite**: "End-to-End Tests"
**Allure Feature**: "Cart"

### Purpose
Test complete user journeys involving cart review, item management, and checkout initiation. These verify the entire cart flow works correctly.

---

## Test Configuration

**Browser**: Real Playwright browser (Chromium, headless)
**Fixture**: PlaywrightFixture
**Target**: https://www.saucedemo.com/
**Test Users**: standard_user / secret_sauce

---

## Workflow Scenarios

### Workflow: View Cart After Adding Products

**User Story**: User adds products and views them in the cart

**Steps**:
1. Login and navigate to inventory
2. Add two products to cart
3. Click cart icon
4. Verify both products appear in cart
5. Verify product details are correct (name, price)

**Test Data**:
- Products: "Sauce Labs Backpack", "Sauce Labs Bike Light"

**Expected Outcome**:
- Cart displays both products
- Names and prices match inventory
- Cart count shows "2"

**Severity**: Critical
**Tags**: Smoke, Cart

---

### Workflow: Remove Single Item From Cart

**User Story**: User removes one item from cart and cart updates correctly

**Steps**:
1. Login, add two products, navigate to cart
2. Remove one product using Remove button
3. Verify removed product disappears
4. Verify remaining product still visible
5. Verify cart badge updates to "1"

**Test Data**:
- Add: "Sauce Labs Backpack", "Sauce Labs Bike Light"
- Remove: "Sauce Labs Backpack"

**Expected Outcome**:
- Only Bike Light remains in cart
- Cart badge shows "1"
- Backpack not visible in cart

**Severity**: Critical
**Tags**: Smoke, Cart

---

### Workflow: Remove All Items From Cart

**User Story**: User empties cart by removing all items

**Steps**:
1. Login, add two products, navigate to cart
2. Remove first product
3. Remove second product
4. Verify cart is empty
5. Verify cart badge disappears

**Expected Outcome**:
- No products visible in cart
- Cart badge not displayed
- Cart shows empty state

**Severity**: Normal
**Tags**: Regression, Cart

---

### Workflow: Continue Shopping From Cart

**User Story**: User returns to inventory from cart to add more items

**Steps**:
1. Login, add product, navigate to cart
2. Click "Continue Shopping" button
3. Verify redirected to inventory page
4. Verify cart badge still shows item count
5. Add another product
6. Verify cart badge increments

**Expected Outcome**:
- Navigates to inventory.html
- Previous cart items preserved
- Can continue shopping

**Severity**: Normal
**Tags**: Regression, Navigation

---

### Workflow: Proceed To Checkout From Cart

**User Story**: User with items in cart proceeds to checkout

**Steps**:
1. Login, add product, navigate to cart
2. Click "Checkout" button
3. Verify redirected to checkout form
4. Verify form fields are visible

**Test Data**:
- Product: "Sauce Labs Backpack"

**Expected Outcome**:
- URL: checkout-step-one.html
- Form has firstName, lastName, postalCode fields
- Continue button visible

**Severity**: Critical
**Tags**: Smoke, Checkout

---

### Workflow: Navigate To Product Details From Cart

**User Story**: User clicks product name in cart to view details

**Steps**:
1. Login, add product, navigate to cart
2. Click product name link
3. Verify redirected to product detail page
4. Verify product information displayed
5. Navigate back to cart

**Test Data**:
- Product: "Sauce Labs Backpack"

**Expected Outcome**:
- URL contains inventory-item.html
- Product details visible
- Back button returns to cart

**Severity**: Normal
**Tags**: Regression, Navigation

---

### Workflow: Verify Cart Persistence Across Navigation

**User Story**: Cart contents persist when navigating between pages

**Steps**:
1. Login, add product, navigate to cart
2. Verify product in cart
3. Click "Continue Shopping"
4. Navigate to product details
5. Return to cart
6. Verify product still in cart

**Test Data**:
- Product: "Sauce Labs Backpack"

**Expected Outcome**:
- Product remains in cart throughout navigation
- Cart badge consistent across pages
- Cart state preserved

**Severity**: Normal
**Tags**: Regression, Cart

---

### Workflow: Attempt Checkout With Empty Cart

**User Story**: User attempts to checkout without items

**Steps**:
1. Login and navigate to cart with no items
2. Verify checkout button state
3. Verify empty cart message or indication

**Expected Outcome**:
- Cart shows empty state
- No items displayed
- Appropriate messaging shown

**Severity**: Normal
**Tags**: Regression, Validation, Negative

---

## Reporting

**Allure Steps**: Each workflow step should be wrapped in `AllureApi.Step()` for granular reporting

---

## Coverage

**What's covered**:
- Cart viewing
- Item removal
- Cart state management
- Continue shopping flow
- Checkout initiation
- Navigation preservation
- Empty cart handling

**Scenarios Tested**:
- ✅ View cart contents
- ✅ Remove single item
- ✅ Remove all items
- ✅ Continue shopping
- ✅ Proceed to checkout
- ✅ Navigate to product details
- ✅ Cart persistence
- ✅ Empty cart state

---

## Notes

**External Dependency**: Tests require SauceDemo to be online
**Test Data**: Uses SauceDemo's standard product catalog
