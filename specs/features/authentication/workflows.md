# Authentication Workflow Tests Specification

## Test Suite Overview

**Test Class**: UserWorkflowTests
**What We're Testing**: Complete authentication workflows from end to end
**Test Type**: End-to-End Tests
**Allure Suite**: "End-to-End Tests"
**Allure Feature**: "Authentication"

### Purpose
Test complete user journeys involving authentication. These tests verify the entire flow from login page through to logged-in features and logout.

---

## Test Configuration

**Browser**: Real Playwright browser (Chromium, headless)
**Fixture**: PlaywrightFixture
**Target**: https://www.saucedemo.com/
**Test Users**: SauceDemo test accounts

---

## Workflow Scenarios

### Workflow 1: Successful Login

**User Story**: User logs in with valid credentials and sees product inventory

**Steps**:
1. Navigate to login page
2. Enter valid credentials and login
3. Verify inventory list is visible

**Test Data**:
- Username: "standard_user"
- Password: "secret_sauce"

**Expected Outcome**:
- `.inventory_list` is visible

**Severity**: Critical
**Tags**: Smoke, Login

---

### Workflow 2: Navigate To Inventory After Login

**User Story**: After successful login, user is redirected to inventory page

**Steps**:
1. Navigate to login page
2. Login with valid credentials
3. Verify URL changed to inventory page
4. Verify inventory container is visible

**Test Data**:
- Username: "standard_user"
- Password: "secret_sauce"

**Expected Outcomes**:
- URL: "https://www.saucedemo.com/inventory.html"
- `.inventory_container` is visible

**Severity**: Critical
**Tags**: Smoke, Navigation

---

### Workflow 3: Logout After Login

**User Story**: User logs in, then logs out and returns to login page

**Steps**:
1. Navigate to login page
2. Login with valid credentials
3. Open burger menu
4. Click logout link
5. Verify back on login page
6. Verify login button is visible

**Test Data**:
- Username: "standard_user"
- Password: "secret_sauce"

**Expected Outcomes**:
- URL returns to "https://www.saucedemo.com/"
- Login button `[data-test='login-button']` is visible

**Severity**: Normal
**Tags**: Regression, Logout

---

### Workflow 4: Failed Login - Invalid Credentials

**User Story**: User attempts login with wrong username/password and sees error

**Steps**:
1. Navigate to login page
2. Attempt login with invalid credentials
3. Verify error message is displayed

**Test Data**:
- Username: "invalid_user"
- Password: "wrong_password"

**Expected Outcome**:
- Error message: "Epic sadface: Username and password do not match any user in this service"

**Severity**: Critical
**Tags**: Smoke, Validation, Negative

---

### Workflow 5: Failed Login - Empty Credentials

**User Story**: User clicks login without entering credentials and sees error

**Steps**:
1. Navigate to login page
2. Click login button without entering anything
3. Verify username required error appears

**Expected Outcome**:
- Error message: "Epic sadface: Username is required"

**Severity**: Normal
**Tags**: Regression, Validation, Negative

---

### Workflow 6: Failed Login - Locked Out User

**User Story**: User attempts login with locked out account and sees error

**Steps**:
1. Navigate to login page
2. Attempt login with locked_out_user
3. Verify locked out error appears

**Test Data**:
- Username: "locked_out_user"
- Password: "secret_sauce"

**Expected Outcome**:
- Error message: "Epic sadface: Sorry, this user has been locked out."

**Severity**: Critical
**Tags**: Smoke, Validation, Negative

---

## Reporting

**Allure Steps**: Each workflow step should be wrapped in `AllureApi.Step()` for granular reporting

---

## Coverage

**What's covered**:
- Complete login-to-inventory flow
- Login with validation
- Logout flow
- Error scenarios (invalid, empty, locked)

**Scenarios Tested**:
- ✅ Happy path login
- ✅ Navigation after login
- ✅ Full logout cycle
- ✅ Invalid credentials
- ✅ Empty credentials
- ✅ Locked user

---

## Notes

**External Dependency**: These tests require SauceDemo to be online
**Test Data**: Uses SauceDemo's built-in test accounts
