# {PageName} Page Object Specification

<!--
  INSTRUCTIONS:
  - Replace {PageName} with the actual page name (e.g., "Login", "Inventory", "Checkout")
  - Replace {URL} with the page URL or URL pattern
  - Define all page interactions as behavioral contracts (methods)
  - Specify selectors for each element
  - Document preconditions and invariants
  - This spec maps to: src/Utilities/PageObjects/{PageName}Page.cs
-->

## Page Overview

**Page Name**: {PageName}

**URL**: {URL or URL pattern}

**Purpose**: {Brief description of the page's role in the application}

**Responsibilities**:
- {Primary responsibility 1}
- {Primary responsibility 2}
- {Additional responsibilities...}

---

## Page Elements (Selectors)

<!-- Define all selectors used on this page -->

| Element | Selector | Type | Description |
|---------|----------|------|-------------|
| {Element Name} | `[data-test='{selector}']` or `#{id}` or `.{class}` | Input/Button/Link/etc. | {What this element does} |
| Username Field | `[data-test='username']` | Input | Text input for username |
| Password Field | `[data-test='password']` | Input | Text input for password |
| Login Button | `[data-test='login-button']` | Button | Submits login form |
| Error Message | `[data-test='error']` | Text | Displays validation errors |

---

## Behavioral Contracts (Methods)

<!-- Each method represents a page interaction that will be implemented in the Page Object class -->

### Method: NavigateToAsync

**Signature**: `public async Task NavigateToAsync(string url)`

**Purpose**: Navigate to the {PageName} page.

**Parameters**:
- `url` (string): The URL to navigate to

**Behavior**:
1. Navigate browser to specified URL
2. Wait for page load completion

**Return**: `Task` (void)

**Preconditions**: Browser is initialized

**Postconditions**: Page is loaded at specified URL

**Example Usage**:
```csharp
await {pageName}Page.NavigateToAsync("https://www.example.com/{page}");
```

---

### Method: {MethodName}Async

**Signature**: `public async Task {MethodName}Async({parameters})`

**Purpose**: {What this method does}

**Parameters**:
- `{paramName}` ({type}): {Parameter description}
- `{paramName2}` ({type}): {Parameter description}

**Behavior**:
1. {Step 1 - e.g., "Fill username field with provided username"}
2. {Step 2 - e.g., "Fill password field with provided password"}
3. {Step 3 - e.g., "Click login button"}
4. {Additional steps...}

**Return**: `Task` or `Task<{type}>` (specify return value if any)

**Preconditions**:
- {Condition 1 - e.g., "Page is loaded"}
- {Condition 2 - e.g., "User is not already logged in"}

**Postconditions**:
- {Expected state 1 - e.g., "User is authenticated"}
- {Expected state 2 - e.g., "Redirected to dashboard"}

**Example Usage**:
```csharp
await {pageName}Page.{MethodName}Async({exampleParams});
```

**Error Scenarios**:
- {Error condition}: {What happens}
- {Error condition 2}: {What happens}

---

### Method: Is{Element}VisibleAsync

**Signature**: `public async Task<bool> Is{Element}VisibleAsync()`

**Purpose**: Check if {element} is visible on the page.

**Parameters**: None

**Behavior**:
1. Query page for {element} visibility

**Return**: `Task<bool>` - `true` if visible, `false` otherwise

**Preconditions**: Page is loaded

**Example Usage**:
```csharp
bool isVisible = await {pageName}Page.Is{Element}VisibleAsync();
Assert.True(isVisible);
```

---

### Method: Get{Element}TextAsync

**Signature**: `public async Task<string> Get{Element}TextAsync()`

**Purpose**: Retrieve text content from {element}.

**Parameters**: None

**Behavior**:
1. Wait for {element} to be visible
2. Extract text content from element

**Return**: `Task<string>` - Text content of element

**Preconditions**:
- Page is loaded
- {Element} exists and is visible

**Example Usage**:
```csharp
string text = await {pageName}Page.Get{Element}TextAsync();
Assert.Equal("Expected Text", text);
```

---

## Page Invariants

<!-- Conditions that should always be true for this page -->

- {Invariant 1 - e.g., "Login button is disabled until both username and password fields have values"}
- {Invariant 2 - e.g., "Error message is only visible after failed login attempt"}
- {Invariant 3 - e.g., "Password field is always type='password' (masked)"}

---

## State Transitions

<!-- Document how interacting with this page changes application state -->

```
Initial State:
  - User is on {PageName} page
  - {Element} is visible
  - User is not authenticated

After {Action} (e.g., successful login):
  - User is authenticated
  - Redirected to {NextPage} page
  - Session cookie is set

After {ErrorAction} (e.g., invalid credentials):
  - User remains on {PageName} page
  - Error message is displayed
  - User is not authenticated
```

---

## Example Scenarios

### Scenario 1: {Successful Interaction}

**Given**: {Precondition}
**When**: {Action performed}
**Then**: {Expected outcome}

**Code Example**:
```csharp
await {pageName}Page.NavigateToAsync("{url}");
await {pageName}Page.{MethodName}Async({params});
// Assert expected state
```

---

### Scenario 2: {Error Interaction}

**Given**: {Precondition}
**When**: {Error action performed}
**Then**: {Expected error state}

**Code Example**:
```csharp
await {pageName}Page.NavigateToAsync("{url}");
await {pageName}Page.{MethodName}Async({invalidParams});
bool errorVisible = await {pageName}Page.IsErrorMessageVisibleAsync();
Assert.True(errorVisible);
```

---

## Notes

- {Additional notes about this page}
- {Implementation considerations}
- {Known limitations or edge cases}
- {Dependencies on other pages or services}

---

## Mapping to Code

**Generated Class**: `src/Utilities/PageObjects/{PageName}Page.cs`

**Namespace**: `csharp_framework_demo.Utilities.PageObjects`

**Constructor**: Accepts `IPage page` parameter via dependency injection

**Methods**: All methods defined in "Behavioral Contracts" section above

**Compliance**: Must follow PROJECT-SPEC.md Page Object Standards
