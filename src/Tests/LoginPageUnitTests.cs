using Microsoft.Playwright;
using Moq;
using Xunit;
using csharp_framework_demo.Utilities.PageObjects;
using Allure.Net.Commons;
using Allure.Xunit.Attributes;

namespace csharp_framework_demo.Tests;

[AllureSuite("Unit Tests")]
[AllureFeature("Login Page Object")]
public class LoginPageUnitTests
{
    [Fact]
    [AllureDescription("Verifies that NavigateToAsync calls the Playwright GotoAsync method with the correct URL")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "Navigation")]
    public async Task NavigateToAsync_CallsGotoWithCorrectUrl()
    {
        var mockPage = new Mock<IPage>();
        var loginPage = new LoginPage(mockPage.Object);
        var expectedUrl = "https://www.saucedemo.com/";

        await AllureApi.Step($"Call NavigateToAsync with URL: {expectedUrl}", async () =>
        {
            await loginPage.NavigateToAsync(expectedUrl);
        });

        AllureApi.Step("Verify GotoAsync was called once with correct URL", () =>
        {
            mockPage.Verify(p => p.GotoAsync(expectedUrl, It.IsAny<PageGotoOptions>()), Times.Once);
        });
    }

    [Fact]
    [AllureDescription("Verifies that LoginAsync fills username and password fields and clicks login button with correct selectors")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Unit", "Login")]
    public async Task LoginAsync_FillsAndClicksCorrectElements()
    {
        var mockPage = new Mock<IPage>();
        var loginPage = new LoginPage(mockPage.Object);
        var username = "test_user";
        var password = "test_pass";

        await AllureApi.Step($"Call LoginAsync with username: {username}", async () =>
        {
            await loginPage.LoginAsync(username, password);
        });

        AllureApi.Step("Verify username field was filled", () =>
        {
            mockPage.Verify(p => p.FillAsync("[data-test='username']", username, It.IsAny<PageFillOptions>()), Times.Once);
        });

        AllureApi.Step("Verify password field was filled", () =>
        {
            mockPage.Verify(p => p.FillAsync("[data-test='password']", password, It.IsAny<PageFillOptions>()), Times.Once);
        });

        AllureApi.Step("Verify login button was clicked", () =>
        {
            mockPage.Verify(p => p.ClickAsync("[data-test='login-button']", It.IsAny<PageClickOptions>()), Times.Once);
        });
    }
}