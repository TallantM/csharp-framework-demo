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
    private readonly PlaywrightFixture _fixture;

    public CheckoutWorkflowTests(PlaywrightFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    [AllureDescription("Verifies that user can complete full checkout process with one product")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Smoke", "Checkout")]
    public async Task CompleteCheckoutWithSingleItem()
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

        await AllureApi.Step("Login and add product to cart", async () =>
        {
            await loginPage.LoginAsync("standard_user", "secret_sauce");
            await inventoryPage.AddToCartAsync("sauce-labs-backpack");
        });

        await AllureApi.Step("Navigate to cart and click checkout", async () =>
        {
            await inventoryPage.NavigateToCartAsync();
            await cartPage.ClickCheckoutAsync();
        });

        await AllureApi.Step("Fill customer information", async () =>
        {
            await checkoutPage.FillCheckoutInformationAsync("John", "Doe", "12345");
        });

        await AllureApi.Step("Click continue to review order", async () =>
        {
            await checkoutPage.ClickContinueAsync();
        });

        await AllureApi.Step("Verify order summary is correct", async () =>
        {
            var subtotal = await checkoutPage.GetSubtotalAsync();
            var tax = await checkoutPage.GetTaxAsync();
            var total = await checkoutPage.GetTotalAsync();

            Assert.Contains("Item total:", subtotal);
            Assert.Contains("Tax:", tax);
            Assert.Contains("Total:", total);
        });

        await AllureApi.Step("Click finish to complete order", async () =>
        {
            await checkoutPage.ClickFinishAsync();
        });

        await AllureApi.Step("Verify confirmation message appears", async () =>
        {
            await Assertions.Expect(page).ToHaveURLAsync("https://www.saucedemo.com/checkout-complete.html");
            var message = await checkoutPage.GetConfirmationMessageAsync();
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

        await AllureApi.Step("Login and add three products to cart", async () =>
        {
            await loginPage.LoginAsync("standard_user", "secret_sauce");
            await inventoryPage.AddToCartAsync("sauce-labs-backpack");
            await inventoryPage.AddToCartAsync("sauce-labs-bike-light");
            await inventoryPage.AddToCartAsync("sauce-labs-bolt-t-shirt");
        });

        await AllureApi.Step("Navigate to cart and verify all items", async () =>
        {
            await inventoryPage.NavigateToCartAsync();
            var count = await cartPage.GetCartItemCountAsync();
            Assert.Equal(3, count);
        });

        await AllureApi.Step("Proceed to checkout", async () =>
        {
            await cartPage.ClickCheckoutAsync();
        });

        await AllureApi.Step("Fill customer information", async () =>
        {
            await checkoutPage.FillCheckoutInformationAsync("Jane", "Smith", "67890");
            await checkoutPage.ClickContinueAsync();
        });

        await AllureApi.Step("Review order summary with multiple items", async () =>
        {
            var items = await page.Locator(".cart_item").CountAsync();
            Assert.Equal(3, items);
        });

        await AllureApi.Step("Verify total price is correct", async () =>
        {
            var total = await checkoutPage.GetTotalAsync();
            Assert.Contains("Total:", total);
        });

        await AllureApi.Step("Complete order", async () =>
        {
            await checkoutPage.ClickFinishAsync();
        });

        await AllureApi.Step("Verify confirmation", async () =>
        {
            var message = await checkoutPage.GetConfirmationMessageAsync();
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

        await AllureApi.Step("Login, add product, navigate to checkout", async () =>
        {
            await loginPage.LoginAsync("standard_user", "secret_sauce");
            await inventoryPage.AddToCartAsync("sauce-labs-backpack");
            await inventoryPage.NavigateToCartAsync();
            await cartPage.ClickCheckoutAsync();
        });

        await AllureApi.Step("Start filling information", async () =>
        {
            await checkoutPage.FillCheckoutInformationAsync("John", "", "");
        });

        await AllureApi.Step("Click cancel button", async () =>
        {
            await checkoutPage.ClickCancelAsync();
        });

        await AllureApi.Step("Verify returned to cart page", async () =>
        {
            await Assertions.Expect(page).ToHaveURLAsync("https://www.saucedemo.com/cart.html");
        });

        await AllureApi.Step("Verify product still in cart", async () =>
        {
            var isInCart = await cartPage.IsItemInCartAsync("Sauce Labs Backpack");
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
        await using var pageContext = await _fixture.CreatePageContextAsync();
        var page = pageContext.Page;
        var loginPage = new LoginPage(page);
        var inventoryPage = new InventoryPage(page);
        var cartPage = new CartPage(page);
        var checkoutPage = new CheckoutPage(page);
        var burgerMenuPage = new BurgerMenuPage(page);
        var productDetailsPage = new ProductDetailsPage(page);

        await AllureApi.Step("Setup checkout", async () =>
        {
            await loginPage.NavigateToAsync("https://www.saucedemo.com/");
            await loginPage.LoginAsync("standard_user", "secret_sauce");
            await inventoryPage.AddToCartAsync("sauce-labs-backpack");
            await inventoryPage.NavigateToCartAsync();
            await cartPage.ClickCheckoutAsync();
        });

        await AllureApi.Step("Fill only last name and postal code", async () =>
        {
            await checkoutPage.FillCheckoutInformationAsync("", "Doe", "12345");
        });

        await AllureApi.Step("Click continue", async () =>
        {
            await checkoutPage.ClickContinueAsync();
        });

        await AllureApi.Step("Verify error message is displayed", async () =>
        {
            var isErrorVisible = await checkoutPage.IsErrorVisibleAsync();
            Assert.True(isErrorVisible);

            var errorMessage = await checkoutPage.GetErrorMessageAsync();
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
        await using var pageContext = await _fixture.CreatePageContextAsync();
        var page = pageContext.Page;
        var loginPage = new LoginPage(page);
        var inventoryPage = new InventoryPage(page);
        var cartPage = new CartPage(page);
        var checkoutPage = new CheckoutPage(page);
        var burgerMenuPage = new BurgerMenuPage(page);
        var productDetailsPage = new ProductDetailsPage(page);

        await AllureApi.Step("Complete full checkout flow", async () =>
        {
            await loginPage.NavigateToAsync("https://www.saucedemo.com/");
            await loginPage.LoginAsync("standard_user", "secret_sauce");
            await inventoryPage.AddToCartAsync("sauce-labs-backpack");
            await inventoryPage.NavigateToCartAsync();
            await cartPage.ClickCheckoutAsync();
            await checkoutPage.FillCheckoutInformationAsync("John", "Doe", "12345");
            await checkoutPage.ClickContinueAsync();
            await checkoutPage.ClickFinishAsync();
        });

        await AllureApi.Step("Click back to products", async () =>
        {
            await checkoutPage.ClickBackToProductsAsync();
        });

        await AllureApi.Step("Verify returned to inventory page", async () =>
        {
            await Assertions.Expect(page).ToHaveURLAsync("https://www.saucedemo.com/inventory.html");
        });

        await AllureApi.Step("Verify cart is empty", async () =>
        {
            var cartCount = await inventoryPage.GetCartItemCountAsync();
            Assert.Equal(0, cartCount);
        });
    }
}
