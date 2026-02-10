using Microsoft.Playwright;
using Moq;
using Xunit;
using csharp_framework_demo.Utilities.PageObjects;
using Allure.Net.Commons;
using Allure.Xunit.Attributes;

namespace csharp_framework_demo.Tests;

[AllureSuite("Unit Tests")]
[AllureFeature("Inventory Page Object")]
public class InventoryPageTests
{
    [Fact]
    [AllureDescription("Verifies that GetProductCountAsync calls Locator and CountAsync with correct selector")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "Inventory")]
    public async Task GetProductCountAsync_CallsLocatorAndCountAsync()
    {
        // Arrange
        var mockPage = new Mock<IPage>();
        var mockLocator = new Mock<ILocator>();
        mockLocator.Setup(l => l.CountAsync()).ReturnsAsync(6);
        mockPage.Setup(p => p.Locator(".inventory_item", null)).Returns(mockLocator.Object);
        var inventoryPage = new InventoryPage(mockPage.Object);

        // Act
        await AllureApi.Step("Call GetProductCountAsync", async () =>
        {
            var count = await inventoryPage.GetProductCountAsync();

            // Assert
            AllureApi.Step($"Verify count is 6", () =>
            {
                Assert.Equal(6, count);
            });
        });

        AllureApi.Step("Verify Locator was called with correct selector", () =>
        {
            mockPage.Verify(p => p.Locator(".inventory_item", null), Times.Once);
        });

        AllureApi.Step("Verify CountAsync was called", () =>
        {
            mockLocator.Verify(l => l.CountAsync(), Times.Once);
        });
    }

    [Fact]
    [AllureDescription("Verifies that GetProductNamesAsync retrieves all product names from the page")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "Inventory")]
    public async Task GetProductNamesAsync_RetrievesAllProductNames()
    {
        // Arrange
        var mockPage = new Mock<IPage>();
        var mockLocator = new Mock<ILocator>();
        var expectedNames = new[] { "Sauce Labs Backpack", "Sauce Labs Bike Light", "Sauce Labs Bolt T-Shirt" };
        mockLocator.Setup(l => l.AllTextContentsAsync()).ReturnsAsync(expectedNames);
        mockPage.Setup(p => p.Locator(".inventory_item_name", null)).Returns(mockLocator.Object);
        var inventoryPage = new InventoryPage(mockPage.Object);

        // Act
        List<string> names = null!;
        await AllureApi.Step("Call GetProductNamesAsync", async () =>
        {
            names = await inventoryPage.GetProductNamesAsync();
        });

        // Assert
        AllureApi.Step("Verify product names list contains expected items", () =>
        {
            Assert.Equal(expectedNames.ToList(), names);
        });

        AllureApi.Step("Verify Locator was called with correct selector", () =>
        {
            mockPage.Verify(p => p.Locator(".inventory_item_name", null), Times.Once);
        });

        AllureApi.Step("Verify AllTextContentsAsync was called", () =>
        {
            mockLocator.Verify(l => l.AllTextContentsAsync(), Times.Once);
        });
    }

    [Fact]
    [AllureDescription("Verifies that AddToCartAsync clicks the correct add-to-cart button")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "Cart")]
    public async Task AddToCartAsync_ClicksCorrectButton()
    {
        // Arrange
        var mockPage = new Mock<IPage>();
        var inventoryPage = new InventoryPage(mockPage.Object);
        var productName = "sauce-labs-backpack";

        // Act
        await AllureApi.Step($"Call AddToCartAsync with product: {productName}", async () =>
        {
            await inventoryPage.AddToCartAsync(productName);
        });

        // Assert
        AllureApi.Step("Verify ClickAsync was called with correct selector", () =>
        {
            mockPage.Verify(p => p.ClickAsync("[data-test='add-to-cart-sauce-labs-backpack']", It.IsAny<PageClickOptions>()), Times.Once);
        });
    }

    [Fact]
    [AllureDescription("Verifies that RemoveFromCartAsync clicks the correct remove button")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "Cart")]
    public async Task RemoveFromCartAsync_ClicksCorrectButton()
    {
        // Arrange
        var mockPage = new Mock<IPage>();
        var inventoryPage = new InventoryPage(mockPage.Object);
        var productName = "sauce-labs-bike-light";

        // Act
        await AllureApi.Step($"Call RemoveFromCartAsync with product: {productName}", async () =>
        {
            await inventoryPage.RemoveFromCartAsync(productName);
        });

        // Assert
        AllureApi.Step("Verify ClickAsync was called with correct selector", () =>
        {
            mockPage.Verify(p => p.ClickAsync("[data-test='remove-sauce-labs-bike-light']", It.IsAny<PageClickOptions>()), Times.Once);
        });
    }

    [Fact]
    [AllureDescription("Verifies that GetCartItemCountAsync reads badge text and parses to integer")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "Cart")]
    public async Task GetCartItemCountAsync_ReadsBadgeText()
    {
        // Arrange
        var mockPage = new Mock<IPage>();
        var mockLocator = new Mock<ILocator>();
        mockLocator.Setup(l => l.IsVisibleAsync(null)).ReturnsAsync(true);
        mockLocator.Setup(l => l.TextContentAsync(null)).ReturnsAsync("3");
        mockPage.Setup(p => p.Locator(".shopping_cart_badge", null)).Returns(mockLocator.Object);
        var inventoryPage = new InventoryPage(mockPage.Object);

        // Act
        int count = 0;
        await AllureApi.Step("Call GetCartItemCountAsync", async () =>
        {
            count = await inventoryPage.GetCartItemCountAsync();
        });

        // Assert
        AllureApi.Step("Verify count is 3", () =>
        {
            Assert.Equal(3, count);
        });

        AllureApi.Step("Verify IsVisibleAsync was called", () =>
        {
            mockLocator.Verify(l => l.IsVisibleAsync(null), Times.Once);
        });

        AllureApi.Step("Verify TextContentAsync was called", () =>
        {
            mockLocator.Verify(l => l.TextContentAsync(null), Times.Once);
        });
    }

    [Fact]
    [AllureDescription("Verifies that ClickProductAsync clicks the product name link")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "Navigation")]
    public async Task ClickProductAsync_ClicksProductNameLink()
    {
        // Arrange
        var mockPage = new Mock<IPage>();
        var mockLocator = new Mock<ILocator>();
        var productName = "Sauce Labs Backpack";
        mockPage.Setup(p => p.Locator(".inventory_item_name", It.IsAny<PageLocatorOptions>())).Returns(mockLocator.Object);
        var inventoryPage = new InventoryPage(mockPage.Object);

        // Act
        await AllureApi.Step($"Call ClickProductAsync with product: {productName}", async () =>
        {
            await inventoryPage.ClickProductAsync(productName);
        });

        // Assert
        AllureApi.Step("Verify Locator was called with correct selector and text", () =>
        {
            mockPage.Verify(p => p.Locator(".inventory_item_name", It.Is<PageLocatorOptions>(o => o.HasTextString == productName)), Times.Once);
        });

        AllureApi.Step("Verify ClickAsync was called on locator", () =>
        {
            mockLocator.Verify(l => l.ClickAsync(null), Times.Once);
        });
    }

    [Fact]
    [AllureDescription("Verifies that NavigateToCartAsync clicks the cart icon link")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "Navigation")]
    public async Task NavigateToCartAsync_ClicksCartLink()
    {
        // Arrange
        var mockPage = new Mock<IPage>();
        var inventoryPage = new InventoryPage(mockPage.Object);

        // Act
        await AllureApi.Step("Call NavigateToCartAsync", async () =>
        {
            await inventoryPage.NavigateToCartAsync();
        });

        // Assert
        AllureApi.Step("Verify ClickAsync was called with correct selector", () =>
        {
            mockPage.Verify(p => p.ClickAsync(".shopping_cart_link", It.IsAny<PageClickOptions>()), Times.Once);
        });
    }

    [Fact]
    [AllureDescription("Verifies that SortProductsAsync selects the specified dropdown option")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "Sorting")]
    public async Task SortProductsAsync_SelectsDropdownOption()
    {
        // Arrange
        var mockPage = new Mock<IPage>();
        var inventoryPage = new InventoryPage(mockPage.Object);
        var sortOption = "lohi";

        // Act
        await AllureApi.Step($"Call SortProductsAsync with option: {sortOption}", async () =>
        {
            await inventoryPage.SortProductsAsync(sortOption);
        });

        // Assert
        AllureApi.Step("Verify SelectOptionAsync was called with correct selector and value", () =>
        {
            mockPage.Verify(p => p.SelectOptionAsync(".product_sort_container", sortOption, It.IsAny<PageSelectOptionOptions>()), Times.Once);
        });
    }

    [Fact]
    [AllureDescription("Verifies that IsProductInCartAsync checks if remove button is visible")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "Cart")]
    public async Task IsProductInCartAsync_ChecksButtonVisibility()
    {
        // Arrange
        var mockPage = new Mock<IPage>();
        mockPage.Setup(p => p.IsVisibleAsync("[data-test='remove-sauce-labs-backpack']", It.IsAny<PageIsVisibleOptions>())).ReturnsAsync(true);
        var inventoryPage = new InventoryPage(mockPage.Object);
        var productName = "sauce-labs-backpack";

        // Act
        bool isInCart = false;
        await AllureApi.Step($"Call IsProductInCartAsync with product: {productName}", async () =>
        {
            isInCart = await inventoryPage.IsProductInCartAsync(productName);
        });

        // Assert
        AllureApi.Step("Verify result is true", () =>
        {
            Assert.True(isInCart);
        });

        AllureApi.Step("Verify IsVisibleAsync was called with correct selector", () =>
        {
            mockPage.Verify(p => p.IsVisibleAsync("[data-test='remove-sauce-labs-backpack']", It.IsAny<PageIsVisibleOptions>()), Times.Once);
        });
    }
}
