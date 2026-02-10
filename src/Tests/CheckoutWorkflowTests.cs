using Microsoft.Playwright;
using Xunit;
using csharp_framework_demo.Utilities.PageObjects;
using Allure.Net.Commons;
using Allure.Xunit.Attributes;

namespace csharp_framework_demo.Tests;

[AllureSuite("End-to-End Tests")]
[AllureFeature("Checkout")]
public class CheckoutWorkflowTests : IClassFixture<PlaywrightFixture>
{
    private readonly IPage _page;
    private readonly LoginPage _loginPage;
    private readonly InventoryPage _inventoryPage;
    private readonly CartPage _cartPage;
    private readonly CheckoutPage _checkoutPage;

    public CheckoutWorkflowTests(PlaywrightFixture fixture)
    {
        _page = fixture.Page;
        _loginPage = new LoginPage(_page);
        _inventoryPage = new InventoryPage(_page);
        _cartPage = new CartPage(_page);
        _checkoutPage = new CheckoutPage(_page);
    }

    [Fact]
    [AllureDescription("Verifies that user can complete full checkout process with one product")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Smoke", "Checkout")]
    public async Task CompleteCheckoutWithSingleItem()
    {
        await AllureApi.Step("Navigate to login page", async () =>
        {
            await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Login and add product to cart", async () =>
        {
            await _loginPage.LoginAsync("standard_user", "secret_sauce");
            await _inventoryPage.AddToCartAsync("sauce-labs-backpack");
        });

        await AllureApi.Step("Navigate to cart and click checkout", async () =>
        {
            await _inventoryPage.NavigateToCartAsync();
            await _cartPage.ClickCheckoutAsync();
        });

        await AllureApi.Step("Fill customer information", async () =>
        {
            await _checkoutPage.FillCheckoutInformationAsync("John", "Doe", "12345");
        });

        await AllureApi.Step("Click continue to review order", async () =>
        {
            await _checkoutPage.ClickContinueAsync();
        });

        await AllureApi.Step("Verify order summary is correct", async () =>
        {
            var subtotal = await _checkoutPage.GetSubtotalAsync();
            var tax = await _checkoutPage.GetTaxAsync();
            var total = await _checkoutPage.GetTotalAsync();

            Assert.Contains("Item total:", subtotal);
            Assert.Contains("Tax:", tax);
            Assert.Contains("Total:", total);
        });

        await AllureApi.Step("Click finish to complete order", async () =>
        {
            await _checkoutPage.ClickFinishAsync();
        });

        await AllureApi.Step("Verify confirmation message appears", async () =>
        {
            await Assertions.Expect(_page).ToHaveURLAsync("https://www.saucedemo.com/checkout-complete.html");
            var message = await _checkoutPage.GetConfirmationMessageAsync();
            Assert.Contains("Thank you for your order", message);
        });
    }

    [Fact]
    [AllureDescription("Verifies that user can complete checkout with multiple products in cart")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Smoke", "Checkout")]
    public async Task CompleteCheckoutWithMultipleItems()
    {
        await AllureApi.Step("Navigate to login page", async () =>
        {
            await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Login and add three products to cart", async () =>
        {
            await _loginPage.LoginAsync("standard_user", "secret_sauce");
            await _inventoryPage.AddToCartAsync("sauce-labs-backpack");
            await _inventoryPage.AddToCartAsync("sauce-labs-bike-light");
            await _inventoryPage.AddToCartAsync("sauce-labs-bolt-t-shirt");
        });

        await AllureApi.Step("Navigate to cart and verify all items", async () =>
        {
            await _inventoryPage.NavigateToCartAsync();
            var count = await _cartPage.GetCartItemCountAsync();
            Assert.Equal(3, count);
        });

        await AllureApi.Step("Proceed to checkout", async () =>
        {
            await _cartPage.ClickCheckoutAsync();
        });

        await AllureApi.Step("Fill customer information", async () =>
        {
            await _checkoutPage.FillCheckoutInformationAsync("Jane", "Smith", "67890");
            await _checkoutPage.ClickContinueAsync();
        });

        await AllureApi.Step("Review order summary with multiple items", async () =>
        {
            var items = await _page.Locator(".cart_item").CountAsync();
            Assert.Equal(3, items);
        });

        await AllureApi.Step("Verify total price is correct", async () =>
        {
            var total = await _checkoutPage.GetTotalAsync();
            Assert.Contains("Total:", total);
        });

        await AllureApi.Step("Complete order", async () =>
        {
            await _checkoutPage.ClickFinishAsync();
        });

        await AllureApi.Step("Verify confirmation", async () =>
        {
            var message = await _checkoutPage.GetConfirmationMessageAsync();
            Assert.Contains("Thank you for your order", message);
        });
    }

    [Fact]
    [AllureDescription("Verifies that user can cancel checkout and return to cart")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Regression", "Checkout")]
    public async Task CancelCheckoutFromStepOne()
    {
        await AllureApi.Step("Navigate to login page", async () =>
        {
            await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Login, add product, navigate to checkout", async () =>
        {
            await _loginPage.LoginAsync("standard_user", "secret_sauce");
            await _inventoryPage.AddToCartAsync("sauce-labs-backpack");
            await _inventoryPage.NavigateToCartAsync();
            await _cartPage.ClickCheckoutAsync();
        });

        await AllureApi.Step("Start filling information", async () =>
        {
            await _checkoutPage.FillCheckoutInformationAsync("John", "", "");
        });

        await AllureApi.Step("Click cancel button", async () =>
        {
            await _checkoutPage.ClickCancelAsync();
        });

        await AllureApi.Step("Verify returned to cart page", async () =>
        {
            await Assertions.Expect(_page).ToHaveURLAsync("https://www.saucedemo.com/cart.html");
        });

        await AllureApi.Step("Verify product still in cart", async () =>
        {
            var isInCart = await _cartPage.IsItemInCartAsync("Sauce Labs Backpack");
            Assert.True(isInCart);
        });
    }

    [Fact]
    [AllureDescription("Verifies validation error when first name is missing")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Regression", "Validation")]
    public async Task ValidationErrorForMissingFirstName()
    {
        await AllureApi.Step("Setup checkout", async () =>
        {
            await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
            await _loginPage.LoginAsync("standard_user", "secret_sauce");
            await _inventoryPage.AddToCartAsync("sauce-labs-backpack");
            await _inventoryPage.NavigateToCartAsync();
            await _cartPage.ClickCheckoutAsync();
        });

        await AllureApi.Step("Fill only last name and postal code", async () =>
        {
            await _checkoutPage.FillCheckoutInformationAsync("", "Doe", "12345");
        });

        await AllureApi.Step("Click continue", async () =>
        {
            await _checkoutPage.ClickContinueAsync();
        });

        await AllureApi.Step("Verify error message is displayed", async () =>
        {
            var isErrorVisible = await _checkoutPage.IsErrorVisibleAsync();
            Assert.True(isErrorVisible);

            var errorMessage = await _checkoutPage.GetErrorMessageAsync();
            Assert.Contains("First Name is required", errorMessage);
        });
    }

    [Fact]
    [AllureDescription("Verifies that user can return to products after completing order")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Regression", "Checkout")]
    public async Task ReturnToProductsAfterOrderCompletion()
    {
        await AllureApi.Step("Complete full checkout flow", async () =>
        {
            await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
            await _loginPage.LoginAsync("standard_user", "secret_sauce");
            await _inventoryPage.AddToCartAsync("sauce-labs-backpack");
            await _inventoryPage.NavigateToCartAsync();
            await _cartPage.ClickCheckoutAsync();
            await _checkoutPage.FillCheckoutInformationAsync("John", "Doe", "12345");
            await _checkoutPage.ClickContinueAsync();
            await _checkoutPage.ClickFinishAsync();
        });

        await AllureApi.Step("Click back to products", async () =>
        {
            await _checkoutPage.ClickBackToProductsAsync();
        });

        await AllureApi.Step("Verify returned to inventory page", async () =>
        {
            await Assertions.Expect(_page).ToHaveURLAsync("https://www.saucedemo.com/inventory.html");
        });

        await AllureApi.Step("Verify cart is empty", async () =>
        {
            var cartCount = await _inventoryPage.GetCartItemCountAsync();
            Assert.Equal(0, cartCount);
        });
    }
}
