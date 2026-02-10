using Microsoft.Playwright;
using Moq;
using Xunit;
using csharp_framework_demo.Utilities.PageObjects;
using Allure.Net.Commons;
using Allure.Xunit.Attributes;

namespace csharp_framework_demo.Tests;

[AllureSuite("Unit Tests")]
[AllureFeature("Burger Menu Page Object")]
public class BurgerMenuPageTests
{
    [Fact]
    [AllureDescription("Verifies that OpenMenuAsync clicks the burger menu button")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "BurgerMenu")]
    public async Task OpenMenuAsync_ClicksMenuButton()
    {
        var mockPage = new Mock<IPage>();
        var burgerMenuPage = new BurgerMenuPage(mockPage.Object);

        await AllureApi.Step("Call OpenMenuAsync", async () =>
        {
            await burgerMenuPage.OpenMenuAsync();
        });

        AllureApi.Step("Verify ClickAsync was called with correct selector", () =>
        {
            mockPage.Verify(p => p.ClickAsync("#react-burger-menu-btn", It.IsAny<PageClickOptions>()), Times.Once);
        });
    }

    [Fact]
    [AllureDescription("Verifies that CloseMenuAsync clicks the close button")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "BurgerMenu")]
    public async Task CloseMenuAsync_ClicksCloseButton()
    {
        var mockPage = new Mock<IPage>();
        var burgerMenuPage = new BurgerMenuPage(mockPage.Object);

        await AllureApi.Step("Call CloseMenuAsync", async () =>
        {
            await burgerMenuPage.CloseMenuAsync();
        });

        AllureApi.Step("Verify ClickAsync was called with correct selector", () =>
        {
            mockPage.Verify(p => p.ClickAsync("#react-burger-cross-btn", It.IsAny<PageClickOptions>()), Times.Once);
        });
    }

    [Fact]
    [AllureDescription("Verifies that IsMenuOpenAsync checks menu visibility")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "BurgerMenu")]
    public async Task IsMenuOpenAsync_ChecksMenuVisibility()
    {
        var mockPage = new Mock<IPage>();
        var mockLocator = new Mock<ILocator>();
        mockLocator.Setup(l => l.IsVisibleAsync(null)).ReturnsAsync(true);
        mockPage.Setup(p => p.Locator(".bm-menu", null)).Returns(mockLocator.Object);
        var burgerMenuPage = new BurgerMenuPage(mockPage.Object);

        bool isOpen = false;
        await AllureApi.Step("Call IsMenuOpenAsync", async () =>
        {
            isOpen = await burgerMenuPage.IsMenuOpenAsync();
        });

        AllureApi.Step("Verify result is true", () =>
        {
            Assert.True(isOpen);
        });
    }

    [Fact]
    [AllureDescription("Verifies that ClickLogoutAsync clicks the logout link")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "BurgerMenu")]
    public async Task ClickLogoutAsync_ClicksLogoutLink()
    {
        var mockPage = new Mock<IPage>();
        var burgerMenuPage = new BurgerMenuPage(mockPage.Object);

        await AllureApi.Step("Call ClickLogoutAsync", async () =>
        {
            await burgerMenuPage.ClickLogoutAsync();
        });

        AllureApi.Step("Verify ClickAsync was called with correct selector", () =>
        {
            mockPage.Verify(p => p.ClickAsync("#logout_sidebar_link", It.IsAny<PageClickOptions>()), Times.Once);
        });
    }

    [Fact]
    [AllureDescription("Verifies that ClickAllItemsAsync clicks the all items link")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "BurgerMenu")]
    public async Task ClickAllItemsAsync_ClicksAllItemsLink()
    {
        var mockPage = new Mock<IPage>();
        var burgerMenuPage = new BurgerMenuPage(mockPage.Object);

        await AllureApi.Step("Call ClickAllItemsAsync", async () =>
        {
            await burgerMenuPage.ClickAllItemsAsync();
        });

        AllureApi.Step("Verify ClickAsync was called with correct selector", () =>
        {
            mockPage.Verify(p => p.ClickAsync("#inventory_sidebar_link", It.IsAny<PageClickOptions>()), Times.Once);
        });
    }

    [Fact]
    [AllureDescription("Verifies that ClickResetAppAsync clicks the reset app link")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "BurgerMenu")]
    public async Task ClickResetAppAsync_ClicksResetLink()
    {
        var mockPage = new Mock<IPage>();
        var burgerMenuPage = new BurgerMenuPage(mockPage.Object);

        await AllureApi.Step("Call ClickResetAppAsync", async () =>
        {
            await burgerMenuPage.ClickResetAppAsync();
        });

        AllureApi.Step("Verify ClickAsync was called with correct selector", () =>
        {
            mockPage.Verify(p => p.ClickAsync("#reset_sidebar_link", It.IsAny<PageClickOptions>()), Times.Once);
        });
    }

    [Fact]
    [AllureDescription("Verifies that LogoutAsync opens menu and clicks logout")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "BurgerMenu")]
    public async Task LogoutAsync_OpensMenuAndClicksLogout()
    {
        var mockPage = new Mock<IPage>();
        var burgerMenuPage = new BurgerMenuPage(mockPage.Object);

        await AllureApi.Step("Call LogoutAsync", async () =>
        {
            await burgerMenuPage.LogoutAsync();
        });

        AllureApi.Step("Verify menu button was clicked", () =>
        {
            mockPage.Verify(p => p.ClickAsync("#react-burger-menu-btn", It.IsAny<PageClickOptions>()), Times.Once);
        });

        AllureApi.Step("Verify logout link was clicked", () =>
        {
            mockPage.Verify(p => p.ClickAsync("#logout_sidebar_link", It.IsAny<PageClickOptions>()), Times.Once);
        });
    }
}
