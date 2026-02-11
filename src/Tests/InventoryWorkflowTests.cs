using Microsoft.Playwright;
using Xunit;
using csharp_framework_demo.Utilities.PageObjects;
using Allure.Net.Commons;
using Allure.Xunit.Attributes;

namespace csharp_framework_demo.Tests;

[AllureSuite("End-to-End Tests")]
[AllureFeature("Inventory")]
public class InventoryWorkflowTests : IClassFixture<PlaywrightFixture>
{
    private readonly PlaywrightFixture _fixture;

    public InventoryWorkflowTests(PlaywrightFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    [AllureDescription("Verifies that user can log in and view the complete product catalog with all details")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Smoke", "Inventory")]
    public async Task BrowseProductsAfterLogin()
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

        await AllureApi.Step("Login with valid credentials", async () =>
        {
            await loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        await AllureApi.Step("Verify inventory page loads", async () =>
        {
            await Assertions.Expect(page).ToHaveURLAsync("https://www.saucedemo.com/inventory.html");
        });

        int productCount = 0;
        await AllureApi.Step("Verify all products are displayed", async () =>
        {
            productCount = await inventoryPage.GetProductCountAsync();
            Assert.Equal(6, productCount);
        });

        await AllureApi.Step("Verify product details are visible", async () =>
        {
            var names = await inventoryPage.GetProductNamesAsync();
            Assert.All(names, name => Assert.False(string.IsNullOrEmpty(name)));

            var inventoryItems = page.Locator(".inventory_item");
            for (int i = 0; i < productCount; i++)
            {
                var item = inventoryItems.Nth(i);
                await Assertions.Expect(item.Locator(".inventory_item_name")).ToBeVisibleAsync();
                await Assertions.Expect(item.Locator(".inventory_item_price")).ToBeVisibleAsync();
                await Assertions.Expect(item.Locator(".inventory_item_desc")).ToBeVisibleAsync();
            }
        });
    }

    [Fact]
    [AllureDescription("Verifies that user can add a single product to cart and cart badge updates correctly")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Smoke", "Cart")]
    public async Task AddSingleProductToCart()
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

        await AllureApi.Step("Login and navigate to inventory", async () =>
        {
            await loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        await AllureApi.Step("Add a product to cart", async () =>
        {
            await inventoryPage.AddToCartAsync("sauce-labs-backpack");
        });

        int cartCount = 0;
        await AllureApi.Step("Verify cart badge shows 1", async () =>
        {
            cartCount = await inventoryPage.GetCartItemCountAsync();
            Assert.Equal(1, cartCount);
        });

        await AllureApi.Step("Verify button changes to Remove", async () =>
        {
            var isInCart = await inventoryPage.IsProductInCartAsync("sauce-labs-backpack");
            Assert.True(isInCart);
        });

        await AllureApi.Step("Click cart to verify product appears", async () =>
        {
            await inventoryPage.NavigateToCartAsync();
            await Assertions.Expect(page).ToHaveURLAsync("https://www.saucedemo.com/cart.html");

            var cartItem = page.Locator(".cart_item");
            await Assertions.Expect(cartItem).ToBeVisibleAsync();
            await Assertions.Expect(cartItem.Locator(".inventory_item_name")).ToContainTextAsync("Sauce Labs Backpack");
        });
    }

    [Fact]
    [AllureDescription("Verifies that user can add multiple products and cart count reflects total")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Smoke", "Cart")]
    public async Task AddMultipleProductsToCart()
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

        await AllureApi.Step("Login and navigate to inventory", async () =>
        {
            await loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        await AllureApi.Step("Add three different products to cart", async () =>
        {
            await inventoryPage.AddToCartAsync("sauce-labs-backpack");
            await inventoryPage.AddToCartAsync("sauce-labs-bike-light");
            await inventoryPage.AddToCartAsync("sauce-labs-bolt-t-shirt");
        });

        int cartCount = 0;
        await AllureApi.Step("Verify cart badge shows 3", async () =>
        {
            cartCount = await inventoryPage.GetCartItemCountAsync();
            Assert.Equal(3, cartCount);
        });

        await AllureApi.Step("Navigate to cart", async () =>
        {
            await inventoryPage.NavigateToCartAsync();
        });

        await AllureApi.Step("Verify all three products are listed", async () =>
        {
            var cartItems = page.Locator(".cart_item");
            var itemCount = await cartItems.CountAsync();
            Assert.Equal(3, itemCount);

            var itemNames = await page.Locator(".cart_item .inventory_item_name").AllTextContentsAsync();
            Assert.Contains("Sauce Labs Backpack", itemNames);
            Assert.Contains("Sauce Labs Bike Light", itemNames);
            Assert.Contains("Sauce Labs Bolt T-Shirt", itemNames);
        });
    }

    [Fact]
    [AllureDescription("Verifies that user can remove a product from cart and count updates correctly")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Regression", "Cart")]
    public async Task RemoveProductFromCart()
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

        await AllureApi.Step("Login and navigate to inventory", async () =>
        {
            await loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        await AllureApi.Step("Add two products to cart", async () =>
        {
            await inventoryPage.AddToCartAsync("sauce-labs-backpack");
            await inventoryPage.AddToCartAsync("sauce-labs-bike-light");
        });

        await AllureApi.Step("Remove one product", async () =>
        {
            await inventoryPage.RemoveFromCartAsync("sauce-labs-backpack");
        });

        int cartCount = 0;
        await AllureApi.Step("Verify cart badge shows 1", async () =>
        {
            cartCount = await inventoryPage.GetCartItemCountAsync();
            Assert.Equal(1, cartCount);
        });

        await AllureApi.Step("Verify button changes back to Add to Cart", async () =>
        {
            var isInCart = await inventoryPage.IsProductInCartAsync("sauce-labs-backpack");
            Assert.False(isInCart);
        });

        await AllureApi.Step("Verify only Bike Light remains in cart", async () =>
        {
            await inventoryPage.NavigateToCartAsync();
            var cartItems = page.Locator(".cart_item");
            var itemCount = await cartItems.CountAsync();
            Assert.Equal(1, itemCount);

            var itemName = await page.Locator(".cart_item .inventory_item_name").TextContentAsync();
            Assert.Equal("Sauce Labs Bike Light", itemName);
        });
    }

    [Fact]
    [AllureDescription("Verifies that user can click product name to view detailed product information")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Regression", "Navigation")]
    public async Task NavigateToProductDetails()
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

        await AllureApi.Step("Login and navigate to inventory", async () =>
        {
            await loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        await AllureApi.Step("Click on a product name", async () =>
        {
            await inventoryPage.ClickProductAsync("Sauce Labs Backpack");
        });

        await AllureApi.Step("Verify redirected to product detail page", async () =>
        {
            Assert.Contains("inventory-item.html?id=", page.Url);
        });

        await AllureApi.Step("Verify product information is displayed", async () =>
        {
            await Assertions.Expect(page.Locator(".inventory_details_name")).ToContainTextAsync("Sauce Labs Backpack");
            await Assertions.Expect(page.Locator(".inventory_details_desc")).ToBeVisibleAsync();
            await Assertions.Expect(page.Locator(".inventory_details_price")).ToBeVisibleAsync();
        });

        await AllureApi.Step("Verify Back button exists", async () =>
        {
            await Assertions.Expect(page.Locator("[data-test='back-to-products']")).ToBeVisibleAsync();
        });
    }

    [Fact]
    [AllureDescription("Verifies that user can sort products by price from low to high")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Regression", "Sorting")]
    public async Task SortProductsByPriceLowToHigh()
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

        await AllureApi.Step("Login and navigate to inventory", async () =>
        {
            await loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        await AllureApi.Step("Select Price (low to high) from sort dropdown", async () =>
        {
            await inventoryPage.SortProductsAsync("lohi");
        });

        await AllureApi.Step("Verify products are sorted correctly", async () =>
        {
            var prices = await page.Locator(".inventory_item_price").AllTextContentsAsync();
            var priceValues = prices.Select(p => decimal.Parse(p.Replace("$", ""))).ToList();

            var sortedPrices = priceValues.OrderBy(p => p).ToList();
            Assert.Equal(sortedPrices, priceValues);
        });

        await AllureApi.Step("Verify sort dropdown shows Price (low to high)", async () =>
        {
            var selectedValue = await page.Locator(".product_sort_container").InputValueAsync();
            Assert.Equal("lohi", selectedValue);
        });
    }

    [Fact]
    [AllureDescription("Verifies that user can sort products alphabetically in reverse order")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Regression", "Sorting")]
    public async Task SortProductsByNameZToA()
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

        await AllureApi.Step("Login and navigate to inventory", async () =>
        {
            await loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        await AllureApi.Step("Select Name (Z to A) from sort dropdown", async () =>
        {
            await inventoryPage.SortProductsAsync("za");
        });

        await AllureApi.Step("Verify products are sorted reverse alphabetically", async () =>
        {
            var names = await inventoryPage.GetProductNamesAsync();
            var sortedNames = names.OrderByDescending(n => n).ToList();
            Assert.Equal(sortedNames, names);
        });

        await AllureApi.Step("Verify sort dropdown shows Name (Z to A)", async () =>
        {
            var selectedValue = await page.Locator(".product_sort_container").InputValueAsync();
            Assert.Equal("za", selectedValue);
        });
    }

    [Fact]
    [AllureDescription("Verifies that user can add product and proceed to checkout flow")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Smoke", "Checkout")]
    public async Task AddProductAndCheckout()
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

        await AllureApi.Step("Login and navigate to inventory", async () =>
        {
            await loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        await AllureApi.Step("Add a product to cart", async () =>
        {
            await inventoryPage.AddToCartAsync("sauce-labs-backpack");
        });

        await AllureApi.Step("Click cart icon", async () =>
        {
            await inventoryPage.NavigateToCartAsync();
        });

        await AllureApi.Step("Verify product in cart", async () =>
        {
            var cartItem = page.Locator(".cart_item");
            await Assertions.Expect(cartItem).ToBeVisibleAsync();
            await Assertions.Expect(cartItem.Locator(".inventory_item_name")).ToContainTextAsync("Sauce Labs Backpack");
        });

        await AllureApi.Step("Click checkout button", async () =>
        {
            await page.ClickAsync("[data-test='checkout']");
        });

        await AllureApi.Step("Verify checkout form appears", async () =>
        {
            await Assertions.Expect(page).ToHaveURLAsync("https://www.saucedemo.com/checkout-step-one.html");
            await Assertions.Expect(page.Locator("[data-test='firstName']")).ToBeVisibleAsync();
            await Assertions.Expect(page.Locator("[data-test='lastName']")).ToBeVisibleAsync();
            await Assertions.Expect(page.Locator("[data-test='postalCode']")).ToBeVisibleAsync();
        });
    }
}
