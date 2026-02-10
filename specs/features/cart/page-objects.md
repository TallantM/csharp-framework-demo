# Cart Page Object Specification

## Page Overview

**Page Name**: CartPage
**URL**: https://www.saucedemo.com/cart.html
**Purpose**: Review items in cart, update quantities, remove items, and proceed to checkout
**Namespace**: csharp_framework_demo.Utilities.PageObjects

### What It Does
Wraps the SauceDemo shopping cart page where users review selected products, manage cart contents, and initiate the checkout process.

---

## Page Elements

| Element | Selector | Type | Purpose |
|---------|----------|------|---------|
| Cart List | `.cart_list` | Container | Container for all cart items |
| Cart Item | `.cart_item` | Container | Individual product in cart |
| Item Name | `.inventory_item_name` | Link | Product name and link to details |
| Item Description | `.inventory_item_desc` | Text | Product description |
| Item Price | `.inventory_item_price` | Text | Product price |
| Remove Button | `[data-test='remove-{product-name}']` | Button | Remove specific product from cart |
| Continue Shopping | `[data-test='continue-shopping']` | Button | Return to inventory page |
| Checkout Button | `[data-test='checkout']` | Button | Proceed to checkout form |
| Cart Quantity | `.cart_quantity` | Text | Quantity label for each item |

### Selector Strategy
Use `data-test` attributes for action buttons. Class selectors for display elements.

---

## Methods

### GetCartItemCountAsync

**Signature**: `Task<int> GetCartItemCountAsync()`

**What it does**: Returns the number of items currently in the cart

**Returns**: Count of cart items

**Behavior**: Counts all elements matching `.cart_item` selector

---

### GetCartItemNamesAsync

**Signature**: `Task<List<string>> GetCartItemNamesAsync()`

**What it does**: Retrieves all product names from items in the cart

**Returns**: List of product name strings

**Behavior**: Extracts text content from all `.inventory_item_name` elements in cart

---

### RemoveItemAsync

**Signature**: `Task RemoveItemAsync(string productName)`

**What it does**: Removes a specific product from the cart

**Parameters**:
- `productName` (string): The product identifier in kebab-case format

**Behavior**: Clicks the button with selector `[data-test='remove-{productName}']`

---

### IsItemInCartAsync

**Signature**: `Task<bool> IsItemInCartAsync(string productName)`

**What it does**: Checks if a specific product is present in the cart

**Parameters**:
- `productName` (string): The visible product name to check

**Returns**: True if product name found in cart, false otherwise

**Behavior**: Searches for matching text in `.inventory_item_name` elements

---

### GetItemPriceAsync

**Signature**: `Task<string> GetItemPriceAsync(string productName)`

**What it does**: Gets the price for a specific product in the cart

**Parameters**:
- `productName` (string): The visible product name

**Returns**: Price string (e.g., "$29.99")

**Behavior**: Locates the cart item by name and returns its price element text

---

### ClickContinueShoppingAsync

**Signature**: `Task ClickContinueShoppingAsync()`

**What it does**: Navigates back to inventory page

**Behavior**: Clicks `[data-test='continue-shopping']` button

---

### ClickCheckoutAsync

**Signature**: `Task ClickCheckoutAsync()`

**What it does**: Proceeds to checkout form page

**Behavior**: Clicks `[data-test='checkout']` button

---

### IsCartEmptyAsync

**Signature**: `Task<bool> IsCartEmptyAsync()`

**What it does**: Checks if the cart has no items

**Returns**: True if cart is empty, false if items exist

**Behavior**: Checks if `.cart_item` elements exist or if cart_list is empty

---

### ClickProductNameAsync

**Signature**: `Task ClickProductNameAsync(string productName)`

**What it does**: Navigates to product details by clicking product name in cart

**Parameters**:
- `productName` (string): The visible product name to click

**Behavior**: Finds and clicks the matching `.inventory_item_name` link

---

## Implementation Rules

**Constructor**:
- Must accept `IPage` parameter
- Store in private readonly field

**Methods**:
- All async (return Task or Task<T>)
- All Playwright calls use await
- No assertions (Page Object performs actions, tests do assertions)

---

## File Location

- Path: `src/Utilities/PageObjects/CartPage.cs`
- Namespace: `csharp_framework_demo.Utilities.PageObjects`
- Class Name: `CartPage`
