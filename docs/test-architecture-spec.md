# Test Architecture Specification: Parallel Execution with Browser Context Isolation

**Version**: 1.0
**Date**: 2026-02-10
**Status**: ✅ Implemented and Validated (78 tests passing in CI)

---

## Purpose

This specification defines the **mandatory architecture** for integration and end-to-end (E2E) tests in the C# Playwright framework. It ensures tests can run in parallel without race conditions by using **isolated browser contexts** per test method.

**Audience**: AI agents (Claude Code), human developers, code reviewers

---

## Core Principles

1. **Isolation**: Each test method gets its own `IBrowserContext` and `IPage`
2. **Shared Resources**: `IPlaywright` and `IBrowser` are shared (expensive to create)
3. **Automatic Cleanup**: Use `IAsyncDisposable` pattern (`await using`) for deterministic cleanup
4. **No Shared Mutable State**: Test fixtures must not expose shared mutable state

---

## Architecture Components

### 1. PlaywrightFixture (Shared Browser Factory)

**Location**: `src/Tests/PlaywrightTests.cs`

**Purpose**: Manages browser lifecycle (one browser per test class) and provides factory method to create isolated contexts.

**Implementation**:

```csharp
public class PlaywrightFixture : IAsyncLifetime
{
    private IPlaywright _playwright = null!;
    public IBrowser Browser { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        _playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        Browser = await _playwright.Chromium.LaunchAsync(new() { Headless = true });
    }

    public async Task<PageContext> CreatePageContextAsync()
    {
        var context = await Browser.NewContextAsync();
        var page = await context.NewPageAsync();
        return new PageContext(context, page);
    }

    public async Task DisposeAsync()
    {
        await Browser.CloseAsync();
        _playwright.Dispose();
    }
}
```

**Key Points**:
- ✅ `Browser` is **public readonly property** (shared, immutable)
- ✅ `CreatePageContextAsync()` is **factory method** (creates new instance per call)
- ❌ **Do NOT** expose `IPage` as public property (would be shared)
- ❌ **Do NOT** create `IBrowserContext` or `IPage` in `InitializeAsync()` (would be shared)

### 2. PageContext (Isolated Context + Page Wrapper)

**Location**: `src/Tests/PlaywrightTests.cs`

**Purpose**: Wraps `IBrowserContext` and `IPage` together with automatic cleanup via `IAsyncDisposable`.

**Implementation**:

```csharp
public class PageContext : IAsyncDisposable
{
    private readonly IBrowserContext _context;
    public IPage Page { get; }

    public PageContext(IBrowserContext context, IPage page)
    {
        _context = context;
        Page = page;
    }

    public async ValueTask DisposeAsync()
    {
        await _context.CloseAsync();
    }
}
```

**Key Points**:
- ✅ Implements `IAsyncDisposable` for `await using` support
- ✅ Closes context (which also closes page) on disposal
- ✅ `Page` property is public readonly (safe because each test gets new instance)
- ⚠️ **Future Enhancement**: Add screenshot capture on failure in `DisposeAsync()`

### 3. Test Class Pattern

**Pattern**: Store fixture reference, create page context per test method

**Implementation**:

```csharp
using Microsoft.Playwright;
using Xunit;
using Allure.Xunit.Attributes;
using csharp_framework_demo.Utilities.PageObjects;

namespace csharp_framework_demo.Tests;

[AllureSuite("Suite Name")]
[AllureFeature("Feature Name")]
public class ExampleTests : IClassFixture<PlaywrightFixture>
{
    private readonly PlaywrightFixture _fixture;  // ✅ Store fixture, NOT page

    public ExampleTests(PlaywrightFixture fixture)
    {
        _fixture = fixture;  // ✅ Save for later use
    }

    [Fact]
    [AllureDescription("Test description")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Tag1", "Tag2")]
    public async Task TestMethod1()
    {
        // ✅ Create isolated context and page for this test
        await using var pageContext = await _fixture.CreatePageContextAsync();
        var page = pageContext.Page;

        // ✅ Create Page Objects with isolated page
        var loginPage = new LoginPage(page);
        var inventoryPage = new InventoryPage(page);

        // Test implementation
        await loginPage.NavigateToAsync("https://www.saucedemo.com/");
        await loginPage.LoginAsync("standard_user", "secret_sauce");

        var productCount = await inventoryPage.GetProductCountAsync();
        Assert.Equal(6, productCount);

    }  // ✅ await using ensures context closes even if test fails

    [Fact]
    [AllureDescription("Another test")]
    public async Task TestMethod2()
    {
        // ✅ Create NEW isolated context and page (separate from TestMethod1)
        await using var pageContext = await _fixture.CreatePageContextAsync();
        var page = pageContext.Page;

        // Different test implementation
    }
}
```

**Key Points**:
- ✅ Test class uses `IClassFixture<PlaywrightFixture>`
- ✅ Constructor stores `_fixture` reference
- ✅ Each test method calls `CreatePageContextAsync()` to get isolated context/page
- ✅ Use `await using` for automatic cleanup
- ✅ Create Page Objects with local `page` variable (isolated per test)
- ❌ **Do NOT** store `IPage` in class field (would prevent isolation)
- ❌ **Do NOT** reuse `pageContext` across test methods

---

## E2E Workflow Tests with Allure Steps

**Pattern**: Wrap test steps in `AllureApi.Step()` for detailed reporting

**Implementation**:

```csharp
[AllureSuite("End-to-End Tests")]
[AllureFeature("Authentication")]
public class UserWorkflowTests : IClassFixture<PlaywrightFixture>
{
    private readonly PlaywrightFixture _fixture;

    public UserWorkflowTests(PlaywrightFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    [AllureDescription("Verifies user can log in and see inventory")]
    [AllureSeverity(SeverityLevel.critical)]
    public async Task SuccessfulLogin()
    {
        await using var pageContext = await _fixture.CreatePageContextAsync();
        var page = pageContext.Page;
        var loginPage = new LoginPage(page);

        await AllureApi.Step("Navigate to login page", async () =>
        {
            await loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Enter valid credentials and login", async () =>
        {
            await loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        await AllureApi.Step("Verify inventory list is visible", async () =>
        {
            var inventoryVisible = await page.IsVisibleAsync(".inventory_list");
            Assert.True(inventoryVisible);
        });
    }
}
```

**Key Points**:
- ✅ Use `AllureApi.Step()` for granular reporting in E2E tests
- ✅ Each step is async lambda: `async () => { ... }`
- ✅ Steps can contain Page Object method calls or direct page interactions
- ✅ Assertions can be inside or outside steps (preference: inside for clarity)

---

## Unit Tests (Page Object Method Validation)

**Pattern**: Mock `IPage` using Moq, validate Page Object behavior

**Implementation**:

```csharp
using Moq;
using Microsoft.Playwright;
using Xunit;
using Allure.Xunit.Attributes;
using csharp_framework_demo.Utilities.PageObjects;

namespace csharp_framework_demo.Tests;

[AllureSuite("Unit Tests")]
[AllureFeature("Page Objects")]
public class LoginPageUnitTests
{
    [Fact]
    [AllureDescription("Validates that NavigateToAsync navigates to correct URL")]
    public async Task NavigateToAsync_NavigatesToUrl()
    {
        // Arrange
        var mockPage = new Mock<IPage>();
        var loginPage = new LoginPage(mockPage.Object);
        var url = "https://www.saucedemo.com/";

        // Act
        await loginPage.NavigateToAsync(url);

        // Assert
        mockPage.Verify(p => p.GotoAsync(url, null), Times.Once);
    }

    [Fact]
    [AllureDescription("Validates that LoginAsync fills username, password, and clicks login button")]
    public async Task LoginAsync_FillsCredentialsAndClicksLogin()
    {
        // Arrange
        var mockPage = new Mock<IPage>();
        var loginPage = new LoginPage(mockPage.Object);

        // Act
        await loginPage.LoginAsync("testuser", "testpass");

        // Assert
        mockPage.Verify(p => p.FillAsync("[data-test='username']", "testuser", null), Times.Once);
        mockPage.Verify(p => p.FillAsync("[data-test='password']", "testpass", null), Times.Once);
        mockPage.Verify(p => p.ClickAsync("[data-test='login-button']", null), Times.Once);
    }
}
```

**Key Points**:
- ✅ Unit tests use `Mock<IPage>` (no browser required)
- ✅ Use `mockPage.Verify()` to assert method calls
- ✅ No fixture needed (unit tests are fast, no browser)
- ⚠️ **Critical**: When changing Page Object implementation, update unit test mocks immediately

---

## Page Object Best Practices

### Wait Strategies

**Rule**: Page Object methods must handle timing (no hardcoded `Task.Delay()` in tests)

**Strategies**:

1. **LoadState.NetworkIdle** (Most Flexible)
   ```csharp
   await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
   ```
   - ✅ Use for: Methods with multiple outcomes (success, failure, redirect)
   - ✅ Examples: LoginAsync (may succeed or show error), form submission
   - ⚠️ May be slower (waits for all network activity)

2. **WaitForURLAsync** (Specific Navigation)
   ```csharp
   await _page.WaitForURLAsync("**/inventory.html");
   ```
   - ✅ Use for: Guaranteed navigation (logout, direct link click)
   - ❌ Avoid for: Conditional navigation (may not navigate on error)

3. **WaitForSelectorAsync** (Element Visibility)
   ```csharp
   await _page.WaitForSelectorAsync(".inventory_list", new() { State = WaitForSelectorState.Visible });
   ```
   - ✅ Use for: Waiting for specific element to appear
   - ✅ Examples: Modal dialogs, success messages, loading spinners

4. **WaitForLoadStateAsync(LoadState.DOMContentLoaded)** (Fast Page Load)
   ```csharp
   await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
   ```
   - ✅ Use for: Quick navigation where network requests don't matter
   - ⚠️ May miss AJAX-loaded content

**Recommendation**: Default to `LoadState.NetworkIdle` for Page Object methods unless performance is critical.

### Visibility Checks

**Rule**: Check **interactive elements**, not containers

**Anti-Pattern**:
```csharp
// ❌ WRONG: Container may be in DOM but transformed off-screen
return await _page.IsVisibleAsync(".bm-menu");
```

**Pattern**:
```csharp
// ✅ CORRECT: Check close button (only visible when menu is actually open)
var closeButton = _page.Locator("#react-burger-cross-btn");
return await closeButton.IsVisibleAsync();
```

**Reasoning**: CSS transforms, `display: none`, `visibility: hidden` can hide containers without removing from DOM. Interactive elements (buttons, links) are more reliable indicators of true visibility.

### Async Backend Operations

**Rule**: If backend operation doesn't auto-update DOM, reload page

**Example**: SauceDemo's "Reset App"
```csharp
public async Task ResetCartWorkflow()
{
    await burgerMenuPage.ClickResetAppAsync();
    await page.ReloadAsync();  // ✅ Required: Backend resets but DOM doesn't auto-update
}
```

**When to Reload**:
- After state-changing operations (reset, settings save, logout)
- When testing persistence (state should survive refresh)
- When UI doesn't automatically sync with backend changes

---

## Validation Checklist

Use this checklist when reviewing new test implementations or refactoring existing tests:

### ✅ Fixture Pattern
- [ ] `PlaywrightFixture` exposes `Browser` property (not `Page`)
- [ ] `PlaywrightFixture` has `CreatePageContextAsync()` factory method
- [ ] `PageContext` implements `IAsyncDisposable`
- [ ] `PageContext.DisposeAsync()` closes browser context

### ✅ Test Class Pattern
- [ ] Test class uses `IClassFixture<PlaywrightFixture>`
- [ ] Constructor stores `_fixture` (not `_page`)
- [ ] Each test method calls `CreatePageContextAsync()`
- [ ] Each test uses `await using var pageContext = ...`
- [ ] Page Objects created with local `page` variable

### ✅ E2E Tests
- [ ] Tests use `AllureApi.Step()` for granular reporting
- [ ] Each step is async lambda
- [ ] Allure attributes present: `[AllureDescription]`, `[AllureSeverity]`, `[AllureOwner]`, `[AllureTag]`

### ✅ Unit Tests
- [ ] Use `Mock<IPage>` (no browser)
- [ ] Verify method calls with `mockPage.Verify()`
- [ ] Mocks match current Page Object implementation

### ✅ Page Objects
- [ ] Methods use appropriate wait strategies (no `Task.Delay()`)
- [ ] Visibility checks use interactive elements (buttons), not containers
- [ ] Async backend operations followed by `page.ReloadAsync()` if needed
- [ ] All methods are async (`async Task` or `async Task<T>`)

---

## Anti-Patterns to Avoid

### ❌ Anti-Pattern #1: Shared Page
```csharp
// ❌ WRONG: Exposes shared page
public class BadFixture : IAsyncLifetime
{
    public IPage Page { get; private set; }  // ❌ Race condition!

    public async Task InitializeAsync()
    {
        Page = await browser.NewPageAsync();  // ❌ Shared by all tests
    }
}

public class BadTests : IClassFixture<BadFixture>
{
    private readonly IPage _page;  // ❌ Shared!

    public BadTests(BadFixture fixture)
    {
        _page = fixture.Page;  // ❌ All tests use same page
    }

    [Fact] public async Task Test1() { /* uses _page */ }  // ❌ Parallel
    [Fact] public async Task Test2() { /* uses _page */ }  // ❌ Parallel
    // Race condition: Both tests fight over same page
}
```

### ❌ Anti-Pattern #2: Hardcoded Delays
```csharp
// ❌ WRONG: Hardcoded delay
await page.ClickAsync("#menu-button");
await Task.Delay(500);  // ❌ Flaky, arbitrary, slow
var isVisible = await page.IsVisibleAsync("#menu-content");

// ✅ CORRECT: Wait for actual condition
await page.ClickAsync("#menu-button");
await page.WaitForSelectorAsync("#menu-content", new() { State = WaitForSelectorState.Visible });
var isVisible = await page.IsVisibleAsync("#menu-content");
```

### ❌ Anti-Pattern #3: Container Visibility Check
```csharp
// ❌ WRONG: Check container (may be in DOM but hidden)
public async Task<bool> IsMenuOpenAsync()
{
    return await _page.IsVisibleAsync(".menu-container");  // ❌ Unreliable
}

// ✅ CORRECT: Check interactive element (close button)
public async Task<bool> IsMenuOpenAsync()
{
    var closeButton = _page.Locator("#close-button");
    return await closeButton.IsVisibleAsync();  // ✅ Reliable
}
```

### ❌ Anti-Pattern #4: Reusing PageContext
```csharp
// ❌ WRONG: Reuse PageContext across tests
public class BadTests : IClassFixture<PlaywrightFixture>
{
    private PageContext _pageContext;  // ❌ Shared field!

    public async Task InitializeAsync()
    {
        _pageContext = await _fixture.CreatePageContextAsync();  // ❌ Created once
    }

    [Fact] public async Task Test1() { var page = _pageContext.Page; }  // ❌ Shared
    [Fact] public async Task Test2() { var page = _pageContext.Page; }  // ❌ Shared
}

// ✅ CORRECT: Create new PageContext per test
[Fact]
public async Task Test1()
{
    await using var pageContext = await _fixture.CreatePageContextAsync();  // ✅ Isolated
    var page = pageContext.Page;
}
```

---

## Performance Characteristics

**Resource Creation Times** (approximate):
- `IPlaywright`: 100-200ms (one per test run)
- `IBrowser`: 1-2 seconds (one per test class)
- `IBrowserContext`: 100-200ms (one per test method)
- `IPage`: 50-100ms (one per test method)

**Parallelization**:
- xUnit runs test methods **in parallel by default** (within same class)
- xUnit runs test classes **in parallel by default**
- Isolated contexts enable true parallelization without race conditions

**Trade-offs**:
- ✅ **Shared Browser**: Fast (browser process reused)
- ✅ **Isolated Contexts**: Safe (no race conditions)
- ⚠️ **Memory Usage**: Each context uses ~50-100MB RAM
- ⚠️ **Test Count Limit**: ~100-200 concurrent contexts per browser (OS limits)

---

## Future Enhancements

### 1. Screenshot Capture on Failure
**Implementation**:
```csharp
public class PageContext : IAsyncDisposable
{
    private readonly IBrowserContext _context;
    public IPage Page { get; }

    public async ValueTask DisposeAsync()
    {
        // TODO: Check if test failed, capture screenshot
        // if (TestContext.CurrentTestOutcome == TestOutcome.Failed)
        // {
        //     await Page.ScreenshotAsync(new() { Path = $"screenshots/{testName}.png" });
        // }
        await _context.CloseAsync();
    }
}
```

### 2. Video Recording
**Implementation**:
```csharp
public async Task<PageContext> CreatePageContextAsync()
{
    var context = await Browser.NewContextAsync(new()
    {
        RecordVideoDir = "videos/",
        RecordVideoSize = new() { Width = 1280, Height = 720 }
    });
    var page = await context.NewPageAsync();
    return new PageContext(context, page);
}
```

### 3. Custom Context Options (Per Test)
**Implementation**:
```csharp
public async Task<PageContext> CreatePageContextAsync(BrowserNewContextOptions? options = null)
{
    var context = await Browser.NewContextAsync(options ?? new());
    var page = await context.NewPageAsync();
    return new PageContext(context, page);
}

// Usage: Test with mobile viewport
var pageContext = await _fixture.CreatePageContextAsync(new()
{
    ViewportSize = new() { Width = 375, Height = 667 },
    UserAgent = "Mobile Safari"
});
```

---

## References

- **Playwright Documentation**: https://playwright.dev/dotnet/docs/intro
- **xUnit Fixtures**: https://xunit.net/docs/shared-context
- **Retrospective**: See `docs/retrospective-parallel-tests.md` for historical context
- **Project Spec**: See `specs/PROJECT-SPEC.md` for overall architecture

---

## Compliance

**This specification is MANDATORY** for all integration and E2E tests in this framework.

**Code reviewers must verify**:
1. All new test classes follow fixture pattern
2. All test methods use `CreatePageContextAsync()`
3. No shared mutable state in fixtures
4. No hardcoded delays in tests or Page Objects

**AI agents must**:
1. Reference this spec when generating test code
2. Follow patterns exactly (no deviations without explicit user approval)
3. Validate generated code against checklist before submitting
4. Flag any existing code that violates this spec for refactoring

---

**Version History**:
- **1.0** (2026-02-10): Initial specification based on successful refactoring (78 tests passing)
