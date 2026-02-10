# Checkout - End-to-End Workflow Specification

## Test Suite Overview

**Test Class**: CheckoutWorkflowTests
**What We're Testing**: Complete checkout workflows from end to end
**Test Type**: End-to-End Tests
**Allure Suite**: "End-to-End Tests"
**Allure Feature**: "Checkout"

### Purpose
Test complete user journeys through the entire checkout process from cart to order confirmation. These verify the full purchase flow works correctly.

---

## Test Configuration

**Browser**: Real Playwright browser (Chromium, headless)
**Fixture**: PlaywrightFixture
**Target**: https://www.saucedemo.com/
**Test Users**: standard_user / secret_sauce

---

## Workflow Scenarios

### Workflow: Complete Checkout With Single Item

**User Story**: User completes full checkout process with one product

**Steps**:
1. Login and add product to cart
2. Navigate to cart and click checkout
3. Fill customer information
4. Click continue to review order
5. Verify order summary is correct
6. Click finish to complete order
7. Verify confirmation message appears

**Test Data**:
- Product: "Sauce Labs Backpack"
- Customer: "John", "Doe", "12345"

**Expected Outcome**:
- Order completes successfully
- Confirmation: "Thank you for your order!"
- URL: checkout-complete.html

**Severity**: Critical
**Tags**: Smoke, Checkout

---

### Workflow: Complete Checkout With Multiple Items

**User Story**: User completes checkout with multiple products in cart

**Steps**:
1. Login and add three products to cart
2. Navigate to cart and verify all items
3. Proceed to checkout
4. Fill customer information
5. Review order summary with multiple items
6. Verify total price is correct
7. Complete order
8. Verify confirmation

**Test Data**:
- Products: "Sauce Labs Backpack", "Sauce Labs Bike Light", "Sauce Labs Bolt T-Shirt"
- Customer: "Jane", "Smith", "67890"

**Expected Outcome**:
- All three items in order summary
- Total price = sum of all items + tax
- Order confirmation displayed

**Severity**: Critical
**Tags**: Smoke, Checkout

---

### Workflow: Cancel Checkout From Step One

**User Story**: User cancels checkout and returns to cart

**Steps**:
1. Login, add product, navigate to checkout
2. Start filling information
3. Click cancel button
4. Verify returned to cart page
5. Verify product still in cart

**Expected Outcome**:
- URL returns to cart.html
- Cart contents preserved
- Can resume checkout later

**Severity**: Normal
**Tags**: Regression, Checkout

---

### Workflow: Validation Error For Missing First Name

**User Story**: User sees error when first name is missing

**Steps**:
1. Login, add product, navigate to checkout
2. Fill only lastName and postalCode
3. Click continue
4. Verify error message appears
5. Verify still on step one

**Test Data**:
- firstName: (empty)
- lastName: "Doe"
- postalCode: "12345"

**Expected Outcome**:
- Error: "Error: First Name is required" or similar
- Remains on checkout-step-one.html

**Severity**: Critical
**Tags**: Smoke, Validation, Negative

---

### Workflow: Validation Error For Missing Last Name

**User Story**: User sees error when last name is missing

**Steps**:
1. Login, add product, navigate to checkout
2. Fill only firstName and postalCode
3. Click continue
4. Verify error message appears

**Test Data**:
- firstName: "John"
- lastName: (empty)
- postalCode: "12345"

**Expected Outcome**:
- Error indicates last name required
- Form not submitted

**Severity**: Normal
**Tags**: Regression, Validation, Negative

---

### Workflow: Validation Error For Missing Postal Code

**User Story**: User sees error when postal code is missing

**Steps**:
1. Login, add product, navigate to checkout
2. Fill only firstName and lastName
3. Click continue
4. Verify error message appears

**Test Data**:
- firstName: "John"
- lastName: "Doe"
- postalCode: (empty)

**Expected Outcome**:
- Error indicates postal code required
- Form not submitted

**Severity**: Normal
**Tags**: Regression, Validation, Negative

---

### Workflow: Verify Order Summary Accuracy

**User Story**: User verifies order details before completing purchase

**Steps**:
1. Login, add two products with known prices
2. Complete checkout step one
3. Review step two order summary
4. Verify subtotal matches item prices
5. Verify tax is calculated
6. Verify total equals subtotal plus tax
7. Verify all product names and prices displayed

**Test Data**:
- Products: "Sauce Labs Backpack" ($29.99), "Sauce Labs Bike Light" ($9.99)

**Expected Outcome**:
- Subtotal: $39.98
- Tax: calculated amount
- Total: $39.98 + tax
- Both products listed

**Severity**: Critical
**Tags**: Smoke, Checkout

---

### Workflow: Return To Inventory After Order Completion

**User Story**: User returns to shopping after successful order

**Steps**:
1. Login, add product, complete full checkout
2. Verify confirmation page
3. Click "Back to Products" button
4. Verify returned to inventory page
5. Verify cart is empty

**Expected Outcome**:
- URL: inventory.html
- Cart badge not visible
- Can start new shopping session

**Severity**: Normal
**Tags**: Regression, Navigation

---

### Workflow: Complete Multiple Checkouts In Same Session

**User Story**: User completes multiple orders in one session

**Steps**:
1. Login, add product, complete checkout
2. Click back to products
3. Add different product to cart
4. Complete second checkout
5. Verify second confirmation

**Expected Outcome**:
- Both orders complete successfully
- Session remains active
- Each order independent

**Severity**: Normal
**Tags**: Regression, Checkout

---

## Reporting

**Allure Steps**: Each workflow step should be wrapped in `AllureApi.Step()` for granular reporting

---

## Coverage

**What's covered**:
- Complete checkout flow (all steps)
- Single and multiple item orders
- Form validation
- Order summary accuracy
- Cancellation
- Post-order navigation
- Multiple orders per session

**Scenarios Tested**:
- ✅ Single item checkout
- ✅ Multiple item checkout
- ✅ Checkout cancellation
- ✅ Missing first name
- ✅ Missing last name
- ✅ Missing postal code
- ✅ Order summary verification
- ✅ Return to inventory
- ✅ Multiple checkouts

---

## Notes

**External Dependency**: Tests require SauceDemo to be online
**Test Data**: Uses SauceDemo's standard product catalog and checkout flow
