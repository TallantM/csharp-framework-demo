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
    private readonly IPage _page;
    private readonly LoginPage _loginPage;
    private readonly InventoryPage _inventoryPage;
    private readonly CartPage _cartPage;
    private readonly CheckoutPage _checkoutPage;

    public CheckoutTests(PlaywrightFixture fixture)
    {
        _page = fixture.Page;
        _loginPage = new LoginPage(_page);
        _inventoryPage = new InventoryPage(_page);
        _cartPage = new CartPage(_page);
        _checkoutPage = new CheckoutPage(_page);
    }

    private async Task SetupCheckoutAsync()
    {
        await _loginPage.NavigateToAsync("https://www.saucedemo.com/");
        await _loginPage.LoginAsync("standard_user", "secret_sauce");
        await _inventoryPage.AddToCartAsync("sauce-labs-backpack");
        await _inventoryPage.NavigateToCartAsync();
        await _cartPage.ClickCheckoutAsync();
    }

    [Fact]
    [AllureDescription("Verifies that FillCheckoutInformationAsync fills all checkout form fields")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "Checkout")]
    public async Task FillCheckoutInformation_FillsAllFields()
    {
        await AllureApi.Step("Setup checkout", async () => await SetupCheckoutAsync());

        await AllureApi.Step("Fill checkout information", async () =>
        {
            await _checkoutPage.FillCheckoutInformationAsync("John", "Doe", "12345");
        });

        await AllureApi.Step("Verify fields are filled", async () =>
        {
            var firstName = await _page.InputValueAsync("[data-test='firstName']");
            var lastName = await _page.InputValueAsync("[data-test='lastName']");
            var postalCode = await _page.InputValueAsync("[data-test='postalCode']");

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
        await AllureApi.Step("Setup checkout and fill information", async () =>
        {
            await SetupCheckoutAsync();
            await _checkoutPage.FillCheckoutInformationAsync("John", "Doe", "12345");
        });

        await AllureApi.Step("Click continue", async () =>
        {
            await _checkoutPage.ClickContinueAsync();
        });

        AllureApi.Step("Verify URL is checkout step two", () =>
        {
            Assert.Equal("https://www.saucedemo.com/checkout-step-two.html", _page.Url);
        });
    }

    [Fact]
    [AllureDescription("Verifies that order summary displays correct values")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "Checkout")]
    public async Task OrderSummary_DisplaysCorrectValues()
    {
        await AllureApi.Step("Setup checkout and navigate to step two", async () =>
        {
            await SetupCheckoutAsync();
            await _checkoutPage.FillCheckoutInformationAsync("John", "Doe", "12345");
            await _checkoutPage.ClickContinueAsync();
        });

        string subtotal = null!, tax = null!, total = null!;
        await AllureApi.Step("Get order summary values", async () =>
        {
            subtotal = await _checkoutPage.GetSubtotalAsync();
            tax = await _checkoutPage.GetTaxAsync();
            total = await _checkoutPage.GetTotalAsync();
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
        await AllureApi.Step("Setup and complete checkout steps", async () =>
        {
            await SetupCheckoutAsync();
            await _checkoutPage.FillCheckoutInformationAsync("John", "Doe", "12345");
            await _checkoutPage.ClickContinueAsync();
        });

        await AllureApi.Step("Click finish", async () =>
        {
            await _checkoutPage.ClickFinishAsync();
        });

        AllureApi.Step("Verify URL is checkout complete", () =>
        {
            Assert.Equal("https://www.saucedemo.com/checkout-complete.html", _page.Url);
        });
    }

    [Fact]
    [AllureDescription("Verifies that confirmation message displays after order completion")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "Checkout")]
    public async Task ConfirmationMessage_DisplaysAfterCompletion()
    {
        await AllureApi.Step("Complete full checkout flow", async () =>
        {
            await SetupCheckoutAsync();
            await _checkoutPage.FillCheckoutInformationAsync("John", "Doe", "12345");
            await _checkoutPage.ClickContinueAsync();
            await _checkoutPage.ClickFinishAsync();
        });

        string message = null!;
        await AllureApi.Step("Get confirmation message", async () =>
        {
            message = await _checkoutPage.GetConfirmationMessageAsync();
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
        await AllureApi.Step("Setup checkout", async () => await SetupCheckoutAsync());

        await AllureApi.Step("Click continue without filling fields", async () =>
        {
            await _checkoutPage.ClickContinueAsync();
        });

        bool isErrorVisible = false;
        await AllureApi.Step("Verify error is visible", async () =>
        {
            isErrorVisible = await _checkoutPage.IsErrorVisibleAsync();
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
        await AllureApi.Step("Setup checkout", async () => await SetupCheckoutAsync());

        await AllureApi.Step("Click cancel", async () =>
        {
            await _checkoutPage.ClickCancelAsync();
        });

        AllureApi.Step("Verify URL is cart page", () =>
        {
            Assert.Equal("https://www.saucedemo.com/cart.html", _page.Url);
        });
    }
}
