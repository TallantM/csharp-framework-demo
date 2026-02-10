# Inventory Page Object Specification

## Page Overview

**Page Name**: InventoryPage
**URL**: https://www.saucedemo.com/inventory.html
**Purpose**: Browse product catalog, add/remove items from cart, and navigate to product details
**Namespace**: csharp_framework_demo.Utilities.PageObjects

### What It Does
Wraps the SauceDemo inventory/product listing page where users can view available products, manage cart items, and access sorting/filtering options.

---

## Page Elements

| Element | Selector | Type | Purpose |
|---------|----------|------|---------|
| Inventory Container | `.inventory_container` | Container | Main container for product list |
| Inventory List | `.inventory_list` | List | Container for all product items |
| Product Item | `.inventory_item` | Container | Individual product card |
| Product Name | `.inventory_item_name` | Link | Product title and link to details |
| Product Description | `.inventory_item_desc` | Text | Product description |
| Product Price | `.inventory_item_price` | Text | Product price |
| Add to Cart Button | `[data-test='add-to-cart-{product-name}']` | Button | Add specific product to cart |
| Remove Button | `[data-test='remove-{product-name}']` | Button | Remove specific product from cart |
| Cart Badge | `.shopping_cart_badge` | Badge | Shows number of items in cart |
| Cart Link | `.shopping_cart_link` | Link | Navigate to cart page |
| Sort Dropdown | `.product_sort_container` | Select | Sort products by name/price |

### Selector Strategy
Use `data-test` attributes for buttons when available. Class selectors for containers and display elements.

---

## Methods

### GetProductCountAsync

**Signature**: `Task<int> GetProductCountAsync()`

**What it does**: Returns the number of products displayed on the page

**Returns**: Count of product items in the inventory list

**Behavior**: Counts all elements matching `.inventory_item` selector

---

### GetProductNamesAsync

**Signature**: `Task<List<string>> GetProductNamesAsync()`

**What it does**: Retrieves all product names from the inventory page

**Returns**: List of product name strings

**Behavior**: Extracts text content from all `.inventory_item_name` elements

---

### AddToCartAsync

**Signature**: `Task AddToCartAsync(string productName)`

**What it does**: Adds a specific product to the shopping cart

**Parameters**:
- `productName` (string): The product identifier in kebab-case format

**Behavior**: Clicks the button with selector `[data-test='add-to-cart-{productName}']`

---

### RemoveFromCartAsync

**Signature**: `Task RemoveFromCartAsync(string productName)`

**What it does**: Removes a specific product from the shopping cart

**Parameters**:
- `productName` (string): The product identifier in kebab-case format

**Behavior**: Clicks the button with selector `[data-test='remove-{productName}']`

---

### GetCartItemCountAsync

**Signature**: `Task<int> GetCartItemCountAsync()`

**What it does**: Gets the number shown in the cart badge

**Returns**: Integer count from cart badge, or 0 if badge not visible

**Behavior**: Reads text from `.shopping_cart_badge` element

---

### ClickProductAsync

**Signature**: `Task ClickProductAsync(string productName)`

**What it does**: Navigates to product detail page by clicking product name

**Parameters**:
- `productName` (string): The visible product name to click

**Behavior**: Finds and clicks the matching `.inventory_item_name` link

---

### NavigateToCartAsync

**Signature**: `Task NavigateToCartAsync()`

**What it does**: Clicks the cart icon to navigate to cart page

**Behavior**: Clicks `.shopping_cart_link` element

---

### SortProductsAsync

**Signature**: `Task SortProductsAsync(string sortOption)`

**What it does**: Sorts products using the dropdown selector

**Parameters**:
- `sortOption` (string): Value from dropdown (az, za, lohi, hilo)

**Behavior**: Selects the specified option from `.product_sort_container`

---

### IsProductInCartAsync

**Signature**: `Task<bool> IsProductInCartAsync(string productName)`

**What it does**: Checks if a product's Remove button is visible

**Parameters**:
- `productName` (string): The product identifier in kebab-case format

**Returns**: True if Remove button visible, false if Add to Cart button visible

**Behavior**: Checks visibility of `[data-test='remove-{productName}']`

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

- Path: `src/Utilities/PageObjects/InventoryPage.cs`
- Namespace: `csharp_framework_demo.Utilities.PageObjects`
- Class Name: `InventoryPage`
