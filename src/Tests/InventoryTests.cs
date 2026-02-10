using Microsoft.Playwright;
using Xunit;
using csharp_framework_demo.Utilities.PageObjects;
using Allure.Net.Commons;
using Allure.Xunit.Attributes;

namespace csharp_framework_demo.Tests;

[AllureSuite("Integration Tests")]
[AllureFeature("Inventory Page Object")]
public class InventoryTests : IClassFixture<PlaywrightFixture>
{
    private readonly IPage _page;
    private readonly LoginPage _loginPage;
    private readonly InventoryPage _inventoryPage;

    public InventoryTests(PlaywrightFixture fixture)
    {
        _page = fixture.Page;
        _loginPage = new LoginPage(_page);
        _inventoryPage = new InventoryPage(_page);
    }

    [Fact]
    [AllureDescription("Verifies that GetProductCountAsync returns the correct number of products displayed")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "Inventory")]
    public async Task GetProductCount_ReturnsCorrectNumber()
    {
        await AllureApi.Step("Navigate to SauceDemo login page", async () =>
        {
            await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Login with valid credentials", async () =>
        {
            await _loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        int count = 0;
        await AllureApi.Step("Get product count", async () =>
        {
            count = await _inventoryPage.GetProductCountAsync();
        });

        AllureApi.Step("Verify product count is 6", () =>
        {
            Assert.Equal(6, count);
        });
    }

    [Fact]
    [AllureDescription("Verifies that GetProductNamesAsync retrieves all visible product names")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "Inventory")]
    public async Task GetProductNames_ReturnsAllNames()
    {
        await AllureApi.Step("Navigate to SauceDemo login page", async () =>
        {
            await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Login with valid credentials", async () =>
        {
            await _loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        List<string> names = null!;
        await AllureApi.Step("Get product names", async () =>
        {
            names = await _inventoryPage.GetProductNamesAsync();
        });

        AllureApi.Step("Verify all 6 product names are returned", () =>
        {
            Assert.Equal(6, names.Count);
            Assert.Contains("Sauce Labs Backpack", names);
            Assert.Contains("Sauce Labs Bike Light", names);
        });
    }

    [Fact]
    [AllureDescription("Verifies that adding a product to cart updates the cart badge")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "Cart")]
    public async Task AddToCart_UpdatesBadgeCount()
    {
        await AllureApi.Step("Navigate to SauceDemo login page", async () =>
        {
            await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Login with valid credentials", async () =>
        {
            await _loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        await AllureApi.Step("Add product to cart", async () =>
        {
            await _inventoryPage.AddToCartAsync("sauce-labs-backpack");
        });

        int cartCount = 0;
        await AllureApi.Step("Get cart item count", async () =>
        {
            cartCount = await _inventoryPage.GetCartItemCountAsync();
        });

        AllureApi.Step("Verify cart badge shows 1", () =>
        {
            Assert.Equal(1, cartCount);
        });

        await AllureApi.Step("Verify button changed to Remove", async () =>
        {
            var isInCart = await _inventoryPage.IsProductInCartAsync("sauce-labs-backpack");
            Assert.True(isInCart);
        });
    }

    [Fact]
    [AllureDescription("Verifies that removing a product from cart updates the cart badge")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "Cart")]
    public async Task RemoveFromCart_UpdatesBadgeCount()
    {
        await AllureApi.Step("Navigate to SauceDemo login page", async () =>
        {
            await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Login with valid credentials", async () =>
        {
            await _loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        await AllureApi.Step("Add product to cart", async () =>
        {
            await _inventoryPage.AddToCartAsync("sauce-labs-backpack");
        });

        await AllureApi.Step("Remove product from cart", async () =>
        {
            await _inventoryPage.RemoveFromCartAsync("sauce-labs-backpack");
        });

        int cartCount = 0;
        await AllureApi.Step("Get cart item count", async () =>
        {
            cartCount = await _inventoryPage.GetCartItemCountAsync();
        });

        AllureApi.Step("Verify cart badge is 0", () =>
        {
            Assert.Equal(0, cartCount);
        });

        await AllureApi.Step("Verify button changed back to Add to Cart", async () =>
        {
            var isInCart = await _inventoryPage.IsProductInCartAsync("sauce-labs-backpack");
            Assert.False(isInCart);
        });
    }

    [Fact]
    [AllureDescription("Verifies that clicking a product name navigates to product detail page")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "Navigation")]
    public async Task ClickProduct_NavigatesToDetails()
    {
        await AllureApi.Step("Navigate to SauceDemo login page", async () =>
        {
            await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Login with valid credentials", async () =>
        {
            await _loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        await AllureApi.Step("Click product name", async () =>
        {
            await _inventoryPage.ClickProductAsync("Sauce Labs Backpack");
        });

        AllureApi.Step("Verify URL contains inventory-item.html", () =>
        {
            Assert.Contains("inventory-item.html?id=", _page.Url);
        });
    }

    [Fact]
    [AllureDescription("Verifies that NavigateToCartAsync navigates to the cart page")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "Navigation")]
    public async Task NavigateToCart_OpensCartPage()
    {
        await AllureApi.Step("Navigate to SauceDemo login page", async () =>
        {
            await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Login with valid credentials", async () =>
        {
            await _loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        await AllureApi.Step("Navigate to cart", async () =>
        {
            await _inventoryPage.NavigateToCartAsync();
        });

        AllureApi.Step("Verify URL is cart page", () =>
        {
            Assert.Equal("https://www.saucedemo.com/cart.html", _page.Url);
        });
    }

    [Fact]
    [AllureDescription("Verifies that SortProductsAsync reorders products based on selected option")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "Sorting")]
    public async Task SortProducts_ChangesDisplayOrder()
    {
        await AllureApi.Step("Navigate to SauceDemo login page", async () =>
        {
            await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Login with valid credentials", async () =>
        {
            await _loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        List<string> initialNames = null!;
        await AllureApi.Step("Get initial product order", async () =>
        {
            initialNames = await _inventoryPage.GetProductNamesAsync();
        });

        await AllureApi.Step("Sort products by price low to high", async () =>
        {
            await _inventoryPage.SortProductsAsync("lohi");
        });

        List<string> sortedNames = null!;
        await AllureApi.Step("Get sorted product order", async () =>
        {
            sortedNames = await _inventoryPage.GetProductNamesAsync();
        });

        AllureApi.Step("Verify products are reordered", () =>
        {
            Assert.NotEqual(initialNames, sortedNames);
        });
    }

    [Fact]
    [AllureDescription("Verifies that IsProductInCartAsync correctly identifies products in cart")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "Cart")]
    public async Task IsProductInCart_ReturnsTrueAfterAdding()
    {
        await AllureApi.Step("Navigate to SauceDemo login page", async () =>
        {
            await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Login with valid credentials", async () =>
        {
            await _loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        bool beforeAdding = false;
        await AllureApi.Step("Check if product is in cart before adding", async () =>
        {
            beforeAdding = await _inventoryPage.IsProductInCartAsync("sauce-labs-backpack");
        });

        AllureApi.Step("Verify product is not in cart initially", () =>
        {
            Assert.False(beforeAdding);
        });

        await AllureApi.Step("Add product to cart", async () =>
        {
            await _inventoryPage.AddToCartAsync("sauce-labs-backpack");
        });

        bool afterAdding = false;
        await AllureApi.Step("Check if product is in cart after adding", async () =>
        {
            afterAdding = await _inventoryPage.IsProductInCartAsync("sauce-labs-backpack");
        });

        AllureApi.Step("Verify product is in cart after adding", () =>
        {
            Assert.True(afterAdding);
        });
    }
}
