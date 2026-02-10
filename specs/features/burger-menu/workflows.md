# Burger Menu - End-to-End Workflow Specification

## Test Suite Overview

**Test Class**: BurgerMenuWorkflowTests
**What We're Testing**: Complete burger menu workflows from end to end
**Test Type**: End-to-End Tests
**Allure Suite**: "End-to-End Tests"
**Allure Feature**: "Burger Menu"

### Purpose
Test complete user journeys involving burger menu navigation, logout, and app state reset. These verify the entire menu experience works correctly across different pages.

---

## Test Configuration

**Browser**: Real Playwright browser (Chromium, headless)
**Fixture**: PlaywrightFixture
**Target**: https://www.saucedemo.com/
**Test Users**: standard_user / secret_sauce

---

## Workflow Scenarios

### Workflow: Open And Close Burger Menu

**User Story**: User opens and closes the navigation menu

**Steps**:
1. Login and navigate to inventory
2. Click burger menu button
3. Verify menu slides open
4. Verify menu links are visible
5. Click close button
6. Verify menu slides closed

**Expected Outcome**:
- Menu animates open smoothly
- All menu links visible when open
- Menu closes when close button clicked
- Menu not visible when closed

**Severity**: Normal
**Tags**: Smoke, BurgerMenu

---

### Workflow: Navigate To All Items From Different Page

**User Story**: User uses menu to return to inventory from another page

**Steps**:
1. Login and navigate to cart page
2. Open burger menu
3. Click "All Items" link
4. Verify redirected to inventory page
5. Verify inventory products are displayed

**Expected Outcome**:
- URL: inventory.html
- Inventory list visible
- Menu closes after navigation

**Severity**: Normal
**Tags**: Regression, Navigation

---

### Workflow: Navigate To About Page

**User Story**: User accesses Sauce Labs information via menu

**Steps**:
1. Login and navigate to inventory
2. Open burger menu
3. Click "About" link
4. Verify redirected to Sauce Labs website
5. Verify external site loads

**Expected Outcome**:
- URL changes to saucelabs.com
- External page loads
- New context or tab may open

**Severity**: Normal
**Tags**: Regression, Navigation

---

### Workflow: Complete Logout Flow

**User Story**: User logs out using burger menu

**Steps**:
1. Login with valid credentials
2. Navigate to inventory
3. Add items to cart (to verify session cleared)
4. Open burger menu
5. Click "Logout" link
6. Verify redirected to login page
7. Verify login form is displayed
8. Verify cannot access inventory without re-login

**Expected Outcome**:
- URL returns to login page
- Login button visible
- Session terminated
- Protected pages inaccessible

**Severity**: Critical
**Tags**: Smoke, Logout

---

### Workflow: Reset Application State

**User Story**: User resets cart and app state via menu

**Steps**:
1. Login and navigate to inventory
2. Add three products to cart
3. Verify cart badge shows "3"
4. Open burger menu
5. Click "Reset App State" link
6. Close menu
7. Verify cart badge is cleared
8. Navigate to cart
9. Verify cart is empty

**Expected Outcome**:
- Cart badge removed
- Cart contains no items
- App state reset to initial

**Severity**: Normal
**Tags**: Regression, BurgerMenu

---

### Workflow: Menu Available Across All Pages

**User Story**: User can access menu from any page after login

**Steps**:
1. Login and navigate to inventory
2. Verify menu button visible
3. Navigate to product details
4. Verify menu button visible
5. Navigate to cart
6. Verify menu button visible
7. Navigate to checkout
8. Verify menu button visible
9. Open menu from checkout
10. Verify menu functions correctly

**Expected Outcome**:
- Menu button visible on all pages
- Menu opens consistently
- Menu links work from any page

**Severity**: Normal
**Tags**: Regression, BurgerMenu

---

### Workflow: Logout From Different Pages

**User Story**: User can logout from any page in the application

**Steps**:
1. Login and navigate to cart page
2. Open menu and logout
3. Verify returned to login
4. Login again and navigate to checkout
5. Open menu and logout
6. Verify returned to login

**Expected Outcome**:
- Logout works from any page
- Always returns to login page
- Session cleared properly

**Severity**: Critical
**Tags**: Smoke, Logout

---

### Workflow: Menu Closes After Navigation

**User Story**: Menu automatically closes after user clicks a navigation link

**Steps**:
1. Login and navigate to inventory
2. Open burger menu
3. Click "All Items" link
4. Verify navigation occurs
5. Verify menu is closed after navigation

**Expected Outcome**:
- Menu closes after link click
- User not stuck with open menu
- Clean UI state after navigation

**Severity**: Normal
**Tags**: Regression, BurgerMenu

---

### Workflow: Reset Cart During Shopping

**User Story**: User resets cart in middle of shopping session

**Steps**:
1. Login and add products to cart
2. Navigate to cart and verify items
3. Return to inventory
4. Open menu and reset app state
5. Verify cart cleared
6. Add new products to cart
7. Verify new items appear
8. Complete checkout with new items

**Expected Outcome**:
- Old cart items cleared
- Can add new items after reset
- Checkout works with new items

**Severity**: Normal
**Tags**: Regression, BurgerMenu, Cart

---

## Reporting

**Allure Steps**: Each workflow step should be wrapped in `AllureApi.Step()` for granular reporting

---

## Coverage

**What's covered**:
- Menu open/close
- Navigation through menu
- Logout from various pages
- App state reset
- Menu availability across pages
- Menu behavior consistency

**Scenarios Tested**:
- ✅ Open and close menu
- ✅ Navigate to All Items
- ✅ Navigate to About
- ✅ Complete logout
- ✅ Reset app state
- ✅ Menu on all pages
- ✅ Logout from any page
- ✅ Auto-close after navigation
- ✅ Reset during shopping

---

## Notes

**External Dependency**: Tests require SauceDemo to be online
**Test Data**: Uses SauceDemo's standard navigation structure
**External Navigation**: About link navigates to external site (saucelabs.com)
