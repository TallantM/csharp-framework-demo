using Microsoft.Playwright;
using Xunit;
using csharp_framework_demo.Utilities.PageObjects;
using Allure.Net.Commons;
using Allure.Xunit.Attributes;

namespace csharp_framework_demo.Tests;

[AllureSuite("Integration Tests")]
[AllureFeature("Product Details Page Object")]
public class ProductDetailsTests : IClassFixture<PlaywrightFixture>
{
    private readonly PlaywrightFixture _fixture;

    public ProductDetailsTests(PlaywrightFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    [AllureDescription("Verifies that GetProductNameAsync returns the product name from details page")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "ProductDetails")]
    public async Task GetProductName_ReturnsCorrectName()
    {
        await using var pageContext = await _fixture.CreatePageContextAsync();
        var page = pageContext.Page;
        var loginPage = new LoginPage(page);
        var inventoryPage = new InventoryPage(page);
        var cartPage = new CartPage(page);
        var checkoutPage = new CheckoutPage(page);
        var burgerMenuPage = new BurgerMenuPage(page);
        var productDetailsPage = new ProductDetailsPage(page);

        await AllureApi.Step("Navigate to product details", async () =>
        {
            await loginPage.NavigateToAsync("https://www.saucedemo.com/");
            await loginPage.LoginAsync("standard_user", "secret_sauce");
            await inventoryPage.ClickProductAsync("Sauce Labs Backpack");
        });

        string name = null!;
        await AllureApi.Step("Get product name", async () =>
        {
            name = await productDetailsPage.GetProductNameAsync();
        });

        AllureApi.Step("Verify product name", () =>
        {
            Assert.Equal("Sauce Labs Backpack", name);
        });
    }

    [Fact]
    [AllureDescription("Verifies that product description is displayed on details page")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "ProductDetails")]
    public async Task GetProductDescription_ReturnsDescription()
    {
        await using var pageContext = await _fixture.CreatePageContextAsync();
        var page = pageContext.Page;
        var loginPage = new LoginPage(page);
        var inventoryPage = new InventoryPage(page);
        var cartPage = new CartPage(page);
        var checkoutPage = new CheckoutPage(page);
        var burgerMenuPage = new BurgerMenuPage(page);
        var productDetailsPage = new ProductDetailsPage(page);

        await AllureApi.Step("Navigate to product details", async () =>
        {
            await loginPage.NavigateToAsync("https://www.saucedemo.com/");
            await loginPage.LoginAsync("standard_user", "secret_sauce");
            await inventoryPage.ClickProductAsync("Sauce Labs Backpack");
        });

        string description = null!;
        await AllureApi.Step("Get product description", async () =>
        {
            description = await productDetailsPage.GetProductDescriptionAsync();
        });

        AllureApi.Step("Verify description is not empty", () =>
        {
            Assert.False(string.IsNullOrEmpty(description));
        });
    }

    [Fact]
    [AllureDescription("Verifies that AddToCartAsync adds product and updates cart badge")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "Cart")]
    public async Task AddToCart_UpdatesCartBadge()
    {
        await using var pageContext = await _fixture.CreatePageContextAsync();
        var page = pageContext.Page;
        var loginPage = new LoginPage(page);
        var inventoryPage = new InventoryPage(page);
        var cartPage = new CartPage(page);
        var checkoutPage = new CheckoutPage(page);
        var burgerMenuPage = new BurgerMenuPage(page);
        var productDetailsPage = new ProductDetailsPage(page);

        await AllureApi.Step("Navigate to product details", async () =>
        {
            await loginPage.NavigateToAsync("https://www.saucedemo.com/");
            await loginPage.LoginAsync("standard_user", "secret_sauce");
            await inventoryPage.ClickProductAsync("Sauce Labs Backpack");
        });

        await AllureApi.Step("Add product to cart", async () =>
        {
            await productDetailsPage.AddToCartAsync();
        });

        int count = 0;
        await AllureApi.Step("Verify cart badge shows 1", async () =>
        {
            count = await productDetailsPage.GetCartItemCountAsync();
            Assert.Equal(1, count);
        });

        await AllureApi.Step("Verify product is in cart", async () =>
        {
            var isInCart = await productDetailsPage.IsProductInCartAsync();
            Assert.True(isInCart);
        });
    }

    [Fact]
    [AllureDescription("Verifies that RemoveFromCartAsync removes product and updates badge")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "Cart")]
    public async Task RemoveFromCart_UpdatesBadge()
    {
        await using var pageContext = await _fixture.CreatePageContextAsync();
        var page = pageContext.Page;
        var loginPage = new LoginPage(page);
        var inventoryPage = new InventoryPage(page);
        var cartPage = new CartPage(page);
        var checkoutPage = new CheckoutPage(page);
        var burgerMenuPage = new BurgerMenuPage(page);
        var productDetailsPage = new ProductDetailsPage(page);

        await AllureApi.Step("Navigate to product details and add to cart", async () =>
        {
            await loginPage.NavigateToAsync("https://www.saucedemo.com/");
            await loginPage.LoginAsync("standard_user", "secret_sauce");
            await inventoryPage.ClickProductAsync("Sauce Labs Backpack");
            await productDetailsPage.AddToCartAsync();
        });

        await AllureApi.Step("Remove product from cart", async () =>
        {
            await productDetailsPage.RemoveFromCartAsync();
        });

        int count = 0;
        await AllureApi.Step("Verify cart badge is 0", async () =>
        {
            count = await productDetailsPage.GetCartItemCountAsync();
            Assert.Equal(0, count);
        });

        await AllureApi.Step("Verify product is not in cart", async () =>
        {
            var isInCart = await productDetailsPage.IsProductInCartAsync();
            Assert.False(isInCart);
        });
    }

    [Fact]
    [AllureDescription("Verifies that ClickBackToProductsAsync navigates back to inventory")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "Navigation")]
    public async Task ClickBackToProducts_NavigatesToInventory()
    {
        await using var pageContext = await _fixture.CreatePageContextAsync();
        var page = pageContext.Page;
        var loginPage = new LoginPage(page);
        var inventoryPage = new InventoryPage(page);
        var cartPage = new CartPage(page);
        var checkoutPage = new CheckoutPage(page);
        var burgerMenuPage = new BurgerMenuPage(page);
        var productDetailsPage = new ProductDetailsPage(page);

        await AllureApi.Step("Navigate to product details", async () =>
        {
            await loginPage.NavigateToAsync("https://www.saucedemo.com/");
            await loginPage.LoginAsync("standard_user", "secret_sauce");
            await inventoryPage.ClickProductAsync("Sauce Labs Backpack");
        });

        await AllureApi.Step("Click back to products", async () =>
        {
            await productDetailsPage.ClickBackToProductsAsync();
        });

        AllureApi.Step("Verify URL is inventory page", () =>
        {
            Assert.Equal("https://www.saucedemo.com/inventory.html", page.Url);
        });
    }

    [Fact]
    [AllureDescription("Verifies that product price is displayed correctly")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "ProductDetails")]
    public async Task GetProductPrice_ReturnsCorrectPrice()
    {
        await using var pageContext = await _fixture.CreatePageContextAsync();
        var page = pageContext.Page;
        var loginPage = new LoginPage(page);
        var inventoryPage = new InventoryPage(page);
        var cartPage = new CartPage(page);
        var checkoutPage = new CheckoutPage(page);
        var burgerMenuPage = new BurgerMenuPage(page);
        var productDetailsPage = new ProductDetailsPage(page);

        await AllureApi.Step("Navigate to product details", async () =>
        {
            await loginPage.NavigateToAsync("https://www.saucedemo.com/");
            await loginPage.LoginAsync("standard_user", "secret_sauce");
            await inventoryPage.ClickProductAsync("Sauce Labs Backpack");
        });

        string price = null!;
        await AllureApi.Step("Get product price", async () =>
        {
            price = await productDetailsPage.GetProductPriceAsync();
        });

        AllureApi.Step("Verify price format", () =>
        {
            Assert.Contains("$", price);
        });
    }

    [Fact]
    [AllureDescription("Verifies that product image is visible on details page")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "ProductDetails")]
    public async Task IsImageVisible_ReturnsTrue()
    {
        await using var pageContext = await _fixture.CreatePageContextAsync();
        var page = pageContext.Page;
        var loginPage = new LoginPage(page);
        var inventoryPage = new InventoryPage(page);
        var cartPage = new CartPage(page);
        var checkoutPage = new CheckoutPage(page);
        var burgerMenuPage = new BurgerMenuPage(page);
        var productDetailsPage = new ProductDetailsPage(page);

        await AllureApi.Step("Navigate to product details", async () =>
        {
            await loginPage.NavigateToAsync("https://www.saucedemo.com/");
            await loginPage.LoginAsync("standard_user", "secret_sauce");
            await inventoryPage.ClickProductAsync("Sauce Labs Backpack");
        });

        bool isVisible = false;
        await AllureApi.Step("Check if image is visible", async () =>
        {
            isVisible = await productDetailsPage.IsImageVisibleAsync();
        });

        AllureApi.Step("Verify image is visible", () =>
        {
            Assert.True(isVisible);
        });
    }
}
