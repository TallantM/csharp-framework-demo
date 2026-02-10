using Microsoft.Playwright;
using Moq;
using Xunit;
using csharp_framework_demo.Utilities.PageObjects;
using Allure.Net.Commons;
using Allure.Xunit.Attributes;

namespace csharp_framework_demo.Tests;

[AllureSuite("Unit Tests")]
[AllureFeature("Cart Page Object")]
public class CartPageTests
{
    [Fact]
    [AllureDescription("Verifies that GetCartItemCountAsync calls Locator and CountAsync with correct selector")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "Cart")]
    public async Task GetCartItemCountAsync_CallsLocatorAndCountAsync()
    {
        // Arrange
        var mockPage = new Mock<IPage>();
        var mockLocator = new Mock<ILocator>();
        mockLocator.Setup(l => l.CountAsync()).ReturnsAsync(2);
        mockPage.Setup(p => p.Locator(".cart_item", null)).Returns(mockLocator.Object);
        var cartPage = new CartPage(mockPage.Object);

        // Act
        int count = 0;
        await AllureApi.Step("Call GetCartItemCountAsync", async () =>
        {
            count = await cartPage.GetCartItemCountAsync();
        });

        // Assert
        AllureApi.Step($"Verify count is 2", () =>
        {
            Assert.Equal(2, count);
        });

        AllureApi.Step("Verify Locator was called with correct selector", () =>
        {
            mockPage.Verify(p => p.Locator(".cart_item", null), Times.Once);
        });

        AllureApi.Step("Verify CountAsync was called", () =>
        {
            mockLocator.Verify(l => l.CountAsync(), Times.Once);
        });
    }

    [Fact]
    [AllureDescription("Verifies that GetCartItemNamesAsync retrieves all item names from cart")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "Cart")]
    public async Task GetCartItemNamesAsync_RetrievesAllItemNames()
    {
        // Arrange
        var mockPage = new Mock<IPage>();
        var mockLocator = new Mock<ILocator>();
        var expectedNames = new[] { "Sauce Labs Backpack", "Sauce Labs Bike Light" };
        mockLocator.Setup(l => l.AllTextContentsAsync()).ReturnsAsync(expectedNames);
        mockPage.Setup(p => p.Locator(".cart_item .inventory_item_name", null)).Returns(mockLocator.Object);
        var cartPage = new CartPage(mockPage.Object);

        // Act
        List<string> names = null!;
        await AllureApi.Step("Call GetCartItemNamesAsync", async () =>
        {
            names = await cartPage.GetCartItemNamesAsync();
        });

        // Assert
        AllureApi.Step("Verify item names list contains expected items", () =>
        {
            Assert.Equal(expectedNames.ToList(), names);
        });

        AllureApi.Step("Verify Locator was called with correct selector", () =>
        {
            mockPage.Verify(p => p.Locator(".cart_item .inventory_item_name", null), Times.Once);
        });

        AllureApi.Step("Verify AllTextContentsAsync was called", () =>
        {
            mockLocator.Verify(l => l.AllTextContentsAsync(), Times.Once);
        });
    }

    [Fact]
    [AllureDescription("Verifies that RemoveItemAsync clicks the correct remove button")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "Cart")]
    public async Task RemoveItemAsync_ClicksCorrectRemoveButton()
    {
        // Arrange
        var mockPage = new Mock<IPage>();
        var cartPage = new CartPage(mockPage.Object);
        var productName = "sauce-labs-backpack";

        // Act
        await AllureApi.Step($"Call RemoveItemAsync with product: {productName}", async () =>
        {
            await cartPage.RemoveItemAsync(productName);
        });

        // Assert
        AllureApi.Step("Verify ClickAsync was called with correct selector", () =>
        {
            mockPage.Verify(p => p.ClickAsync("[data-test='remove-sauce-labs-backpack']", It.IsAny<PageClickOptions>()), Times.Once);
        });
    }

    [Fact]
    [AllureDescription("Verifies that IsItemInCartAsync checks for product name in cart items")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "Cart")]
    public async Task IsItemInCartAsync_ChecksForProductName()
    {
        // Arrange
        var mockPage = new Mock<IPage>();
        var mockLocator = new Mock<ILocator>();
        var cartItems = new[] { "Sauce Labs Backpack", "Sauce Labs Bike Light" };
        mockLocator.Setup(l => l.AllTextContentsAsync()).ReturnsAsync(cartItems);
        mockPage.Setup(p => p.Locator(".cart_item .inventory_item_name", null)).Returns(mockLocator.Object);
        var cartPage = new CartPage(mockPage.Object);
        var productName = "Sauce Labs Backpack";

        // Act
        bool isInCart = false;
        await AllureApi.Step($"Call IsItemInCartAsync with product: {productName}", async () =>
        {
            isInCart = await cartPage.IsItemInCartAsync(productName);
        });

        // Assert
        AllureApi.Step("Verify result is true", () =>
        {
            Assert.True(isInCart);
        });
    }

    [Fact]
    [AllureDescription("Verifies that GetItemPriceAsync returns price for specified product")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "Cart")]
    public async Task GetItemPriceAsync_ReturnsPriceForProduct()
    {
        // Arrange
        var mockPage = new Mock<IPage>();
        var mockCartItems = new Mock<ILocator>();
        var mockItem = new Mock<ILocator>();
        var mockName = new Mock<ILocator>();
        var mockPrice = new Mock<ILocator>();

        mockCartItems.Setup(l => l.CountAsync()).ReturnsAsync(1);
        mockCartItems.Setup(l => l.Nth(0)).Returns(mockItem.Object);
        mockItem.Setup(i => i.Locator(".inventory_item_name", null)).Returns(mockName.Object);
        mockItem.Setup(i => i.Locator(".inventory_item_price", null)).Returns(mockPrice.Object);
        mockName.Setup(n => n.TextContentAsync(null)).ReturnsAsync("Sauce Labs Backpack");
        mockPrice.Setup(p => p.TextContentAsync(null)).ReturnsAsync("$29.99");
        mockPage.Setup(p => p.Locator(".cart_item", null)).Returns(mockCartItems.Object);

        var cartPage = new CartPage(mockPage.Object);
        var productName = "Sauce Labs Backpack";

        // Act
        string price = null!;
        await AllureApi.Step($"Call GetItemPriceAsync with product: {productName}", async () =>
        {
            price = await cartPage.GetItemPriceAsync(productName);
        });

        // Assert
        AllureApi.Step("Verify price is $29.99", () =>
        {
            Assert.Equal("$29.99", price);
        });
    }

    [Fact]
    [AllureDescription("Verifies that ClickContinueShoppingAsync clicks the continue shopping button")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "Navigation")]
    public async Task ClickContinueShoppingAsync_ClicksContinueButton()
    {
        // Arrange
        var mockPage = new Mock<IPage>();
        var cartPage = new CartPage(mockPage.Object);

        // Act
        await AllureApi.Step("Call ClickContinueShoppingAsync", async () =>
        {
            await cartPage.ClickContinueShoppingAsync();
        });

        // Assert
        AllureApi.Step("Verify ClickAsync was called with correct selector", () =>
        {
            mockPage.Verify(p => p.ClickAsync("[data-test='continue-shopping']", It.IsAny<PageClickOptions>()), Times.Once);
        });
    }

    [Fact]
    [AllureDescription("Verifies that ClickCheckoutAsync clicks the checkout button")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "Checkout")]
    public async Task ClickCheckoutAsync_ClicksCheckoutButton()
    {
        // Arrange
        var mockPage = new Mock<IPage>();
        var cartPage = new CartPage(mockPage.Object);

        // Act
        await AllureApi.Step("Call ClickCheckoutAsync", async () =>
        {
            await cartPage.ClickCheckoutAsync();
        });

        // Assert
        AllureApi.Step("Verify ClickAsync was called with correct selector", () =>
        {
            mockPage.Verify(p => p.ClickAsync("[data-test='checkout']", It.IsAny<PageClickOptions>()), Times.Once);
        });
    }

    [Fact]
    [AllureDescription("Verifies that IsCartEmptyAsync checks for cart items correctly")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "Cart")]
    public async Task IsCartEmptyAsync_ChecksForCartItems()
    {
        // Arrange
        var mockPage = new Mock<IPage>();
        var mockLocator = new Mock<ILocator>();
        mockLocator.Setup(l => l.CountAsync()).ReturnsAsync(0);
        mockPage.Setup(p => p.Locator(".cart_item", null)).Returns(mockLocator.Object);
        var cartPage = new CartPage(mockPage.Object);

        // Act
        bool isEmpty = false;
        await AllureApi.Step("Call IsCartEmptyAsync", async () =>
        {
            isEmpty = await cartPage.IsCartEmptyAsync();
        });

        // Assert
        AllureApi.Step("Verify result is true for empty cart", () =>
        {
            Assert.True(isEmpty);
        });
    }

    [Fact]
    [AllureDescription("Verifies that ClickProductNameAsync clicks the product name link in cart")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "Navigation")]
    public async Task ClickProductNameAsync_ClicksProductLink()
    {
        // Arrange
        var mockPage = new Mock<IPage>();
        var mockLocator = new Mock<ILocator>();
        var productName = "Sauce Labs Backpack";
        mockPage.Setup(p => p.Locator(".cart_item .inventory_item_name", It.IsAny<PageLocatorOptions>())).Returns(mockLocator.Object);
        var cartPage = new CartPage(mockPage.Object);

        // Act
        await AllureApi.Step($"Call ClickProductNameAsync with product: {productName}", async () =>
        {
            await cartPage.ClickProductNameAsync(productName);
        });

        // Assert
        AllureApi.Step("Verify Locator was called with correct selector and text", () =>
        {
            mockPage.Verify(p => p.Locator(".cart_item .inventory_item_name", It.Is<PageLocatorOptions>(o => o.HasTextString == productName)), Times.Once);
        });

        AllureApi.Step("Verify ClickAsync was called on locator", () =>
        {
            mockLocator.Verify(l => l.ClickAsync(null), Times.Once);
        });
    }
}
