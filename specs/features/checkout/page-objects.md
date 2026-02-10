# Checkout Page Object Specification

## Page Overview

**Page Name**: CheckoutPage
**URL**: https://www.saucedemo.com/checkout-step-one.html (and step-two.html, complete.html)
**Purpose**: Collect customer information, review order, and complete purchase
**Namespace**: csharp_framework_demo.Utilities.PageObjects

### What It Does
Wraps the SauceDemo checkout process across multiple steps: information entry, order review, and purchase completion.

---

## Page Elements

| Element | Selector | Type | Purpose |
|---------|----------|------|---------|
| First Name | `[data-test='firstName']` | Input | Customer first name |
| Last Name | `[data-test='lastName']` | Input | Customer last name |
| Postal Code | `[data-test='postalCode']` | Input | Customer postal/zip code |
| Continue Button | `[data-test='continue']` | Button | Proceed to order review |
| Cancel Button | `[data-test='cancel']` | Button | Return to cart |
| Payment Info | `.summary_info_label` | Text | Payment information label |
| Shipping Info | `.summary_info_label` | Text | Shipping information label |
| Price Total | `.summary_subtotal_label` | Text | Subtotal before tax |
| Tax | `.summary_tax_label` | Text | Tax amount |
| Total | `.summary_total_label` | Text | Final total with tax |
| Finish Button | `[data-test='finish']` | Button | Complete purchase |
| Back Button | `[data-test='back-to-products']` | Button | Return to inventory after completion |
| Complete Header | `.complete-header` | Text | Order confirmation message |
| Complete Text | `.complete-text` | Text | Order confirmation details |
| Error Message | `[data-test='error']` | Text | Validation error message |

### Selector Strategy
Use `data-test` attributes for form fields and buttons. Class selectors for summary information and messages.

---

## Methods

### FillCheckoutInformationAsync

**Signature**: `Task FillCheckoutInformationAsync(string firstName, string lastName, string postalCode)`

**What it does**: Fills the customer information form on step one

**Parameters**:
- `firstName` (string): Customer's first name
- `lastName` (string): Customer's last name
- `postalCode` (string): Customer's postal/zip code

**Behavior**: Fills all three form fields with provided values

---

### ClickContinueAsync

**Signature**: `Task ClickContinueAsync()`

**What it does**: Proceeds from step one to step two (order review)

**Behavior**: Clicks `[data-test='continue']` button

---

### ClickCancelAsync

**Signature**: `Task ClickCancelAsync()`

**What it does**: Cancels checkout and returns to cart

**Behavior**: Clicks `[data-test='cancel']` button

---

### GetSubtotalAsync

**Signature**: `Task<string> GetSubtotalAsync()`

**What it does**: Retrieves the subtotal amount from order summary

**Returns**: Subtotal text (e.g., "Item total: $29.99")

**Behavior**: Reads text from `.summary_subtotal_label` element

---

### GetTaxAsync

**Signature**: `Task<string> GetTaxAsync()`

**What it does**: Retrieves the tax amount from order summary

**Returns**: Tax text (e.g., "Tax: $2.40")

**Behavior**: Reads text from `.summary_tax_label` element

---

### GetTotalAsync

**Signature**: `Task<string> GetTotalAsync()`

**What it does**: Retrieves the final total from order summary

**Returns**: Total text (e.g., "Total: $32.39")

**Behavior**: Reads text from `.summary_total_label` element

---

### ClickFinishAsync

**Signature**: `Task ClickFinishAsync()`

**What it does**: Completes the purchase

**Behavior**: Clicks `[data-test='finish']` button on step two

---

### GetConfirmationMessageAsync

**Signature**: `Task<string> GetConfirmationMessageAsync()`

**What it does**: Retrieves the order confirmation header text

**Returns**: Confirmation message (e.g., "Thank you for your order!")

**Behavior**: Reads text from `.complete-header` element

---

### GetConfirmationDetailsAsync

**Signature**: `Task<string> GetConfirmationDetailsAsync()`

**What it does**: Retrieves the confirmation details text

**Returns**: Details text describing order dispatch

**Behavior**: Reads text from `.complete-text` element

---

### ClickBackToProductsAsync

**Signature**: `Task ClickBackToProductsAsync()`

**What it does**: Returns to inventory page after order completion

**Behavior**: Clicks `[data-test='back-to-products']` button

---

### IsErrorVisibleAsync

**Signature**: `Task<bool> IsErrorVisibleAsync()`

**What it does**: Checks if validation error message is displayed

**Returns**: True if error visible, false otherwise

**Behavior**: Checks visibility of `[data-test='error']` element

---

### GetErrorMessageAsync

**Signature**: `Task<string> GetErrorMessageAsync()`

**What it does**: Retrieves the validation error message text

**Returns**: Error message text

**Behavior**: Reads text from `[data-test='error']` element

---

## Implementation Rules

**Constructor**:
- Must accept `IPage` parameter
- Store in private readonly field

**Methods**:
- All async (return Task or Task<T>)
- All Playwright calls use await
- No assertions (Page Object performs actions, tests do assertions)

**Multi-Step Flow**:
- Methods work across checkout steps (step-one, step-two, complete)
- No need for separate page objects per step

---

## File Location

- Path: `src/Utilities/PageObjects/CheckoutPage.cs`
- Namespace: `csharp_framework_demo.Utilities.PageObjects`
- Class Name: `CheckoutPage`
