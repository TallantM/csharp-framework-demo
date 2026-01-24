using Microsoft.Playwright;
using Xunit;
using csharp_framework_demo.Utilities.PageObjects;
using Allure.Net.Commons;
using Allure.Xunit.Attributes;

namespace csharp_framework_demo.Tests;

[AllureSuite("End-to-End Tests")]
[AllureFeature("Authentication")]
public class ExampleTests : IClassFixture<PlaywrightFixture>
{
    private readonly IPage _page;
    private readonly LoginPage _loginPage;

    public ExampleTests(PlaywrightFixture fixture)
    {
        _page = fixture.Page;
        _loginPage = new LoginPage(_page);
    }

    [Fact]
    [AllureDescription("Verifies that a user can successfully log in with valid credentials and see the inventory list")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Smoke", "Login")]
    public async Task SuccessfulLogin()
    {
        await AllureApi.Step("Navigate to login page", async () =>
        {
            await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Enter valid credentials and login", async () =>
        {
            await _loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        await AllureApi.Step("Verify inventory list is visible", async () =>
        {
            var inventoryVisible = await _page.IsVisibleAsync(".inventory_list");
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
        await AllureApi.Step("Navigate to login page", async () =>
        {
            await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Login with valid credentials", async () =>
        {
            await _loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        await AllureApi.Step("Verify URL redirects to inventory page", async () =>
        {
            await Assertions.Expect(_page).ToHaveURLAsync("https://www.saucedemo.com/inventory.html");
        });

        await AllureApi.Step("Verify inventory container is visible", async () =>
        {
            var inventoryContainer = _page.Locator(".inventory_container");
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
        await AllureApi.Step("Navigate to login page", async () =>
        {
            await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Login with valid credentials", async () =>
        {
            await _loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        await AllureApi.Step("Open burger menu", async () =>
        {
            await _page.ClickAsync("#react-burger-menu-btn");
        });

        await AllureApi.Step("Click logout link", async () =>
        {
            await _page.ClickAsync("#logout_sidebar_link");
        });

        await AllureApi.Step("Verify redirected to login page", async () =>
        {
            await Assertions.Expect(_page).ToHaveURLAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Verify login button is visible", async () =>
        {
            var loginButton = _page.Locator("[data-test='login-button']");
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
        await AllureApi.Step("Navigate to login page", async () =>
        {
            await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Attempt login with invalid credentials", async () =>
        {
            await _loginPage.LoginAsync("invalid_user", "wrong_password");
        });

        await AllureApi.Step("Verify error message is displayed", async () =>
        {
            var errorMessage = await _page.TextContentAsync("[data-test='error']");
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
        await AllureApi.Step("Navigate to login page", async () =>
        {
            await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Click login button without entering credentials", async () =>
        {
            await _page.ClickAsync("[data-test='login-button']");
        });

        await AllureApi.Step("Verify username required error is displayed", async () =>
        {
            var errorMessage = await _page.TextContentAsync("[data-test='error']");
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
        await AllureApi.Step("Navigate to login page", async () =>
        {
            await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Attempt login with locked out user", async () =>
        {
            await _loginPage.LoginAsync("locked_out_user", "secret_sauce");
        });

        await AllureApi.Step("Verify locked out error message is displayed", async () =>
        {
            var errorMessage = await _page.TextContentAsync("[data-test='error']");
            Assert.Equal("Epic sadface: Sorry, this user has been locked out.", errorMessage);
        });
    }
}

public class PlaywrightFixture : IAsyncLifetime
{
    public IPlaywright Playwright { get; private set; } = null!;
    public IBrowser Browser { get; private set; } = null!;
    public IPage Page { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        Browser = await Playwright.Chromium.LaunchAsync(new() { Headless = true });
        Page = await Browser.NewPageAsync();
    }

    public async Task DisposeAsync()
    {
        await Browser.CloseAsync();
        Playwright.Dispose();
    }
}