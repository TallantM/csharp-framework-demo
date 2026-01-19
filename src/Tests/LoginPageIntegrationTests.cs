using Microsoft.Playwright;
using Xunit;
using PlaywrightDemo.Utilities.PageObjects;

namespace PlaywrightDemo.Tests;

public class LoginPageIntegrationTests : IClassFixture<PlaywrightFixture>
{
    private readonly IPage _page;
    private readonly LoginPage _loginPage;

    public LoginPageIntegrationTests(PlaywrightFixture fixture)
    {
        _page = fixture.Page;
        _loginPage = new LoginPage(_page);
    }

    [Fact]
    public async Task LoginAsync_IntegratesWithPageAndNavigatesSuccessfully()
    {
        await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        await _loginPage.LoginAsync("standard_user", "secret_sauce");

        Assert.Equal("https://www.saucedemo.com/inventory.html", _page.Url);
        var inventoryVisible = await _page.IsVisibleAsync(".inventory_container");
        Assert.True(inventoryVisible);
    }

    [Fact]
    public async Task LoginAsync_HandlesInvalidCredentialsAndDisplaysError()
    {
        await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        await _loginPage.LoginAsync("invalid_user", "wrong_password");

        var errorVisible = await _page.IsVisibleAsync("[data-test='error']");
        Assert.True(errorVisible);
    }
}