using Microsoft.Playwright;
using Xunit;
using csharp_framework_demo.Utilities.PageObjects;
using Allure.Net.Commons;
using Allure.Xunit.Attributes;

namespace csharp_framework_demo.Tests;

[AllureSuite("Integration Tests")]
[AllureFeature("Checkout Page Object")]
public class CheckoutTests : IClassFixture<PlaywrightFixture>
{
    private readonly PlaywrightFixture _fixture;

    public CheckoutTests(PlaywrightFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    [AllureDescription("Verifies that FillCheckoutInformationAsync fills all checkout form fields")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "Checkout")]
    public async Task FillCheckoutInformation_FillsAllFields()
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

        await AllureApi.Step("Fill checkout information", async () =>
        {
            await checkoutPage.FillCheckoutInformationAsync("John", "Doe", "12345");
        });

        await AllureApi.Step("Verify fields are filled", async () =>
        {
            var firstName = await page.InputValueAsync("[data-test='firstName']");
            var lastName = await page.InputValueAsync("[data-test='lastName']");
            var postalCode = await page.InputValueAsync("[data-test='postalCode']");

            Assert.Equal("John", firstName);
            Assert.Equal("Doe", lastName);
            Assert.Equal("12345", postalCode);
        });
    }

    [Fact]
    [AllureDescription("Verifies that ClickContinueAsync navigates to checkout step two")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "Checkout")]
    public async Task ClickContinue_NavigatesToStepTwo()
    {
        await using var pageContext = await _fixture.CreatePageContextAsync();
        var page = pageContext.Page;
        var loginPage = new LoginPage(page);
        var inventoryPage = new InventoryPage(page);
        var cartPage = new CartPage(page);
        var checkoutPage = new CheckoutPage(page);
        var burgerMenuPage = new BurgerMenuPage(page);
        var productDetailsPage = new ProductDetailsPage(page);

        await AllureApi.Step("Setup checkout and fill information", async () =>
        {
            await loginPage.NavigateToAsync("https://www.saucedemo.com/");
            await loginPage.LoginAsync("standard_user", "secret_sauce");
            await inventoryPage.AddToCartAsync("sauce-labs-backpack");
            await inventoryPage.NavigateToCartAsync();
            await cartPage.ClickCheckoutAsync();
            await checkoutPage.FillCheckoutInformationAsync("John", "Doe", "12345");
        });

        await AllureApi.Step("Click continue", async () =>
        {
            await checkoutPage.ClickContinueAsync();
        });

        AllureApi.Step("Verify URL is checkout step two", () =>
        {
            Assert.Equal("https://www.saucedemo.com/checkout-step-two.html", page.Url);
        });
    }

    [Fact]
    [AllureDescription("Verifies that order summary displays correct values")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "Checkout")]
    public async Task OrderSummary_DisplaysCorrectValues()
    {
        await using var pageContext = await _fixture.CreatePageContextAsync();
        var page = pageContext.Page;
        var loginPage = new LoginPage(page);
        var inventoryPage = new InventoryPage(page);
        var cartPage = new CartPage(page);
        var checkoutPage = new CheckoutPage(page);
        var burgerMenuPage = new BurgerMenuPage(page);
        var productDetailsPage = new ProductDetailsPage(page);

        await AllureApi.Step("Setup checkout and navigate to step two", async () =>
        {
            await loginPage.NavigateToAsync("https://www.saucedemo.com/");
            await loginPage.LoginAsync("standard_user", "secret_sauce");
            await inventoryPage.AddToCartAsync("sauce-labs-backpack");
            await inventoryPage.NavigateToCartAsync();
            await cartPage.ClickCheckoutAsync();
            await checkoutPage.FillCheckoutInformationAsync("John", "Doe", "12345");
            await checkoutPage.ClickContinueAsync();
        });

        string subtotal = null!, tax = null!, total = null!;
        await AllureApi.Step("Get order summary values", async () =>
        {
            subtotal = await checkoutPage.GetSubtotalAsync();
            tax = await checkoutPage.GetTaxAsync();
            total = await checkoutPage.GetTotalAsync();
        });

        AllureApi.Step("Verify summary values are displayed", () =>
        {
            Assert.Contains("Item total:", subtotal);
            Assert.Contains("Tax:", tax);
            Assert.Contains("Total:", total);
        });
    }

    [Fact]
    [AllureDescription("Verifies that ClickFinishAsync completes the order")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "Checkout")]
    public async Task ClickFinish_CompletesOrder()
    {
        await using var pageContext = await _fixture.CreatePageContextAsync();
        var page = pageContext.Page;
        var loginPage = new LoginPage(page);
        var inventoryPage = new InventoryPage(page);
        var cartPage = new CartPage(page);
        var checkoutPage = new CheckoutPage(page);
        var burgerMenuPage = new BurgerMenuPage(page);
        var productDetailsPage = new ProductDetailsPage(page);

        await AllureApi.Step("Setup and complete checkout steps", async () =>
        {
            await loginPage.NavigateToAsync("https://www.saucedemo.com/");
            await loginPage.LoginAsync("standard_user", "secret_sauce");
            await inventoryPage.AddToCartAsync("sauce-labs-backpack");
            await inventoryPage.NavigateToCartAsync();
            await cartPage.ClickCheckoutAsync();
            await checkoutPage.FillCheckoutInformationAsync("John", "Doe", "12345");
            await checkoutPage.ClickContinueAsync();
        });

        await AllureApi.Step("Click finish", async () =>
        {
            await checkoutPage.ClickFinishAsync();
        });

        AllureApi.Step("Verify URL is checkout complete", () =>
        {
            Assert.Equal("https://www.saucedemo.com/checkout-complete.html", page.Url);
        });
    }

    [Fact]
    [AllureDescription("Verifies that confirmation message displays after order completion")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "Checkout")]
    public async Task ConfirmationMessage_DisplaysAfterCompletion()
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

        string message = null!;
        await AllureApi.Step("Get confirmation message", async () =>
        {
            message = await checkoutPage.GetConfirmationMessageAsync();
        });

        AllureApi.Step("Verify confirmation message", () =>
        {
            Assert.Contains("Thank you", message);
        });
    }

    [Fact]
    [AllureDescription("Verifies that error displays when required fields are missing")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "Checkout", "Validation")]
    public async Task ClickContinue_ShowsErrorForMissingFields()
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

        await AllureApi.Step("Click continue without filling fields", async () =>
        {
            await checkoutPage.ClickContinueAsync();
        });

        bool isErrorVisible = false;
        await AllureApi.Step("Verify error is visible", async () =>
        {
            isErrorVisible = await checkoutPage.IsErrorVisibleAsync();
            Assert.True(isErrorVisible);
        });
    }

    [Fact]
    [AllureDescription("Verifies that ClickCancelAsync returns to cart page")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "Checkout")]
    public async Task ClickCancel_ReturnsToCart()
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

        await AllureApi.Step("Click cancel", async () =>
        {
            await checkoutPage.ClickCancelAsync();
        });

        AllureApi.Step("Verify URL is cart page", () =>
        {
            Assert.Equal("https://www.saucedemo.com/cart.html", page.Url);
        });
    }
}
