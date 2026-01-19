using Microsoft.Playwright;
using Xunit;
using PlaywrightDemo.Utilities.PageObjects;

namespace PlaywrightDemo.Tests;
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
    public async Task SuccessfulLogin()
    {
        await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        await _loginPage.LoginAsync("standard_user", "secret_sauce");
        var inventoryVisible = await _page.IsVisibleAsync(".inventory_list");
        Assert.True(inventoryVisible);
    }
    [Fact]
    public async Task NavigateToInventoryAfterSuccessfulLogin()
    {
        await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        await _loginPage.LoginAsync("standard_user", "secret_sauce");
        await Assertions.Expect(_page).ToHaveURLAsync("https://www.saucedemo.com/inventory.html");
        var inventoryContainer = _page.Locator(".inventory_container");
        await Assertions.Expect(inventoryContainer).ToBeVisibleAsync();
    }
    [Fact]
    public async Task LogoutAfterSuccessfulLogin()
    {
        await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        await _loginPage.LoginAsync("standard_user", "secret_sauce");
        await _page.ClickAsync("#react-burger-menu-btn");
        await _page.ClickAsync("#logout_sidebar_link");
        await Assertions.Expect(_page).ToHaveURLAsync("https://www.saucedemo.com/");
        var loginButton = _page.Locator("[data-test='login-button']");
        await Assertions.Expect(loginButton).ToBeVisibleAsync();
    }
    [Fact]
    public async Task FailedLogin_InvalidCredentials()
    {
        await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        await _loginPage.LoginAsync("invalid_user", "wrong_password");
        var errorMessage = await _page.TextContentAsync("[data-test='error']");
        Assert.Equal("Epic sadface: Username and password do not match any user in this service", errorMessage);
    }
    [Fact]
    public async Task FailedLogin_EmptyCredentials()
    {
        await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        await _page.ClickAsync("[data-test='login-button']");
        var errorMessage = await _page.TextContentAsync("[data-test='error']");
        Assert.Equal("Epic sadface: Username is required", errorMessage);
    }
    [Fact]
    public async Task FailedLogin_LockedUser()
    {
        await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        await _loginPage.LoginAsync("locked_out_user", "secret_sauce");
        var errorMessage = await _page.TextContentAsync("[data-test='error']");
        Assert.Equal("Epic sadface: Sorry, this user has been locked out.", errorMessage);
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