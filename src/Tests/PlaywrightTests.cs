using Microsoft.Playwright;
using Xunit;
using csharp_framework_demo.Utilities.PageObjects;
using Allure.Net.Commons;
using Allure.Xunit.Attributes;

namespace csharp_framework_demo.Tests;

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
    [AllureDescription("Verifies that a user can successfully log in with valid credentials and see the inventory list")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Smoke", "Login")]
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

    [Fact]
    [AllureDescription("Verifies that after successful login, user is redirected to inventory page with correct URL and container is visible")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Smoke", "Navigation")]
    public async Task NavigateToInventoryAfterSuccessfulLogin()
    {
        await using var pageContext = await _fixture.CreatePageContextAsync();
        var page = pageContext.Page;
        var loginPage = new LoginPage(page);

        await AllureApi.Step("Navigate to login page", async () =>
        {
            await loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Login with valid credentials", async () =>
        {
            await loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        await AllureApi.Step("Verify URL redirects to inventory page", async () =>
        {
            await Assertions.Expect(page).ToHaveURLAsync("https://www.saucedemo.com/inventory.html");
        });

        await AllureApi.Step("Verify inventory container is visible", async () =>
        {
            var inventoryContainer = page.Locator(".inventory_container");
            await Assertions.Expect(inventoryContainer).ToBeVisibleAsync();
        });
    }

    [Fact]
    [AllureDescription("Verifies that a user can successfully logout after logging in and returns to login page")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Regression", "Logout")]
    public async Task LogoutAfterSuccessfulLogin()
    {
        await using var pageContext = await _fixture.CreatePageContextAsync();
        var page = pageContext.Page;
        var loginPage = new LoginPage(page);

        await AllureApi.Step("Navigate to login page", async () =>
        {
            await loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Login with valid credentials", async () =>
        {
            await loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        await AllureApi.Step("Open burger menu", async () =>
        {
            await page.ClickAsync("#react-burger-menu-btn");
        });

        await AllureApi.Step("Click logout link", async () =>
        {
            await page.ClickAsync("#logout_sidebar_link");
        });

        await AllureApi.Step("Verify redirected to login page", async () =>
        {
            await Assertions.Expect(page).ToHaveURLAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Verify login button is visible", async () =>
        {
            var loginButton = page.Locator("[data-test='login-button']");
            await Assertions.Expect(loginButton).ToBeVisibleAsync();
        });
    }

    [Fact]
    [AllureDescription("Verifies that login fails with invalid credentials and displays appropriate error message")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Smoke", "Validation", "Negative")]
    public async Task FailedLogin_InvalidCredentials()
    {
        await using var pageContext = await _fixture.CreatePageContextAsync();
        var page = pageContext.Page;
        var loginPage = new LoginPage(page);

        await AllureApi.Step("Navigate to login page", async () =>
        {
            await loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Attempt login with invalid credentials", async () =>
        {
            await loginPage.LoginAsync("invalid_user", "wrong_password");
        });

        await AllureApi.Step("Verify error message is displayed", async () =>
        {
            var errorMessage = await page.TextContentAsync("[data-test='error']");
            Assert.Equal("Epic sadface: Username and password do not match any user in this service", errorMessage);
        });
    }

    [Fact]
    [AllureDescription("Verifies that login fails when no credentials are provided and displays username required error")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Regression", "Validation", "Negative")]
    public async Task FailedLogin_EmptyCredentials()
    {
        await using var pageContext = await _fixture.CreatePageContextAsync();
        var page = pageContext.Page;
        var loginPage = new LoginPage(page);

        await AllureApi.Step("Navigate to login page", async () =>
        {
            await loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Click login button without entering credentials", async () =>
        {
            await page.ClickAsync("[data-test='login-button']");
        });

        await AllureApi.Step("Verify username required error is displayed", async () =>
        {
            var errorMessage = await page.TextContentAsync("[data-test='error']");
            Assert.Equal("Epic sadface: Username is required", errorMessage);
        });
    }

    [Fact]
    [AllureDescription("Verifies that login fails for locked out user and displays locked out error message")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Smoke", "Validation", "Negative")]
    public async Task FailedLogin_LockedUser()
    {
        await using var pageContext = await _fixture.CreatePageContextAsync();
        var page = pageContext.Page;
        var loginPage = new LoginPage(page);

        await AllureApi.Step("Navigate to login page", async () =>
        {
            await loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Attempt login with locked out user", async () =>
        {
            await loginPage.LoginAsync("locked_out_user", "secret_sauce");
        });

        await AllureApi.Step("Verify locked out error message is displayed", async () =>
        {
            var errorMessage = await page.TextContentAsync("[data-test='error']");
            Assert.Equal("Epic sadface: Sorry, this user has been locked out.", errorMessage);
        });
    }
}

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