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
    private readonly IPage _page;
    private readonly LoginPage _loginPage;
    private readonly InventoryPage _inventoryPage;
    private readonly ProductDetailsPage _productDetailsPage;

    public ProductDetailsTests(PlaywrightFixture fixture)
    {
        _page = fixture.Page;
        _loginPage = new LoginPage(_page);
        _inventoryPage = new InventoryPage(_page);
        _productDetailsPage = new ProductDetailsPage(_page);
    }

    private async Task NavigateToProductDetailsAsync()
    {
        await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        await _loginPage.LoginAsync("standard_user", "secret_sauce");
        await _inventoryPage.ClickProductAsync("Sauce Labs Backpack");
    }

    [Fact]
    [AllureDescription("Verifies that GetProductNameAsync returns the product name from details page")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "ProductDetails")]
    public async Task GetProductName_ReturnsCorrectName()
    {
        await AllureApi.Step("Navigate to product details", async () => await NavigateToProductDetailsAsync());

        string name = null!;
        await AllureApi.Step("Get product name", async () =>
        {
            name = await _productDetailsPage.GetProductNameAsync();
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
        await AllureApi.Step("Navigate to product details", async () => await NavigateToProductDetailsAsync());

        string description = null!;
        await AllureApi.Step("Get product description", async () =>
        {
            description = await _productDetailsPage.GetProductDescriptionAsync();
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
        await AllureApi.Step("Navigate to product details", async () => await NavigateToProductDetailsAsync());

        await AllureApi.Step("Add product to cart", async () =>
        {
            await _productDetailsPage.AddToCartAsync();
        });

        int count = 0;
        await AllureApi.Step("Verify cart badge shows 1", async () =>
        {
            count = await _productDetailsPage.GetCartItemCountAsync();
            Assert.Equal(1, count);
        });

        await AllureApi.Step("Verify product is in cart", async () =>
        {
            var isInCart = await _productDetailsPage.IsProductInCartAsync();
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
        await AllureApi.Step("Navigate to product details and add to cart", async () =>
        {
            await NavigateToProductDetailsAsync();
            await _productDetailsPage.AddToCartAsync();
        });

        await AllureApi.Step("Remove product from cart", async () =>
        {
            await _productDetailsPage.RemoveFromCartAsync();
        });

        int count = 0;
        await AllureApi.Step("Verify cart badge is 0", async () =>
        {
            count = await _productDetailsPage.GetCartItemCountAsync();
            Assert.Equal(0, count);
        });

        await AllureApi.Step("Verify product is not in cart", async () =>
        {
            var isInCart = await _productDetailsPage.IsProductInCartAsync();
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
        await AllureApi.Step("Navigate to product details", async () => await NavigateToProductDetailsAsync());

        await AllureApi.Step("Click back to products", async () =>
        {
            await _productDetailsPage.ClickBackToProductsAsync();
        });

        AllureApi.Step("Verify URL is inventory page", () =>
        {
            Assert.Equal("https://www.saucedemo.com/inventory.html", _page.Url);
        });
    }

    [Fact]
    [AllureDescription("Verifies that product price is displayed correctly")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "ProductDetails")]
    public async Task GetProductPrice_ReturnsCorrectPrice()
    {
        await AllureApi.Step("Navigate to product details", async () => await NavigateToProductDetailsAsync());

        string price = null!;
        await AllureApi.Step("Get product price", async () =>
        {
            price = await _productDetailsPage.GetProductPriceAsync();
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
        await AllureApi.Step("Navigate to product details", async () => await NavigateToProductDetailsAsync());

        bool isVisible = false;
        await AllureApi.Step("Check if image is visible", async () =>
        {
            isVisible = await _productDetailsPage.IsImageVisibleAsync();
        });

        AllureApi.Step("Verify image is visible", () =>
        {
            Assert.True(isVisible);
        });
    }
}
