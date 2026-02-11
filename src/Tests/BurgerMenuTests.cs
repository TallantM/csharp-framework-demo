using Microsoft.Playwright;
using Xunit;
using csharp_framework_demo.Utilities.PageObjects;
using Allure.Net.Commons;
using Allure.Xunit.Attributes;

namespace csharp_framework_demo.Tests;

[AllureSuite("Integration Tests")]
[AllureFeature("Burger Menu Page Object")]
public class BurgerMenuTests : IClassFixture<PlaywrightFixture>
{
    private readonly IPage _page;
    private readonly LoginPage _loginPage;
    private readonly BurgerMenuPage _burgerMenuPage;

    public BurgerMenuTests(PlaywrightFixture fixture)
    {
        _page = fixture.Page;
        _loginPage = new LoginPage(_page);
        _burgerMenuPage = new BurgerMenuPage(_page);
    }

    private async Task LoginAsync()
    {
        await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        await _loginPage.LoginAsync("standard_user", "secret_sauce");
    }

    [Fact]
    [AllureDescription("Verifies that OpenMenuAsync opens the burger menu")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "BurgerMenu")]
    public async Task OpenMenu_OpensMenuSuccessfully()
    {
        await AllureApi.Step("Login to application", async () => await LoginAsync());

        await AllureApi.Step("Open burger menu", async () =>
        {
            await _burgerMenuPage.OpenMenuAsync();
        });

        await AllureApi.Step("Verify menu is open", async () =>
        {
            var isOpen = await _burgerMenuPage.IsMenuOpenAsync();
            Assert.True(isOpen);
        });
    }

    [Fact]
    [AllureDescription("Verifies that CloseMenuAsync closes the burger menu")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "BurgerMenu")]
    public async Task CloseMenu_ClosesMenuSuccessfully()
    {
        await AllureApi.Step("Login and open menu", async () =>
        {
            await LoginAsync();
            await _burgerMenuPage.OpenMenuAsync();
        });

        await AllureApi.Step("Close burger menu", async () =>
        {
            await _burgerMenuPage.CloseMenuAsync();
        });

        await AllureApi.Step("Verify menu is closed", async () =>
        {
            var isOpen = await _burgerMenuPage.IsMenuOpenAsync();
            Assert.False(isOpen);
        });
    }

    [Fact]
    [AllureDescription("Verifies that ClickLogoutAsync logs out the user")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "BurgerMenu")]
    public async Task ClickLogout_LogsOutUser()
    {
        await AllureApi.Step("Login and open menu", async () =>
        {
            await LoginAsync();
            await _burgerMenuPage.OpenMenuAsync();
        });

        await AllureApi.Step("Click logout", async () =>
        {
            await _burgerMenuPage.ClickLogoutAsync();
        });

        AllureApi.Step("Verify redirected to login page", () =>
        {
            Assert.Equal("https://www.saucedemo.com/", _page.Url);
        });
    }

    [Fact]
    [AllureDescription("Verifies that LogoutAsync convenience method works correctly")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "BurgerMenu")]
    public async Task LogoutAsync_PerformsFullLogout()
    {
        await AllureApi.Step("Login to application", async () => await LoginAsync());

        await AllureApi.Step("Logout using LogoutAsync", async () =>
        {
            await _burgerMenuPage.LogoutAsync();
        });

        AllureApi.Step("Verify redirected to login page", () =>
        {
            Assert.Equal("https://www.saucedemo.com/", _page.Url);
        });

        await AllureApi.Step("Verify login button is visible", async () =>
        {
            await Assertions.Expect(_page.Locator("[data-test='login-button']")).ToBeVisibleAsync();
        });
    }

    [Fact]
    [AllureDescription("Verifies that ClickAllItemsAsync navigates to inventory")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "BurgerMenu")]
    public async Task ClickAllItems_NavigatesToInventory()
    {
        await AllureApi.Step("Login and navigate to cart", async () =>
        {
            await LoginAsync();
            await _page.ClickAsync(".shopping_cart_link");
        });

        await AllureApi.Step("Open menu and click All Items", async () =>
        {
            await _burgerMenuPage.OpenMenuAsync();
            await _burgerMenuPage.ClickAllItemsAsync();
        });

        AllureApi.Step("Verify returned to inventory page", () =>
        {
            Assert.Equal("https://www.saucedemo.com/inventory.html", _page.Url);
        });
    }

    [Fact]
    [AllureDescription("Verifies that ClickResetAppAsync resets application state")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "BurgerMenu")]
    public async Task ClickResetApp_ResetsAppState()
    {
        await AllureApi.Step("Login and add items to cart", async () =>
        {
            await LoginAsync();
            await _page.ClickAsync("[data-test='add-to-cart-sauce-labs-backpack']");
        });

        await AllureApi.Step("Open menu and reset app", async () =>
        {
            await _burgerMenuPage.OpenMenuAsync();
            await _burgerMenuPage.ClickResetAppAsync();
        });

        await AllureApi.Step("Close menu", async () =>
        {
            await _burgerMenuPage.CloseMenuAsync();
        });

        await AllureApi.Step("Verify cart is reset", async () =>
        {
            var badgeVisible = await _page.IsVisibleAsync(".shopping_cart_badge");
            Assert.False(badgeVisible);
        });
    }

    [Fact]
    [AllureDescription("Verifies that logout link is visible in menu")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "BurgerMenu")]
    public async Task IsLogoutLinkVisible_ReturnsTrueWhenMenuOpen()
    {
        await AllureApi.Step("Login and open menu", async () =>
        {
            await LoginAsync();
            await _burgerMenuPage.OpenMenuAsync();
        });

        bool isVisible = false;
        await AllureApi.Step("Check if logout link is visible", async () =>
        {
            isVisible = await _burgerMenuPage.IsLogoutLinkVisibleAsync();
        });

        AllureApi.Step("Verify logout link is visible", () =>
        {
            Assert.True(isVisible);
        });
    }
}
