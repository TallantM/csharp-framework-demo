using Microsoft.Playwright;
using Moq;
using Xunit;
using csharp_framework_demo.Utilities.PageObjects;
using Allure.Net.Commons;
using Allure.Xunit.Attributes;

namespace csharp_framework_demo.Tests;

[AllureSuite("Unit Tests")]
[AllureFeature("Product Details Page Object")]
public class ProductDetailsPageTests
{
    [Fact]
    [AllureDescription("Verifies that GetProductNameAsync reads product name from details page")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "ProductDetails")]
    public async Task GetProductNameAsync_ReadsProductName()
    {
        var mockPage = new Mock<IPage>();
        var mockLocator = new Mock<ILocator>();
        mockLocator.Setup(l => l.TextContentAsync(null)).ReturnsAsync("Sauce Labs Backpack");
        mockPage.Setup(p => p.Locator(".inventory_details_name", null)).Returns(mockLocator.Object);
        var productDetailsPage = new ProductDetailsPage(mockPage.Object);

        string name = null!;
        await AllureApi.Step("Call GetProductNameAsync", async () =>
        {
            name = await productDetailsPage.GetProductNameAsync();
        });

        AllureApi.Step("Verify product name is returned", () =>
        {
            Assert.Equal("Sauce Labs Backpack", name);
        });
    }

    [Fact]
    [AllureDescription("Verifies that AddToCartAsync clicks the add-to-cart button")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "Cart")]
    public async Task AddToCartAsync_ClicksAddButton()
    {
        var mockPage = new Mock<IPage>();
        var productDetailsPage = new ProductDetailsPage(mockPage.Object);

        await AllureApi.Step("Call AddToCartAsync", async () =>
        {
            await productDetailsPage.AddToCartAsync();
        });

        AllureApi.Step("Verify ClickAsync was called with correct selector", () =>
        {
            mockPage.Verify(p => p.ClickAsync("[data-test='add-to-cart']", It.IsAny<PageClickOptions>()), Times.Once);
        });
    }

    [Fact]
    [AllureDescription("Verifies that RemoveFromCartAsync clicks the remove button")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "Cart")]
    public async Task RemoveFromCartAsync_ClicksRemoveButton()
    {
        var mockPage = new Mock<IPage>();
        var productDetailsPage = new ProductDetailsPage(mockPage.Object);

        await AllureApi.Step("Call RemoveFromCartAsync", async () =>
        {
            await productDetailsPage.RemoveFromCartAsync();
        });

        AllureApi.Step("Verify ClickAsync was called with correct selector", () =>
        {
            mockPage.Verify(p => p.ClickAsync("[data-test='remove']", It.IsAny<PageClickOptions>()), Times.Once);
        });
    }

    [Fact]
    [AllureDescription("Verifies that IsProductInCartAsync checks remove button visibility")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "Cart")]
    public async Task IsProductInCartAsync_ChecksButtonVisibility()
    {
        var mockPage = new Mock<IPage>();
        mockPage.Setup(p => p.IsVisibleAsync("[data-test='remove']", It.IsAny<PageIsVisibleOptions>())).ReturnsAsync(true);
        var productDetailsPage = new ProductDetailsPage(mockPage.Object);

        bool isInCart = false;
        await AllureApi.Step("Call IsProductInCartAsync", async () =>
        {
            isInCart = await productDetailsPage.IsProductInCartAsync();
        });

        AllureApi.Step("Verify result is true", () =>
        {
            Assert.True(isInCart);
        });
    }

    [Fact]
    [AllureDescription("Verifies that ClickBackToProductsAsync clicks the back button")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "Navigation")]
    public async Task ClickBackToProductsAsync_ClicksBackButton()
    {
        var mockPage = new Mock<IPage>();
        var productDetailsPage = new ProductDetailsPage(mockPage.Object);

        await AllureApi.Step("Call ClickBackToProductsAsync", async () =>
        {
            await productDetailsPage.ClickBackToProductsAsync();
        });

        AllureApi.Step("Verify ClickAsync was called with correct selector", () =>
        {
            mockPage.Verify(p => p.ClickAsync("[data-test='back-to-products']", It.IsAny<PageClickOptions>()), Times.Once);
        });
    }

    [Fact]
    [AllureDescription("Verifies that GetCartItemCountAsync reads cart badge and parses to integer")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "Cart")]
    public async Task GetCartItemCountAsync_ReadsBadgeText()
    {
        var mockPage = new Mock<IPage>();
        var mockLocator = new Mock<ILocator>();
        mockLocator.Setup(l => l.IsVisibleAsync(null)).ReturnsAsync(true);
        mockLocator.Setup(l => l.TextContentAsync(null)).ReturnsAsync("2");
        mockPage.Setup(p => p.Locator(".shopping_cart_badge", null)).Returns(mockLocator.Object);
        var productDetailsPage = new ProductDetailsPage(mockPage.Object);

        int count = 0;
        await AllureApi.Step("Call GetCartItemCountAsync", async () =>
        {
            count = await productDetailsPage.GetCartItemCountAsync();
        });

        AllureApi.Step("Verify count is 2", () =>
        {
            Assert.Equal(2, count);
        });
    }
}
