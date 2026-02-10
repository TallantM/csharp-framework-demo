# Product Details Page Object Specification

## Page Overview

**Page Name**: ProductDetailsPage
**URL**: https://www.saucedemo.com/inventory-item.html?id={product-id}
**Purpose**: View detailed product information, add to cart, and navigate back to inventory
**Namespace**: csharp_framework_demo.Utilities.PageObjects

### What It Does
Wraps the SauceDemo product detail page where users view full product information and manage cart actions for a specific product.

---

## Page Elements

| Element | Selector | Type | Purpose |
|---------|----------|------|---------|
| Product Name | `.inventory_details_name` | Text | Product title |
| Product Description | `.inventory_details_desc` | Text | Product full description |
| Product Price | `.inventory_details_price` | Text | Product price |
| Product Image | `.inventory_details_img` | Image | Product photo |
| Add to Cart Button | `[data-test='add-to-cart']` | Button | Add product to cart |
| Remove Button | `[data-test='remove']` | Button | Remove product from cart |
| Back to Products | `[data-test='back-to-products']` | Button | Return to inventory page |
| Cart Badge | `.shopping_cart_badge` | Badge | Shows number of items in cart |

### Selector Strategy
Use `data-test` attributes for buttons. Class selectors for product information display elements.

---

## Methods

### GetProductNameAsync

**Signature**: `Task<string> GetProductNameAsync()`

**What it does**: Retrieves the product name from the detail page

**Returns**: Product name string

**Behavior**: Reads text from `.inventory_details_name` element

---

### GetProductDescriptionAsync

**Signature**: `Task<string> GetProductDescriptionAsync()`

**What it does**: Retrieves the full product description

**Returns**: Product description text

**Behavior**: Reads text from `.inventory_details_desc` element

---

### GetProductPriceAsync

**Signature**: `Task<string> GetProductPriceAsync()`

**What it does**: Retrieves the product price

**Returns**: Price string (e.g., "$29.99")

**Behavior**: Reads text from `.inventory_details_price` element

---

### AddToCartAsync

**Signature**: `Task AddToCartAsync()`

**What it does**: Adds the current product to the shopping cart

**Behavior**: Clicks `[data-test='add-to-cart']` button

---

### RemoveFromCartAsync

**Signature**: `Task RemoveFromCartAsync()`

**What it does**: Removes the current product from the shopping cart

**Behavior**: Clicks `[data-test='remove']` button

---

### IsProductInCartAsync

**Signature**: `Task<bool> IsProductInCartAsync()`

**What it does**: Checks if the current product is in the cart

**Returns**: True if Remove button visible, false if Add to Cart button visible

**Behavior**: Checks visibility of `[data-test='remove']` element

---

### ClickBackToProductsAsync

**Signature**: `Task ClickBackToProductsAsync()`

**What it does**: Navigates back to the inventory page

**Behavior**: Clicks `[data-test='back-to-products']` button

---

### GetCartItemCountAsync

**Signature**: `Task<int> GetCartItemCountAsync()`

**What it does**: Gets the number shown in the cart badge

**Returns**: Integer count from cart badge, or 0 if badge not visible

**Behavior**: Reads text from `.shopping_cart_badge` element and parses to integer

---

### IsImageVisibleAsync

**Signature**: `Task<bool> IsImageVisibleAsync()`

**What it does**: Checks if the product image is displayed

**Returns**: True if image visible, false otherwise

**Behavior**: Checks visibility of `.inventory_details_img` element

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

- Path: `src/Utilities/PageObjects/ProductDetailsPage.cs`
- Namespace: `csharp_framework_demo.Utilities.PageObjects`
- Class Name: `ProductDetailsPage`
