using Microsoft.Playwright;
using Xunit;
using csharp_framework_demo.Utilities.PageObjects;
using Allure.Net.Commons;
using Allure.Xunit.Attributes;

namespace csharp_framework_demo.Tests;

[AllureSuite("Integration Tests")]
[AllureFeature("Cart Page Object")]
public class CartTests : IClassFixture<PlaywrightFixture>
{
    private readonly PlaywrightFixture _fixture;

    public CartTests(PlaywrightFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    [AllureDescription("Verifies that GetCartItemCountAsync returns the correct number of items in cart")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "Cart")]
    public async Task GetCartItemCount_ReturnsCorrectNumber()
    {
        await using var pageContext = await _fixture.CreatePageContextAsync();
        var page = pageContext.Page;
        var loginPage = new LoginPage(page);
        var inventoryPage = new InventoryPage(page);
        var cartPage = new CartPage(page);

        await AllureApi.Step("Navigate and login", async () =>
        {
            await loginPage.NavigateToAsync("https://www.saucedemo.com/");
            await loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        await AllureApi.Step("Add 2 products to cart", async () =>
        {
            await inventoryPage.AddToCartAsync("sauce-labs-backpack");
            await inventoryPage.AddToCartAsync("sauce-labs-bike-light");
        });

        await AllureApi.Step("Navigate to cart page", async () =>
        {
            await inventoryPage.NavigateToCartAsync();
        });

        int count = 0;
        await AllureApi.Step("Get cart item count", async () =>
        {
            count = await cartPage.GetCartItemCountAsync();
        });

        AllureApi.Step("Verify count is 2", () =>
        {
            Assert.Equal(2, count);
        });
    }

    [Fact]
    [AllureDescription("Verifies that GetCartItemNamesAsync retrieves all product names in cart")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "Cart")]
    public async Task GetCartItemNames_ReturnsAllNames()
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

        await AllureApi.Step("Add specific products to cart", async () =>
        {
            await inventoryPage.AddToCartAsync("sauce-labs-backpack");
            await inventoryPage.AddToCartAsync("sauce-labs-bike-light");
        });

        await AllureApi.Step("Navigate to cart page", async () =>
        {
            await inventoryPage.NavigateToCartAsync();
        });

        List<string> names = null!;
        await AllureApi.Step("Get cart item names", async () =>
        {
            names = await cartPage.GetCartItemNamesAsync();
        });

        AllureApi.Step("Verify list contains expected names", () =>
        {
            Assert.Contains("Sauce Labs Backpack", names);
            Assert.Contains("Sauce Labs Bike Light", names);
        });
    }

    [Fact]
    [AllureDescription("Verifies that RemoveItemAsync removes the product and updates the display")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "Cart")]
    public async Task RemoveItem_RemovesProductFromCart()
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

        await AllureApi.Step("Add 2 products to cart", async () =>
        {
            await inventoryPage.AddToCartAsync("sauce-labs-backpack");
            await inventoryPage.AddToCartAsync("sauce-labs-bike-light");
        });

        await AllureApi.Step("Navigate to cart page", async () =>
        {
            await inventoryPage.NavigateToCartAsync();
        });

        await AllureApi.Step("Remove one product", async () =>
        {
            await cartPage.RemoveItemAsync("sauce-labs-backpack");
        });

        int count = 0;
        await AllureApi.Step("Verify cart count is 1", async () =>
        {
            count = await cartPage.GetCartItemCountAsync();
            Assert.Equal(1, count);
        });

        await AllureApi.Step("Verify product no longer in cart", async () =>
        {
            var isInCart = await cartPage.IsItemInCartAsync("Sauce Labs Backpack");
            Assert.False(isInCart);
        });
    }

    [Fact]
    [AllureDescription("Verifies that IsItemInCartAsync correctly identifies items in the cart")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "Cart")]
    public async Task IsItemInCart_ReturnsTrueForAddedItems()
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

        await AllureApi.Step("Add product to cart", async () =>
        {
            await inventoryPage.AddToCartAsync("sauce-labs-backpack");
        });

        await AllureApi.Step("Navigate to cart page", async () =>
        {
            await inventoryPage.NavigateToCartAsync();
        });

        bool isInCart = false;
        await AllureApi.Step("Check if product is in cart", async () =>
        {
            isInCart = await cartPage.IsItemInCartAsync("Sauce Labs Backpack");
        });

        AllureApi.Step("Verify product is in cart", () =>
        {
            Assert.True(isInCart);
        });
    }

    [Fact]
    [AllureDescription("Verifies that GetItemPriceAsync returns the correct price for a cart item")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "Cart")]
    public async Task GetItemPrice_ReturnsCorrectPrice()
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

        await AllureApi.Step("Add product to cart", async () =>
        {
            await inventoryPage.AddToCartAsync("sauce-labs-backpack");
        });

        await AllureApi.Step("Navigate to cart page", async () =>
        {
            await inventoryPage.NavigateToCartAsync();
        });

        string price = null!;
        await AllureApi.Step("Get item price", async () =>
        {
            price = await cartPage.GetItemPriceAsync("Sauce Labs Backpack");
        });

        AllureApi.Step("Verify price is $29.99", () =>
        {
            Assert.Equal("$29.99", price);
        });
    }

    [Fact]
    [AllureDescription("Verifies that ClickContinueShoppingAsync navigates back to inventory page")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "Navigation")]
    public async Task ClickContinueShopping_NavigatesToInventory()
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

        await AllureApi.Step("Navigate to cart page", async () =>
        {
            await inventoryPage.NavigateToCartAsync();
        });

        await AllureApi.Step("Click continue shopping", async () =>
        {
            await cartPage.ClickContinueShoppingAsync();
        });

        AllureApi.Step("Verify URL is inventory page", () =>
        {
            Assert.Equal("https://www.saucedemo.com/inventory.html", page.Url);
        });
    }

    [Fact]
    [AllureDescription("Verifies that ClickCheckoutAsync navigates to checkout page")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "Checkout")]
    public async Task ClickCheckout_NavigatesToCheckoutForm()
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

        await AllureApi.Step("Add item to cart", async () =>
        {
            await inventoryPage.AddToCartAsync("sauce-labs-backpack");
        });

        await AllureApi.Step("Navigate to cart", async () =>
        {
            await inventoryPage.NavigateToCartAsync();
        });

        await AllureApi.Step("Click checkout", async () =>
        {
            await cartPage.ClickCheckoutAsync();
        });

        AllureApi.Step("Verify URL is checkout step one", () =>
        {
            Assert.Equal("https://www.saucedemo.com/checkout-step-one.html", page.Url);
        });
    }

    [Fact]
    [AllureDescription("Verifies that IsCartEmptyAsync correctly identifies empty cart state")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "Cart")]
    public async Task IsCartEmpty_ReturnsTrueForEmptyCart()
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

        await AllureApi.Step("Navigate to cart with no items", async () =>
        {
            await inventoryPage.NavigateToCartAsync();
        });

        bool isEmpty = false;
        await AllureApi.Step("Check if cart is empty", async () =>
        {
            isEmpty = await cartPage.IsCartEmptyAsync();
        });

        AllureApi.Step("Verify cart is empty", () =>
        {
            Assert.True(isEmpty);
        });
    }

    [Fact]
    [AllureDescription("Verifies that ClickProductNameAsync navigates to product detail page")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "Navigation")]
    public async Task ClickProductName_NavigatesToDetails()
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

        await AllureApi.Step("Add product and navigate to cart", async () =>
        {
            await inventoryPage.AddToCartAsync("sauce-labs-backpack");
            await inventoryPage.NavigateToCartAsync();
        });

        await AllureApi.Step("Click product name", async () =>
        {
            await cartPage.ClickProductNameAsync("Sauce Labs Backpack");
        });

        AllureApi.Step("Verify URL contains inventory-item.html", () =>
        {
            Assert.Contains("inventory-item.html?id=", page.Url);
        });
    }
}
