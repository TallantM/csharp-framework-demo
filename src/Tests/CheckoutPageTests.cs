using Microsoft.Playwright;
using Moq;
using Xunit;
using csharp_framework_demo.Utilities.PageObjects;
using Allure.Net.Commons;
using Allure.Xunit.Attributes;

namespace csharp_framework_demo.Tests;

[AllureSuite("Unit Tests")]
[AllureFeature("Checkout Page Object")]
public class CheckoutPageTests
{
    [Fact]
    [AllureDescription("Verifies that FillCheckoutInformationAsync fills all three form fields")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "Checkout")]
    public async Task FillCheckoutInformationAsync_FillsAllThreeFields()
    {
        var mockPage = new Mock<IPage>();
        var checkoutPage = new CheckoutPage(mockPage.Object);
        var firstName = "John";
        var lastName = "Doe";
        var postalCode = "12345";

        await AllureApi.Step($"Call FillCheckoutInformationAsync", async () =>
        {
            await checkoutPage.FillCheckoutInformationAsync(firstName, lastName, postalCode);
        });

        AllureApi.Step("Verify firstName field was filled", () =>
        {
            mockPage.Verify(p => p.FillAsync("[data-test='firstName']", firstName, It.IsAny<PageFillOptions>()), Times.Once);
        });

        AllureApi.Step("Verify lastName field was filled", () =>
        {
            mockPage.Verify(p => p.FillAsync("[data-test='lastName']", lastName, It.IsAny<PageFillOptions>()), Times.Once);
        });

        AllureApi.Step("Verify postalCode field was filled", () =>
        {
            mockPage.Verify(p => p.FillAsync("[data-test='postalCode']", postalCode, It.IsAny<PageFillOptions>()), Times.Once);
        });
    }

    [Fact]
    [AllureDescription("Verifies that ClickContinueAsync clicks the continue button")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "Checkout")]
    public async Task ClickContinueAsync_ClicksContinueButton()
    {
        var mockPage = new Mock<IPage>();
        var checkoutPage = new CheckoutPage(mockPage.Object);

        await AllureApi.Step("Call ClickContinueAsync", async () =>
        {
            await checkoutPage.ClickContinueAsync();
        });

        AllureApi.Step("Verify ClickAsync was called with correct selector", () =>
        {
            mockPage.Verify(p => p.ClickAsync("[data-test='continue']", It.IsAny<PageClickOptions>()), Times.Once);
        });
    }

    [Fact]
    [AllureDescription("Verifies that ClickCancelAsync clicks the cancel button")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "Checkout")]
    public async Task ClickCancelAsync_ClicksCancelButton()
    {
        var mockPage = new Mock<IPage>();
        var checkoutPage = new CheckoutPage(mockPage.Object);

        await AllureApi.Step("Call ClickCancelAsync", async () =>
        {
            await checkoutPage.ClickCancelAsync();
        });

        AllureApi.Step("Verify ClickAsync was called with correct selector", () =>
        {
            mockPage.Verify(p => p.ClickAsync("[data-test='cancel']", It.IsAny<PageClickOptions>()), Times.Once);
        });
    }

    [Fact]
    [AllureDescription("Verifies that GetSubtotalAsync reads the subtotal label text")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "Checkout")]
    public async Task GetSubtotalAsync_ReadsSubtotalLabel()
    {
        var mockPage = new Mock<IPage>();
        var mockLocator = new Mock<ILocator>();
        mockLocator.Setup(l => l.TextContentAsync(null)).ReturnsAsync("Item total: $29.99");
        mockPage.Setup(p => p.Locator(".summary_subtotal_label", null)).Returns(mockLocator.Object);
        var checkoutPage = new CheckoutPage(mockPage.Object);

        string subtotal = null!;
        await AllureApi.Step("Call GetSubtotalAsync", async () =>
        {
            subtotal = await checkoutPage.GetSubtotalAsync();
        });

        AllureApi.Step("Verify subtotal text is returned", () =>
        {
            Assert.Equal("Item total: $29.99", subtotal);
        });
    }

    [Fact]
    [AllureDescription("Verifies that GetTaxAsync reads the tax label text")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "Checkout")]
    public async Task GetTaxAsync_ReadsTaxLabel()
    {
        var mockPage = new Mock<IPage>();
        var mockLocator = new Mock<ILocator>();
        mockLocator.Setup(l => l.TextContentAsync(null)).ReturnsAsync("Tax: $2.40");
        mockPage.Setup(p => p.Locator(".summary_tax_label", null)).Returns(mockLocator.Object);
        var checkoutPage = new CheckoutPage(mockPage.Object);

        string tax = null!;
        await AllureApi.Step("Call GetTaxAsync", async () =>
        {
            tax = await checkoutPage.GetTaxAsync();
        });

        AllureApi.Step("Verify tax text is returned", () =>
        {
            Assert.Equal("Tax: $2.40", tax);
        });
    }

    [Fact]
    [AllureDescription("Verifies that GetTotalAsync reads the total label text")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "Checkout")]
    public async Task GetTotalAsync_ReadsTotalLabel()
    {
        var mockPage = new Mock<IPage>();
        var mockLocator = new Mock<ILocator>();
        mockLocator.Setup(l => l.TextContentAsync(null)).ReturnsAsync("Total: $32.39");
        mockPage.Setup(p => p.Locator(".summary_total_label", null)).Returns(mockLocator.Object);
        var checkoutPage = new CheckoutPage(mockPage.Object);

        string total = null!;
        await AllureApi.Step("Call GetTotalAsync", async () =>
        {
            total = await checkoutPage.GetTotalAsync();
        });

        AllureApi.Step("Verify total text is returned", () =>
        {
            Assert.Equal("Total: $32.39", total);
        });
    }

    [Fact]
    [AllureDescription("Verifies that ClickFinishAsync clicks the finish button")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "Checkout")]
    public async Task ClickFinishAsync_ClicksFinishButton()
    {
        var mockPage = new Mock<IPage>();
        var checkoutPage = new CheckoutPage(mockPage.Object);

        await AllureApi.Step("Call ClickFinishAsync", async () =>
        {
            await checkoutPage.ClickFinishAsync();
        });

        AllureApi.Step("Verify ClickAsync was called with correct selector", () =>
        {
            mockPage.Verify(p => p.ClickAsync("[data-test='finish']", It.IsAny<PageClickOptions>()), Times.Once);
        });
    }

    [Fact]
    [AllureDescription("Verifies that GetConfirmationMessageAsync reads the confirmation header")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "Checkout")]
    public async Task GetConfirmationMessageAsync_ReadsConfirmationHeader()
    {
        var mockPage = new Mock<IPage>();
        var mockLocator = new Mock<ILocator>();
        mockLocator.Setup(l => l.TextContentAsync(null)).ReturnsAsync("Thank you for your order!");
        mockPage.Setup(p => p.Locator(".complete-header", null)).Returns(mockLocator.Object);
        var checkoutPage = new CheckoutPage(mockPage.Object);

        string message = null!;
        await AllureApi.Step("Call GetConfirmationMessageAsync", async () =>
        {
            message = await checkoutPage.GetConfirmationMessageAsync();
        });

        AllureApi.Step("Verify confirmation message is returned", () =>
        {
            Assert.Equal("Thank you for your order!", message);
        });
    }

    [Fact]
    [AllureDescription("Verifies that IsErrorVisibleAsync checks for error element visibility")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "Checkout")]
    public async Task IsErrorVisibleAsync_ChecksErrorVisibility()
    {
        var mockPage = new Mock<IPage>();
        mockPage.Setup(p => p.IsVisibleAsync("[data-test='error']", It.IsAny<PageIsVisibleOptions>())).ReturnsAsync(true);
        var checkoutPage = new CheckoutPage(mockPage.Object);

        bool isVisible = false;
        await AllureApi.Step("Call IsErrorVisibleAsync", async () =>
        {
            isVisible = await checkoutPage.IsErrorVisibleAsync();
        });

        AllureApi.Step("Verify result is true", () =>
        {
            Assert.True(isVisible);
        });
    }
}
