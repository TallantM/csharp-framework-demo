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
    private readonly PlaywrightFixture _fixture;

    public BurgerMenuWorkflowTests(PlaywrightFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    [AllureDescription("Verifies that user can open and close burger menu")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Smoke", "BurgerMenu")]
    public async Task OpenAndCloseBurgerMenu()
    {
        await using var pageContext = await _fixture.CreatePageContextAsync();
        var page = pageContext.Page;
        var loginPage = new LoginPage(page);
        var burgerMenuPage = new BurgerMenuPage(page);

        await AllureApi.Step("Navigate to login page", async () =>
        {
            await loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Login to application", async () =>
        {
            await loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        await AllureApi.Step("Open burger menu", async () =>
        {
            await burgerMenuPage.OpenMenuAsync();
        });

        await AllureApi.Step("Verify menu is open", async () =>
        {
            var isOpen = await burgerMenuPage.IsMenuOpenAsync();
            Assert.True(isOpen);
        });

        await AllureApi.Step("Verify menu links are visible", async () =>
        {
            await Assertions.Expect(page.Locator("#inventory_sidebar_link")).ToBeVisibleAsync();
            await Assertions.Expect(page.Locator("#logout_sidebar_link")).ToBeVisibleAsync();
            await Assertions.Expect(page.Locator("#about_sidebar_link")).ToBeVisibleAsync();
            await Assertions.Expect(page.Locator("#reset_sidebar_link")).ToBeVisibleAsync();
        });

        await AllureApi.Step("Close burger menu", async () =>
        {
            await burgerMenuPage.CloseMenuAsync();
        });

        await AllureApi.Step("Verify menu is closed", async () =>
        {
            var isOpen = await burgerMenuPage.IsMenuOpenAsync();
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
        await using var pageContext = await _fixture.CreatePageContextAsync();
        var page = pageContext.Page;
        var loginPage = new LoginPage(page);
        var burgerMenuPage = new BurgerMenuPage(page);

        await AllureApi.Step("Navigate to login page", async () =>
        {
            await loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Login to application", async () =>
        {
            await loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        await AllureApi.Step("Verify user is on inventory page", async () =>
        {
            await Assertions.Expect(page).ToHaveURLAsync("https://www.saucedemo.com/inventory.html");
        });

        await AllureApi.Step("Logout using burger menu", async () =>
        {
            await burgerMenuPage.LogoutAsync();
        });

        await AllureApi.Step("Verify redirected to login page", async () =>
        {
            await Assertions.Expect(page).ToHaveURLAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Verify login form is visible", async () =>
        {
            await Assertions.Expect(page.Locator("[data-test='login-button']")).ToBeVisibleAsync();
        });
    }

    [Fact]
    [AllureDescription("Verifies that user can navigate to All Items from cart")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Regression", "Navigation")]
    public async Task NavigateToAllItemsFromCart()
    {
        await using var pageContext = await _fixture.CreatePageContextAsync();
        var page = pageContext.Page;
        var loginPage = new LoginPage(page);
        var burgerMenuPage = new BurgerMenuPage(page);
        var inventoryPage = new InventoryPage(page);

        await AllureApi.Step("Navigate and login", async () =>
        {
            await loginPage.NavigateToAsync("https://www.saucedemo.com/");
            await loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        await AllureApi.Step("Add item and navigate to cart", async () =>
        {
            await inventoryPage.AddToCartAsync("sauce-labs-backpack");
            await inventoryPage.NavigateToCartAsync();
        });

        await AllureApi.Step("Open burger menu", async () =>
        {
            await burgerMenuPage.OpenMenuAsync();
        });

        await AllureApi.Step("Click All Items link", async () =>
        {
            await burgerMenuPage.ClickAllItemsAsync();
        });

        await AllureApi.Step("Verify returned to inventory page", async () =>
        {
            await Assertions.Expect(page).ToHaveURLAsync("https://www.saucedemo.com/inventory.html");
        });

        await AllureApi.Step("Verify cart badge still shows item", async () =>
        {
            var count = await inventoryPage.GetCartItemCountAsync();
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
        await using var pageContext = await _fixture.CreatePageContextAsync();
        var page = pageContext.Page;
        var loginPage = new LoginPage(page);
        var burgerMenuPage = new BurgerMenuPage(page);
        var inventoryPage = new InventoryPage(page);

        await AllureApi.Step("Navigate and login", async () =>
        {
            await loginPage.NavigateToAsync("https://www.saucedemo.com/");
            await loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        await AllureApi.Step("Add two items to cart", async () =>
        {
            await inventoryPage.AddToCartAsync("sauce-labs-backpack");
            await inventoryPage.AddToCartAsync("sauce-labs-bike-light");
        });

        await AllureApi.Step("Verify cart badge shows 2", async () =>
        {
            var count = await inventoryPage.GetCartItemCountAsync();
            Assert.Equal(2, count);
        });

        await AllureApi.Step("Open menu and reset app", async () =>
        {
            await burgerMenuPage.OpenMenuAsync();
            await burgerMenuPage.ClickResetAppAsync();
            await burgerMenuPage.CloseMenuAsync();
        });

        await AllureApi.Step("Verify cart is empty", async () =>
        {
            var count = await inventoryPage.GetCartItemCountAsync();
            Assert.Equal(0, count);
        });

        await AllureApi.Step("Verify Remove buttons changed to Add to Cart", async () =>
        {
            var backpackInCart = await inventoryPage.IsProductInCartAsync("sauce-labs-backpack");
            var bikeInCart = await inventoryPage.IsProductInCartAsync("sauce-labs-bike-light");
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
        await using var pageContext = await _fixture.CreatePageContextAsync();
        var page = pageContext.Page;
        var loginPage = new LoginPage(page);
        var burgerMenuPage = new BurgerMenuPage(page);
        var inventoryPage = new InventoryPage(page);

        bool isOpen;

        await AllureApi.Step("Navigate and login", async () =>
        {
            await loginPage.NavigateToAsync("https://www.saucedemo.com/");
            await loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        await AllureApi.Step("Verify menu accessible from inventory page", async () =>
        {
            await burgerMenuPage.OpenMenuAsync();
            isOpen = await burgerMenuPage.IsMenuOpenAsync();
            Assert.True(isOpen);
            await burgerMenuPage.CloseMenuAsync();
        });

        await AllureApi.Step("Navigate to cart and verify menu accessible", async () =>
        {
            await inventoryPage.NavigateToCartAsync();
            await burgerMenuPage.OpenMenuAsync();
            isOpen = await burgerMenuPage.IsMenuOpenAsync();
            Assert.True(isOpen);
            await burgerMenuPage.CloseMenuAsync();
        });

        await AllureApi.Step("Navigate to product details and verify menu accessible", async () =>
        {
            await burgerMenuPage.ClickAllItemsAsync();
            await inventoryPage.ClickProductAsync("Sauce Labs Backpack");
            await burgerMenuPage.OpenMenuAsync();
            isOpen = await burgerMenuPage.IsMenuOpenAsync();
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
        await using var pageContext = await _fixture.CreatePageContextAsync();
        var page = pageContext.Page;
        var loginPage = new LoginPage(page);
        var burgerMenuPage = new BurgerMenuPage(page);

        await AllureApi.Step("Navigate and login", async () =>
        {
            await loginPage.NavigateToAsync("https://www.saucedemo.com/");
            await loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        await AllureApi.Step("Open burger menu", async () =>
        {
            await burgerMenuPage.OpenMenuAsync();
        });

        await AllureApi.Step("Click About link", async () =>
        {
            await burgerMenuPage.ClickAboutAsync();
        });

        await AllureApi.Step("Verify navigated to Sauce Labs website", async () =>
        {
            await page.WaitForURLAsync("https://saucelabs.com/**");
            Assert.Contains("saucelabs.com", page.Url);
        });
    }
}
