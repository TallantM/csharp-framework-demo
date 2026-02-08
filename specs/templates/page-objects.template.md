# {PageName} Page Object Specification

<!--
INSTRUCTIONS:
- Replace {PageName} with your page name (e.g., "Login", "Inventory")
- Replace {URL} with the actual page URL
- Fill in the selectors table with all elements on the page
- Document each method the Page Object should have
- Keep it concise - focus on WHAT, not HOW
- This spec should guide AI/developers to generate the Page Object class
-->

## Page Overview

**Page Name**: {PageName}
**URL**: {URL}
**Purpose**: {Brief description - what does this page let users do?}
**Namespace**: csharp_framework_demo.Utilities.PageObjects

### What It Does
{1-2 sentences explaining what this Page Object wraps and why it's useful}

---

## Page Elements

| Element | Selector | Type | Purpose |
|---------|----------|------|---------|
| {Element Name} | `[data-test='{selector}']` | Input/Button/Link | {What it's for} |
| {Element Name 2} | `#{id}` or `.{class}` | Input/Button/etc. | {What it's for} |

### Selector Strategy
Use `data-test` attributes when available (they're stable and meant for testing)

---

## Methods

### {MethodName}Async

**Signature**: `Task {MethodName}Async({parameters})`

**What it does**: {Brief description}

**Parameters**:
- `{paramName}` ({type}): {What it's for}

**Behavior**:
{What happens when you call this method - keep it simple}

---

### {AnotherMethodName}Async

**Signature**: `Task<{ReturnType}> {MethodName}Async()`

**What it does**: {Brief description}

**Returns**: {What it returns and what that means}

**Behavior**:
{What happens when you call this}

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

- Path: `src/Utilities/PageObjects/{PageName}Page.cs`
- Namespace: `csharp_framework_demo.Utilities.PageObjects`
- Class Name: `{PageName}Page`
