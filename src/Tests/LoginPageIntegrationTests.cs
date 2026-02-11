using Microsoft.Playwright;
using Xunit;
using csharp_framework_demo.Utilities.PageObjects;
using Allure.Net.Commons;
using Allure.Xunit.Attributes;

namespace csharp_framework_demo.Tests;

[AllureSuite("Integration Tests")]
[AllureFeature("Login Page Object")]
public class LoginPageIntegrationTests : IClassFixture<PlaywrightFixture>
{
    private readonly PlaywrightFixture _fixture;

    public LoginPageIntegrationTests(PlaywrightFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    [AllureDescription("Verifies that LoginPage integrates correctly with Playwright Page and navigates successfully to inventory after login")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "Login")]
    public async Task LoginAsync_IntegratesWithPageAndNavigatesSuccessfully()
    {
        await using var pageContext = await _fixture.CreatePageContextAsync();
        var page = pageContext.Page;
        var loginPage = new LoginPage(page);

        await AllureApi.Step("Navigate to SauceDemo login page", async () =>
        {
            await loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Perform login with valid credentials", async () =>
        {
            await loginPage.LoginAsync("standard_user", "secret_sauce");
        });

        AllureApi.Step("Verify URL changed to inventory page", () =>
        {
            Assert.Equal("https://www.saucedemo.com/inventory.html", page.Url);
        });

        await AllureApi.Step("Verify inventory container is visible on page", async () =>
        {
            var inventoryVisible = await page.IsVisibleAsync(".inventory_container");
            Assert.True(inventoryVisible);
        });
    }

    [Fact]
    [AllureDescription("Verifies that LoginPage handles invalid credentials correctly and displays error message")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("Integration", "Validation", "Negative")]
    public async Task LoginAsync_HandlesInvalidCredentialsAndDisplaysError()
    {
        await using var pageContext = await _fixture.CreatePageContextAsync();
        var page = pageContext.Page;
        var loginPage = new LoginPage(page);

        await AllureApi.Step("Navigate to SauceDemo login page", async () =>
        {
            await loginPage.NavigateToAsync("https://www.saucedemo.com/");
        });

        await AllureApi.Step("Attempt login with invalid credentials", async () =>
        {
            await loginPage.LoginAsync("invalid_user", "wrong_password");
        });

        await AllureApi.Step("Verify error message is displayed", async () =>
        {
            var errorVisible = await page.IsVisibleAsync("[data-test='error']");
            Assert.True(errorVisible);
        });
    }
}
