using Microsoft.Playwright;
using Moq;
using Xunit;
using PlaywrightDemo.Utilities.PageObjects;

namespace PlaywrightDemo.Tests;

public class LoginPageUnitTests
{
    [Fact]
    public async Task NavigateToAsync_CallsGotoWithCorrectUrl()
    {
        var mockPage = new Mock<IPage>();
        var loginPage = new LoginPage(mockPage.Object);
        var expectedUrl = "https://www.saucedemo.com/";

        await loginPage.NavigateToAsync(expectedUrl);

        mockPage.Verify(p => p.GotoAsync(expectedUrl, It.IsAny<PageGotoOptions>()), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_FillsAndClicksCorrectElements()
    {
        var mockPage = new Mock<IPage>();
        var loginPage = new LoginPage(mockPage.Object);
        var username = "test_user";
        var password = "test_pass";

        await loginPage.LoginAsync(username, password);

        mockPage.Verify(p => p.FillAsync("[data-test='username']", username, It.IsAny<PageFillOptions>()), Times.Once);
        mockPage.Verify(p => p.FillAsync("[data-test='password']", password, It.IsAny<PageFillOptions>()), Times.Once);
        mockPage.Verify(p => p.ClickAsync("[data-test='login-button']", It.IsAny<PageClickOptions>()), Times.Once);
    }
}