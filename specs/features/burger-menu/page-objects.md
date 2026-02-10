# Burger Menu Page Object Specification

## Page Overview

**Page Name**: BurgerMenuPage
**URL**: Available on all pages after login (https://www.saucedemo.com/*)
**Purpose**: Access navigation menu for logout, reset state, and additional links
**Namespace**: csharp_framework_demo.Utilities.PageObjects

### What It Does
Wraps the SauceDemo hamburger menu component that provides navigation options including logout, reset app state, and informational links. Available across all authenticated pages.

---

## Page Elements

| Element | Selector | Type | Purpose |
|---------|----------|------|---------|
| Menu Button | `#react-burger-menu-btn` | Button | Opens the burger menu |
| Menu Container | `.bm-menu` | Container | Side menu container |
| All Items Link | `#inventory_sidebar_link` | Link | Navigate to inventory page |
| About Link | `#about_sidebar_link` | Link | Navigate to Sauce Labs about page |
| Logout Link | `#logout_sidebar_link` | Link | Log out of application |
| Reset App Link | `#reset_sidebar_link` | Link | Reset application state |
| Close Button | `#react-burger-cross-btn` | Button | Closes the burger menu |

### Selector Strategy
Use ID selectors for all menu elements as they are stable and unique.

---

## Methods

### OpenMenuAsync

**Signature**: `Task OpenMenuAsync()`

**What it does**: Opens the burger menu by clicking the menu button

**Behavior**: Clicks `#react-burger-menu-btn` element

---

### CloseMenuAsync

**Signature**: `Task CloseMenuAsync()`

**What it does**: Closes the burger menu by clicking the close button

**Behavior**: Clicks `#react-burger-cross-btn` element

---

### IsMenuOpenAsync

**Signature**: `Task<bool> IsMenuOpenAsync()`

**What it does**: Checks if the burger menu is currently open

**Returns**: True if menu is visible, false otherwise

**Behavior**: Checks visibility of `.bm-menu` element or verifies menu container state

---

### ClickAllItemsAsync

**Signature**: `Task ClickAllItemsAsync()`

**What it does**: Navigates to inventory page via menu link

**Behavior**: Clicks `#inventory_sidebar_link` element

---

### ClickAboutAsync

**Signature**: `Task ClickAboutAsync()`

**What it does**: Navigates to Sauce Labs about page

**Behavior**: Clicks `#about_sidebar_link` element

---

### ClickLogoutAsync

**Signature**: `Task ClickLogoutAsync()`

**What it does**: Logs out the current user

**Behavior**: Clicks `#logout_sidebar_link` element

---

### ClickResetAppAsync

**Signature**: `Task ClickResetAppAsync()`

**What it does**: Resets application state (clears cart, etc.)

**Behavior**: Clicks `#reset_sidebar_link` element

---

### LogoutAsync

**Signature**: `Task LogoutAsync()`

**What it does**: Convenience method to open menu and logout in one call

**Behavior**: Opens menu, then clicks logout link

---

### IsLogoutLinkVisibleAsync

**Signature**: `Task<bool> IsLogoutLinkVisibleAsync()`

**What it does**: Checks if logout link is visible in the menu

**Returns**: True if logout link is visible, false otherwise

**Behavior**: Checks visibility of `#logout_sidebar_link` element

---

## Implementation Rules

**Constructor**:
- Must accept `IPage` parameter
- Store in private readonly field

**Methods**:
- All async (return Task or Task<T>)
- All Playwright calls use await
- No assertions (Page Object performs actions, tests do assertions)

**Menu State**:
- Some methods may need to wait for menu to open before interacting with links
- Consider using Playwright's auto-waiting capabilities

---

## File Location

- Path: `src/Utilities/PageObjects/BurgerMenuPage.cs`
- Namespace: `csharp_framework_demo.Utilities.PageObjects`
- Class Name: `BurgerMenuPage`
