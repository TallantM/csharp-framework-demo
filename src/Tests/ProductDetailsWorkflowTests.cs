using Microsoft.Playwright;
using Xunit;
using csharp_framework_demo.Utilities.PageObjects;
using Allure.Net.Commons;
using Allure.Xunit.Attributes;

namespace csharp_framework_demo.Tests;

[AllureSuite("End-to-End Tests")]
[AllureFeature("Product Details")]
public class ProductDetailsWorkflowTests : IClassFixture<PlaywrightFixture>
{
    private readonly PlaywrightFixture _fixture;

    public ProductDetailsWorkflowTests(PlaywrightFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    [AllureDescription("Verifies that user can view detailed product information from inventory")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Smoke", "ProductDetails")]
    public async Task ViewProductDetailsFromInventory()
    {
        await using var pageContext = await _fixture.CreatePageContextAsync();
        var page = pageContext.Page;
        var loginPage = new LoginPage(page);
        var inventoryPage = new InventoryPage(page);
        var cartPage = new CartPage(page);
        var checkoutPage = new CheckoutPage(page);
        var burgerMenuPage = new BurgerMenuPage(page);
        var productDetailsPage = new ProductDetailsPage(page);

        await AllureApi.Step("Navigate to login page", async () =>
        {
            await loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Login to application", async () =>
        {
            await loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        await AllureApi.Step("Click product from inventory", async () =>
        {
            await inventoryPage.ClickProductAsync("Sauce Labs Backpack");
        });

        await AllureApi.Step("Verify product details page loads", async () =>
        {
            Assert.Contains("inventory-item.html", page.Url);
        });

        await AllureApi.Step("Verify product information is displayed", async () =>
        {
            var name = await productDetailsPage.GetProductNameAsync();
            var description = await productDetailsPage.GetProductDescriptionAsync();
            var price = await productDetailsPage.GetProductPriceAsync();
            var imageVisible = await productDetailsPage.IsImageVisibleAsync();

            Assert.Equal("Sauce Labs Backpack", name);
            Assert.False(string.IsNullOrEmpty(description));
            Assert.Contains("$", price);
            Assert.True(imageVisible);
        });
    }

    [Fact]
    [AllureDescription("Verifies that user can add product from details page and cart updates")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Smoke", "Cart")]
    public async Task AddProductFromDetailsPage()
    {
        await using var pageContext = await _fixture.CreatePageContextAsync();
        var page = pageContext.Page;
        var loginPage = new LoginPage(page);
        var inventoryPage = new InventoryPage(page);
        var cartPage = new CartPage(page);
        var checkoutPage = new CheckoutPage(page);
        var burgerMenuPage = new BurgerMenuPage(page);
        var productDetailsPage = new ProductDetailsPage(page);

        await AllureApi.Step("Navigate and login", async () =>
        {
            await loginPage.NavigateToAsync("https://www.saucedemo.com/");
            await loginPage.LoginAsync("standard_user", "secret_sauce");
            await inventoryPage.ClickProductAsync("Sauce Labs Backpack");
        });

        await AllureApi.Step("Add product to cart from details page", async () =>
        {
            await productDetailsPage.AddToCartAsync();
        });

        await AllureApi.Step("Verify cart badge shows 1", async () =>
        {
            var count = await productDetailsPage.GetCartItemCountAsync();
            Assert.Equal(1, count);
        });

        await AllureApi.Step("Verify button changes to Remove", async () =>
        {
            var isInCart = await productDetailsPage.IsProductInCartAsync();
            Assert.True(isInCart);
        });
    }

    [Fact]
    [AllureDescription("Verifies that user can remove product from details page")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Regression", "Cart")]
    public async Task RemoveProductFromDetailsPage()
    {
        await using var pageContext = await _fixture.CreatePageContextAsync();
        var page = pageContext.Page;
        var loginPage = new LoginPage(page);
        var inventoryPage = new InventoryPage(page);
        var cartPage = new CartPage(page);
        var checkoutPage = new CheckoutPage(page);
        var burgerMenuPage = new BurgerMenuPage(page);
        var productDetailsPage = new ProductDetailsPage(page);

        await AllureApi.Step("Navigate, login, and add product", async () =>
        {
            await loginPage.NavigateToAsync("https://www.saucedemo.com/");
            await loginPage.LoginAsync("standard_user", "secret_sauce");
            await inventoryPage.ClickProductAsync("Sauce Labs Backpack");
            await productDetailsPage.AddToCartAsync();
        });

        await AllureApi.Step("Remove product from details page", async () =>
        {
            await productDetailsPage.RemoveFromCartAsync();
        });

        await AllureApi.Step("Verify cart badge is 0", async () =>
        {
            var count = await productDetailsPage.GetCartItemCountAsync();
            Assert.Equal(0, count);
        });

        await AllureApi.Step("Verify button changes back to Add to Cart", async () =>
        {
            var isInCart = await productDetailsPage.IsProductInCartAsync();
            Assert.False(isInCart);
        });
    }

    [Fact]
    [AllureDescription("Verifies that user can navigate back to inventory from details page")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Regression", "Navigation")]
    public async Task NavigateBackToInventory()
    {
        await using var pageContext = await _fixture.CreatePageContextAsync();
        var page = pageContext.Page;
        var loginPage = new LoginPage(page);
        var inventoryPage = new InventoryPage(page);
        var cartPage = new CartPage(page);
        var checkoutPage = new CheckoutPage(page);
        var burgerMenuPage = new BurgerMenuPage(page);
        var productDetailsPage = new ProductDetailsPage(page);

        await AllureApi.Step("Navigate and login", async () =>
        {
            await loginPage.NavigateToAsync("https://www.saucedemo.com/");
            await loginPage.LoginAsync("standard_user", "secret_sauce");
            await inventoryPage.ClickProductAsync("Sauce Labs Backpack");
        });

        await AllureApi.Step("Click back to products button", async () =>
        {
            await productDetailsPage.ClickBackToProductsAsync();
        });

        await AllureApi.Step("Verify returned to inventory page", async () =>
        {
            await Assertions.Expect(page).ToHaveURLAsync("https://www.saucedemo.com/inventory.html");
        });

        await AllureApi.Step("Verify inventory list is visible", async () =>
        {
            await Assertions.Expect(page.Locator(".inventory_list")).ToBeVisibleAsync();
        });
    }

    [Fact]
    [AllureDescription("Verifies that cart state persists when viewing different product details")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Regression", "Cart")]
    public async Task CartPersistsAcrossProductDetails()
    {
        await using var pageContext = await _fixture.CreatePageContextAsync();
        var page = pageContext.Page;
        var loginPage = new LoginPage(page);
        var inventoryPage = new InventoryPage(page);
        var cartPage = new CartPage(page);
        var checkoutPage = new CheckoutPage(page);
        var burgerMenuPage = new BurgerMenuPage(page);
        var productDetailsPage = new ProductDetailsPage(page);

        await AllureApi.Step("Navigate and login", async () =>
        {
            await loginPage.NavigateToAsync("https://www.saucedemo.com/");
            await loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        await AllureApi.Step("Add first product from details", async () =>
        {
            await inventoryPage.ClickProductAsync("Sauce Labs Backpack");
            await productDetailsPage.AddToCartAsync();
        });

        await AllureApi.Step("Return to inventory", async () =>
        {
            await productDetailsPage.ClickBackToProductsAsync();
        });

        await AllureApi.Step("View second product details", async () =>
        {
            await inventoryPage.ClickProductAsync("Sauce Labs Bike Light");
        });

        await AllureApi.Step("Verify cart still shows 1 item", async () =>
        {
            var count = await productDetailsPage.GetCartItemCountAsync();
            Assert.Equal(1, count);
        });

        await AllureApi.Step("Add second product", async () =>
        {
            await productDetailsPage.AddToCartAsync();
        });

        await AllureApi.Step("Verify cart now shows 2 items", async () =>
        {
            var count = await productDetailsPage.GetCartItemCountAsync();
            Assert.Equal(2, count);
        });
    }
}
