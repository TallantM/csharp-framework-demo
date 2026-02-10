using Microsoft.Playwright;
using Xunit;
using csharp_framework_demo.Utilities.PageObjects;
using Allure.Net.Commons;
using Allure.Xunit.Attributes;

namespace csharp_framework_demo.Tests;

[AllureSuite("End-to-End Tests")]
[AllureFeature("Cart")]
public class CartWorkflowTests : IClassFixture<PlaywrightFixture>
{
    private readonly IPage _page;
    private readonly LoginPage _loginPage;
    private readonly InventoryPage _inventoryPage;
    private readonly CartPage _cartPage;

    public CartWorkflowTests(PlaywrightFixture fixture)
    {
        _page = fixture.Page;
        _loginPage = new LoginPage(_page);
        _inventoryPage = new InventoryPage(_page);
        _cartPage = new CartPage(_page);
    }

    [Fact]
    [AllureDescription("Verifies that user can add products and view them in the cart with correct details")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Smoke", "Cart")]
    public async Task ViewCartAfterAddingProducts()
    {
        await AllureApi.Step("Navigate to login page", async () =>
        {
            await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Login and navigate to inventory", async () =>
        {
            await _loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        await AllureApi.Step("Add two products to cart", async () =>
        {
            await _inventoryPage.AddToCartAsync("sauce-labs-backpack");
            await _inventoryPage.AddToCartAsync("sauce-labs-bike-light");
        });

        await AllureApi.Step("Click cart icon", async () =>
        {
            await _inventoryPage.NavigateToCartAsync();
        });

        await AllureApi.Step("Verify both products appear in cart", async () =>
        {
            var names = await _cartPage.GetCartItemNamesAsync();
            Assert.Contains("Sauce Labs Backpack", names);
            Assert.Contains("Sauce Labs Bike Light", names);
        });

        await AllureApi.Step("Verify product details are correct", async () =>
        {
            var backpackPrice = await _cartPage.GetItemPriceAsync("Sauce Labs Backpack");
            Assert.Equal("$29.99", backpackPrice);

            var bikePrice = await _cartPage.GetItemPriceAsync("Sauce Labs Bike Light");
            Assert.Equal("$9.99", bikePrice);
        });

        await AllureApi.Step("Verify cart count shows 2", async () =>
        {
            var count = await _cartPage.GetCartItemCountAsync();
            Assert.Equal(2, count);
        });
    }

    [Fact]
    [AllureDescription("Verifies that user can remove one item from cart and cart updates correctly")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Smoke", "Cart")]
    public async Task RemoveSingleItemFromCart()
    {
        await AllureApi.Step("Navigate to login page", async () =>
        {
            await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Login, add two products, navigate to cart", async () =>
        {
            await _loginPage.LoginAsync("standard_user", "secret_sauce");
            await _inventoryPage.AddToCartAsync("sauce-labs-backpack");
            await _inventoryPage.AddToCartAsync("sauce-labs-bike-light");
            await _inventoryPage.NavigateToCartAsync();
        });

        await AllureApi.Step("Remove one product using Remove button", async () =>
        {
            await _cartPage.RemoveItemAsync("sauce-labs-backpack");
        });

        await AllureApi.Step("Verify removed product disappears", async () =>
        {
            var isInCart = await _cartPage.IsItemInCartAsync("Sauce Labs Backpack");
            Assert.False(isInCart);
        });

        await AllureApi.Step("Verify remaining product still visible", async () =>
        {
            var isInCart = await _cartPage.IsItemInCartAsync("Sauce Labs Bike Light");
            Assert.True(isInCart);
        });

        await AllureApi.Step("Verify cart badge updates to 1", async () =>
        {
            var badgeCount = await _inventoryPage.GetCartItemCountAsync();
            Assert.Equal(1, badgeCount);
        });
    }

    [Fact]
    [AllureDescription("Verifies that user can empty cart by removing all items")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Regression", "Cart")]
    public async Task RemoveAllItemsFromCart()
    {
        await AllureApi.Step("Navigate to login page", async () =>
        {
            await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Login, add two products, navigate to cart", async () =>
        {
            await _loginPage.LoginAsync("standard_user", "secret_sauce");
            await _inventoryPage.AddToCartAsync("sauce-labs-backpack");
            await _inventoryPage.AddToCartAsync("sauce-labs-bike-light");
            await _inventoryPage.NavigateToCartAsync();
        });

        await AllureApi.Step("Remove first product", async () =>
        {
            await _cartPage.RemoveItemAsync("sauce-labs-backpack");
        });

        await AllureApi.Step("Remove second product", async () =>
        {
            await _cartPage.RemoveItemAsync("sauce-labs-bike-light");
        });

        await AllureApi.Step("Verify cart is empty", async () =>
        {
            var isEmpty = await _cartPage.IsCartEmptyAsync();
            Assert.True(isEmpty);
        });

        await AllureApi.Step("Verify cart badge disappears", async () =>
        {
            var badgeCount = await _inventoryPage.GetCartItemCountAsync();
            Assert.Equal(0, badgeCount);
        });
    }

    [Fact]
    [AllureDescription("Verifies that user can return to inventory from cart to add more items")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Regression", "Navigation")]
    public async Task ContinueShoppingFromCart()
    {
        await AllureApi.Step("Navigate to login page", async () =>
        {
            await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Login, add product, navigate to cart", async () =>
        {
            await _loginPage.LoginAsync("standard_user", "secret_sauce");
            await _inventoryPage.AddToCartAsync("sauce-labs-backpack");
            await _inventoryPage.NavigateToCartAsync();
        });

        await AllureApi.Step("Click Continue Shopping button", async () =>
        {
            await _cartPage.ClickContinueShoppingAsync();
        });

        await AllureApi.Step("Verify redirected to inventory page", async () =>
        {
            await Assertions.Expect(_page).ToHaveURLAsync("https://www.saucedemo.com/inventory.html");
        });

        await AllureApi.Step("Verify cart badge still shows item count", async () =>
        {
            var count = await _inventoryPage.GetCartItemCountAsync();
            Assert.Equal(1, count);
        });

        await AllureApi.Step("Add another product", async () =>
        {
            await _inventoryPage.AddToCartAsync("sauce-labs-bike-light");
        });

        await AllureApi.Step("Verify cart badge increments", async () =>
        {
            var count = await _inventoryPage.GetCartItemCountAsync();
            Assert.Equal(2, count);
        });
    }

    [Fact]
    [AllureDescription("Verifies that user with items in cart can proceed to checkout")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Smoke", "Checkout")]
    public async Task ProceedToCheckoutFromCart()
    {
        await AllureApi.Step("Navigate to login page", async () =>
        {
            await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Login, add product, navigate to cart", async () =>
        {
            await _loginPage.LoginAsync("standard_user", "secret_sauce");
            await _inventoryPage.AddToCartAsync("sauce-labs-backpack");
            await _inventoryPage.NavigateToCartAsync();
        });

        await AllureApi.Step("Click Checkout button", async () =>
        {
            await _cartPage.ClickCheckoutAsync();
        });

        await AllureApi.Step("Verify redirected to checkout form", async () =>
        {
            await Assertions.Expect(_page).ToHaveURLAsync("https://www.saucedemo.com/checkout-step-one.html");
        });

        await AllureApi.Step("Verify form fields are visible", async () =>
        {
            await Assertions.Expect(_page.Locator("[data-test='firstName']")).ToBeVisibleAsync();
            await Assertions.Expect(_page.Locator("[data-test='lastName']")).ToBeVisibleAsync();
            await Assertions.Expect(_page.Locator("[data-test='postalCode']")).ToBeVisibleAsync();
            await Assertions.Expect(_page.Locator("[data-test='continue']")).ToBeVisibleAsync();
        });
    }

    [Fact]
    [AllureDescription("Verifies that user can click product name in cart to view details")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Regression", "Navigation")]
    public async Task NavigateToProductDetailsFromCart()
    {
        await AllureApi.Step("Navigate to login page", async () =>
        {
            await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Login, add product, navigate to cart", async () =>
        {
            await _loginPage.LoginAsync("standard_user", "secret_sauce");
            await _inventoryPage.AddToCartAsync("sauce-labs-backpack");
            await _inventoryPage.NavigateToCartAsync();
        });

        await AllureApi.Step("Click product name link", async () =>
        {
            await _cartPage.ClickProductNameAsync("Sauce Labs Backpack");
        });

        await AllureApi.Step("Verify redirected to product detail page", async () =>
        {
            Assert.Contains("inventory-item.html", _page.Url);
        });

        await AllureApi.Step("Verify product information displayed", async () =>
        {
            await Assertions.Expect(_page.Locator(".inventory_details_name")).ToContainTextAsync("Sauce Labs Backpack");
            await Assertions.Expect(_page.Locator(".inventory_details_desc")).ToBeVisibleAsync();
            await Assertions.Expect(_page.Locator(".inventory_details_price")).ToBeVisibleAsync();
        });

        await AllureApi.Step("Navigate back to cart", async () =>
        {
            await _page.ClickAsync("[data-test='back-to-products']");
            await _page.ClickAsync(".shopping_cart_link");
            await Assertions.Expect(_page).ToHaveURLAsync("https://www.saucedemo.com/cart.html");
        });
    }

    [Fact]
    [AllureDescription("Verifies that cart contents persist when navigating between pages")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Regression", "Cart")]
    public async Task VerifyCartPersistenceAcrossNavigation()
    {
        await AllureApi.Step("Navigate to login page", async () =>
        {
            await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Login, add product, navigate to cart", async () =>
        {
            await _loginPage.LoginAsync("standard_user", "secret_sauce");
            await _inventoryPage.AddToCartAsync("sauce-labs-backpack");
            await _inventoryPage.NavigateToCartAsync();
        });

        await AllureApi.Step("Verify product in cart", async () =>
        {
            var isInCart = await _cartPage.IsItemInCartAsync("Sauce Labs Backpack");
            Assert.True(isInCart);
        });

        await AllureApi.Step("Click Continue Shopping", async () =>
        {
            await _cartPage.ClickContinueShoppingAsync();
        });

        await AllureApi.Step("Navigate to product details", async () =>
        {
            await _inventoryPage.ClickProductAsync("Sauce Labs Bike Light");
        });

        await AllureApi.Step("Return to cart", async () =>
        {
            await _page.ClickAsync(".shopping_cart_link");
        });

        await AllureApi.Step("Verify product still in cart", async () =>
        {
            var isInCart = await _cartPage.IsItemInCartAsync("Sauce Labs Backpack");
            Assert.True(isInCart);
        });

        await AllureApi.Step("Verify cart badge consistent", async () =>
        {
            var count = await _inventoryPage.GetCartItemCountAsync();
            Assert.Equal(1, count);
        });
    }

    [Fact]
    [AllureDescription("Verifies cart displays empty state when no items are present")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Regression", "Validation", "Negative")]
    public async Task AttemptCheckoutWithEmptyCart()
    {
        await AllureApi.Step("Navigate to login page", async () =>
        {
            await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Login and navigate to cart with no items", async () =>
        {
            await _loginPage.LoginAsync("standard_user", "secret_sauce");
            await _inventoryPage.NavigateToCartAsync();
        });

        await AllureApi.Step("Verify cart shows empty state", async () =>
        {
            var isEmpty = await _cartPage.IsCartEmptyAsync();
            Assert.True(isEmpty);
        });

        await AllureApi.Step("Verify no items displayed", async () =>
        {
            var count = await _cartPage.GetCartItemCountAsync();
            Assert.Equal(0, count);
        });
    }
}
