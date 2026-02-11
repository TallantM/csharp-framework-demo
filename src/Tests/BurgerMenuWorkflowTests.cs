using Microsoft.Playwright;
using Xunit;
using csharp_framework_demo.Utilities.PageObjects;
using Allure.Net.Commons;
using Allure.Xunit.Attributes;

namespace csharp_framework_demo.Tests;

[AllureSuite("End-to-End Tests")]
[AllureFeature("Burger Menu")]
public class BurgerMenuWorkflowTests : IClassFixture<PlaywrightFixture>
{
    private readonly IPage _page;
    private readonly LoginPage _loginPage;
    private readonly BurgerMenuPage _burgerMenuPage;
    private readonly InventoryPage _inventoryPage;

    public BurgerMenuWorkflowTests(PlaywrightFixture fixture)
    {
        _page = fixture.Page;
        _loginPage = new LoginPage(_page);
        _burgerMenuPage = new BurgerMenuPage(_page);
        _inventoryPage = new InventoryPage(_page);
    }

    [Fact]
    [AllureDescription("Verifies that user can open and close burger menu")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Smoke", "BurgerMenu")]
    public async Task OpenAndCloseBurgerMenu()
    {
        await AllureApi.Step("Navigate to login page", async () =>
        {
            await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Login to application", async () =>
        {
            await _loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        await AllureApi.Step("Open burger menu", async () =>
        {
            await _burgerMenuPage.OpenMenuAsync();
        });

        await AllureApi.Step("Verify menu is open", async () =>
        {
            var isOpen = await _burgerMenuPage.IsMenuOpenAsync();
            Assert.True(isOpen);
        });

        await AllureApi.Step("Verify menu links are visible", async () =>
        {
            await Assertions.Expect(_page.Locator("#inventory_sidebar_link")).ToBeVisibleAsync();
            await Assertions.Expect(_page.Locator("#logout_sidebar_link")).ToBeVisibleAsync();
            await Assertions.Expect(_page.Locator("#about_sidebar_link")).ToBeVisibleAsync();
            await Assertions.Expect(_page.Locator("#reset_sidebar_link")).ToBeVisibleAsync();
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
    [AllureDescription("Verifies that user can logout via burger menu")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Smoke", "BurgerMenu")]
    public async Task LogoutViaBurgerMenu()
    {
        await AllureApi.Step("Navigate to login page", async () =>
        {
            await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Login to application", async () =>
        {
            await _loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        await AllureApi.Step("Verify user is on inventory page", async () =>
        {
            await Assertions.Expect(_page).ToHaveURLAsync("https://www.saucedemo.com/inventory.html");
        });

        await AllureApi.Step("Logout using burger menu", async () =>
        {
            await _burgerMenuPage.LogoutAsync();
        });

        await AllureApi.Step("Verify redirected to login page", async () =>
        {
            await Assertions.Expect(_page).ToHaveURLAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Verify login form is visible", async () =>
        {
            await Assertions.Expect(_page.Locator("[data-test='login-button']")).ToBeVisibleAsync();
        });
    }

    [Fact]
    [AllureDescription("Verifies that user can navigate to All Items from cart")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Regression", "Navigation")]
    public async Task NavigateToAllItemsFromCart()
    {
        await AllureApi.Step("Navigate and login", async () =>
        {
            await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
            await _loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        await AllureApi.Step("Add item and navigate to cart", async () =>
        {
            await _inventoryPage.AddToCartAsync("sauce-labs-backpack");
            await _inventoryPage.NavigateToCartAsync();
        });

        await AllureApi.Step("Open burger menu", async () =>
        {
            await _burgerMenuPage.OpenMenuAsync();
        });

        await AllureApi.Step("Click All Items link", async () =>
        {
            await _burgerMenuPage.ClickAllItemsAsync();
        });

        await AllureApi.Step("Verify returned to inventory page", async () =>
        {
            await Assertions.Expect(_page).ToHaveURLAsync("https://www.saucedemo.com/inventory.html");
        });

        await AllureApi.Step("Verify cart badge still shows item", async () =>
        {
            var count = await _inventoryPage.GetCartItemCountAsync();
            Assert.Equal(1, count);
        });
    }

    [Fact]
    [AllureDescription("Verifies that reset app clears cart items")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Regression", "BurgerMenu")]
    public async Task ResetAppClearsCart()
    {
        await AllureApi.Step("Navigate and login", async () =>
        {
            await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
            await _loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        await AllureApi.Step("Add two items to cart", async () =>
        {
            await _inventoryPage.AddToCartAsync("sauce-labs-backpack");
            await _inventoryPage.AddToCartAsync("sauce-labs-bike-light");
        });

        await AllureApi.Step("Verify cart badge shows 2", async () =>
        {
            var count = await _inventoryPage.GetCartItemCountAsync();
            Assert.Equal(2, count);
        });

        await AllureApi.Step("Open menu and reset app", async () =>
        {
            await _burgerMenuPage.OpenMenuAsync();
            await _burgerMenuPage.ClickResetAppAsync();
            await _burgerMenuPage.CloseMenuAsync();
        });

        await AllureApi.Step("Verify cart is empty", async () =>
        {
            var count = await _inventoryPage.GetCartItemCountAsync();
            Assert.Equal(0, count);
        });

        await AllureApi.Step("Verify Remove buttons changed to Add to Cart", async () =>
        {
            var backpackInCart = await _inventoryPage.IsProductInCartAsync("sauce-labs-backpack");
            var bikeInCart = await _inventoryPage.IsProductInCartAsync("sauce-labs-bike-light");
            Assert.False(backpackInCart);
            Assert.False(bikeInCart);
        });
    }

    [Fact]
    [AllureDescription("Verifies that burger menu is accessible from all authenticated pages")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Regression", "BurgerMenu")]
    public async Task BurgerMenuAccessibleFromAllPages()
    {
        bool isOpen;

        await AllureApi.Step("Navigate and login", async () =>
        {
            await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
            await _loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        await AllureApi.Step("Verify menu accessible from inventory page", async () =>
        {
            await _burgerMenuPage.OpenMenuAsync();
            isOpen = await _burgerMenuPage.IsMenuOpenAsync();
            Assert.True(isOpen);
            await _burgerMenuPage.CloseMenuAsync();
        });

        await AllureApi.Step("Navigate to cart and verify menu accessible", async () =>
        {
            await _inventoryPage.NavigateToCartAsync();
            await _burgerMenuPage.OpenMenuAsync();
            isOpen = await _burgerMenuPage.IsMenuOpenAsync();
            Assert.True(isOpen);
            await _burgerMenuPage.CloseMenuAsync();
        });

        await AllureApi.Step("Navigate to product details and verify menu accessible", async () =>
        {
            await _burgerMenuPage.ClickAllItemsAsync();
            await _inventoryPage.ClickProductAsync("Sauce Labs Backpack");
            await _burgerMenuPage.OpenMenuAsync();
            isOpen = await _burgerMenuPage.IsMenuOpenAsync();
            Assert.True(isOpen);
        });
    }

    [Fact]
    [AllureDescription("Verifies that about link opens external Sauce Labs page")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Regression", "BurgerMenu")]
    public async Task AboutLinkOpensExternalPage()
    {
        await AllureApi.Step("Navigate and login", async () =>
        {
            await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
            await _loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        await AllureApi.Step("Open burger menu", async () =>
        {
            await _burgerMenuPage.OpenMenuAsync();
        });

        await AllureApi.Step("Click About link", async () =>
        {
            await _burgerMenuPage.ClickAboutAsync();
        });

        await AllureApi.Step("Verify navigated to Sauce Labs website", async () =>
        {
            await _page.WaitForURLAsync("https://saucelabs.com/**");
            Assert.Contains("saucelabs.com", _page.Url);
        });
    }
}
