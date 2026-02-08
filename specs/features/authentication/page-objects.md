# Login Page Object Specification

## Page Overview

**Page Name**: LoginPage
**URL**: https://www.saucedemo.com/
**Purpose**: Handles navigation to the login page and submitting login credentials
**Namespace**: csharp_framework_demo.Utilities.PageObjects

### What It Does
Wraps the SauceDemo login page so tests can interact with it without dealing with selectors directly. Provides methods for navigating to the page and logging in.

---

## Page Elements

| Element | Selector | Type | Purpose |
|---------|----------|------|---------|
| Username Field | `[data-test='username']` | Input | Enter username |
| Password Field | `[data-test='password']` | Input | Enter password |
| Login Button | `[data-test='login-button']` | Button | Submit login |

### Selector Strategy
Use `data-test` attributes when available (they're stable and meant for testing)

---

## Methods

### NavigateToAsync

**Signature**: `Task NavigateToAsync(string url)`

**What it does**: Navigates the browser to the specified URL

**Parameters**:
- `url`: The URL to navigate to

**Behavior**: Uses Playwright's GotoAsync to navigate

---

### LoginAsync

**Signature**: `Task LoginAsync(string username, string password)`

**What it does**: Fills username and password fields, then clicks login button

**Parameters**:
- `username`: Username to enter
- `password`: Password to enter

**Behavior**:
1. Fill username field
2. Fill password field
3. Click login button

**After login**:
- Valid credentials → Redirects to inventory page
- Invalid credentials → Error message appears

---

## Implementation Rules

**Constructor**:
- Must accept `IPage` parameter
- Store in private readonly field

**Methods**:
- All async (return Task)
- All Playwright calls use await
- No assertions (Page Object performs actions, tests do assertions)

---

## File Location

- Path: `src/Utilities/PageObjects/LoginPage.cs`
- Namespace: `csharp_framework_demo.Utilities.PageObjects`
- Class Name: `LoginPage`
