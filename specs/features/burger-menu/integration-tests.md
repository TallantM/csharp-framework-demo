# Burger Menu Page Integration Tests Specification

## Test Suite Overview

**Test Class**: BurgerMenuPageIntegrationTests
**What We're Testing**: BurgerMenuPage with real Playwright browser
**Test Type**: Integration Tests
**Allure Suite**: "Integration Tests"
**Allure Feature**: "Burger Menu Page Object"

### Purpose
Verify BurgerMenuPage works with a real browser and the actual SauceDemo website. Tests check that selectors are correct and menu interactions behave as expected.

---

## Test Configuration

**Browser**: Real Playwright browser (Chromium, headless)
**Fixture**: PlaywrightFixture
**Target**: https://www.saucedemo.com/inventory.html
**Prerequisites**: User must be logged in

---

## Test Scenarios

### Test: Open Menu Displays Menu Container

**What we verify**: OpenMenuAsync opens the burger menu and makes it visible

**Steps**:
1. Login and navigate to inventory
2. Call OpenMenuAsync
3. Verify menu container is visible
4. Verify menu links are visible

**Expected**:
- Menu container `.bm-menu` is visible
- Menu links are displayed

**Severity**: Critical
**Tags**: Integration, BurgerMenu

---

### Test: Close Menu Hides Menu Container

**What we verify**: CloseMenuAsync closes the burger menu

**Steps**:
1. Login and navigate to inventory
2. Open menu
3. Call CloseMenuAsync
4. Verify menu container is not visible

**Expected**:
- Menu container not visible or hidden

**Severity**: Normal
**Tags**: Integration, BurgerMenu

---

### Test: Is Menu Open Returns Correct State

**What we verify**: IsMenuOpenAsync correctly identifies menu open/closed state

**Steps**:
1. Login and navigate to inventory
2. Verify IsMenuOpenAsync returns false initially
3. Open menu
4. Verify IsMenuOpenAsync returns true
5. Close menu
6. Verify IsMenuOpenAsync returns false

**Expected**:
- Returns false when closed
- Returns true when open

**Severity**: Normal
**Tags**: Integration, BurgerMenu

---

### Test: Click All Items Navigates To Inventory

**What we verify**: ClickAllItemsAsync navigates to inventory page

**Steps**:
1. Login and navigate to any page
2. Open menu
3. Call ClickAllItemsAsync
4. Verify URL is inventory page

**Expected**:
- URL: "https://www.saucedemo.com/inventory.html"

**Severity**: Normal
**Tags**: Integration, Navigation

---

### Test: Click About Navigates To External Site

**What we verify**: ClickAboutAsync navigates to Sauce Labs website

**Steps**:
1. Login and navigate to inventory
2. Open menu
3. Call ClickAboutAsync
4. Verify URL changes to Sauce Labs about page

**Expected**:
- URL contains "saucelabs.com"

**Severity**: Normal
**Tags**: Integration, Navigation

---

### Test: Click Logout Returns To Login Page

**What we verify**: ClickLogoutAsync logs out user and returns to login

**Steps**:
1. Login and navigate to inventory
2. Open menu
3. Call ClickLogoutAsync
4. Verify URL is login page
5. Verify login button is visible

**Expected**:
- URL: "https://www.saucedemo.com/"
- Login button visible

**Severity**: Critical
**Tags**: Integration, Logout

---

### Test: Click Reset App Clears Cart

**What we verify**: ClickResetAppAsync resets application state including cart

**Steps**:
1. Login, add items to cart, navigate to inventory
2. Verify cart badge shows item count
3. Open menu
4. Call ClickResetAppAsync
5. Close menu
6. Verify cart badge is cleared

**Expected**:
- Cart badge not visible after reset
- Application state reset

**Severity**: Normal
**Tags**: Integration, BurgerMenu

---

### Test: Logout Convenience Method Works End To End

**What we verify**: LogoutAsync opens menu and logs out in one call

**Steps**:
1. Login and navigate to inventory
2. Call LogoutAsync
3. Verify redirected to login page
4. Verify logged out successfully

**Expected**:
- URL: login page
- Menu opened and logout performed
- User logged out

**Severity**: Critical
**Tags**: Integration, Logout

---

### Test: Is Logout Link Visible After Opening Menu

**What we verify**: IsLogoutLinkVisibleAsync checks logout link visibility

**Steps**:
1. Login and navigate to inventory
2. Verify IsLogoutLinkVisibleAsync returns false (menu closed)
3. Open menu
4. Verify IsLogoutLinkVisibleAsync returns true

**Expected**:
- Returns false when menu closed
- Returns true when menu open

**Severity**: Normal
**Tags**: Integration, BurgerMenu

---

## Coverage

**What's covered**:
- Page Object + Playwright integration
- Correct selectors
- Menu open/close operations
- Navigation through menu links
- Logout functionality
- Reset app state
- Menu state verification

**What's NOT covered**:
- Complete user workflows (E2E tests handle that)
