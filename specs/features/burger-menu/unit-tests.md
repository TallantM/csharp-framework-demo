# Burger Menu Page Object - Unit Tests Specification

## Test Suite Overview

**Test Class**: BurgerMenuPageUnitTests
**What We're Testing**: BurgerMenuPage methods in isolation
**Test Type**: Unit Tests
**Allure Suite**: "Unit Tests"
**Allure Feature**: "Burger Menu Page Object"

### Purpose
Verify that BurgerMenuPage methods call the correct Playwright methods with the correct parameters. Uses mocks instead of real browser for fast execution.

---

## Test Configuration

**Mocking**: Use Moq to mock `IPage`
**Test Framework**: xUnit
**Reporting**: Allure with step descriptions

### Mocking Strategy
Create mock IPage, pass to Page Object, call method, verify mock received expected calls

---

## Test Scenarios

### Test: OpenMenuAsync Clicks Menu Button

**What we verify**: OpenMenuAsync should click the burger menu button

**Expected Behavior**:
- ClickAsync called with `#react-burger-menu-btn`
- Called exactly once

**Severity**: Critical
**Tags**: Unit, BurgerMenu

---

### Test: CloseMenuAsync Clicks Close Button

**What we verify**: CloseMenuAsync should click the menu close button

**Expected Behavior**:
- ClickAsync called with `#react-burger-cross-btn`
- Called exactly once

**Severity**: Normal
**Tags**: Unit, BurgerMenu

---

### Test: IsMenuOpenAsync Checks Menu Visibility

**What we verify**: IsMenuOpenAsync should check if menu container is visible

**Expected Behavior**:
- IsVisibleAsync called with `.bm-menu` or similar selector
- Returns boolean result

**Severity**: Normal
**Tags**: Unit, BurgerMenu

---

### Test: ClickAllItemsAsync Clicks All Items Link

**What we verify**: ClickAllItemsAsync should click the all items menu link

**Expected Behavior**:
- ClickAsync called with `#inventory_sidebar_link`
- Called exactly once

**Severity**: Normal
**Tags**: Unit, Navigation

---

### Test: ClickAboutAsync Clicks About Link

**What we verify**: ClickAboutAsync should click the about menu link

**Expected Behavior**:
- ClickAsync called with `#about_sidebar_link`
- Called exactly once

**Severity**: Normal
**Tags**: Unit, Navigation

---

### Test: ClickLogoutAsync Clicks Logout Link

**What we verify**: ClickLogoutAsync should click the logout menu link

**Expected Behavior**:
- ClickAsync called with `#logout_sidebar_link`
- Called exactly once

**Severity**: Critical
**Tags**: Unit, Logout

---

### Test: ClickResetAppAsync Clicks Reset Link

**What we verify**: ClickResetAppAsync should click the reset app state link

**Expected Behavior**:
- ClickAsync called with `#reset_sidebar_link`
- Called exactly once

**Severity**: Normal
**Tags**: Unit, BurgerMenu

---

### Test: LogoutAsync Opens Menu And Clicks Logout

**What we verify**: LogoutAsync should open menu then click logout link

**Expected Behavior**:
- ClickAsync called with `#react-burger-menu-btn`
- ClickAsync called with `#logout_sidebar_link`
- Both called exactly once in correct order

**Severity**: Critical
**Tags**: Unit, Logout

---

### Test: IsLogoutLinkVisibleAsync Checks Logout Link Visibility

**What we verify**: IsLogoutLinkVisibleAsync should check if logout link is visible

**Expected Behavior**:
- IsVisibleAsync called with `#logout_sidebar_link`
- Returns boolean result

**Severity**: Normal
**Tags**: Unit, BurgerMenu

---

## Expected Outcomes

**When tests pass**: Confirms Page Object correctly delegates to Playwright
**When tests fail**: Indicates wrong selectors, wrong methods, or wrong parameters

---

## Coverage

**What's covered**:
- Method delegation
- Selector usage
- Menu opening/closing
- Link clicks
- Visibility checks
- Multi-step operations

**What's NOT covered**:
- Actual browser behavior (that's integration tests)
