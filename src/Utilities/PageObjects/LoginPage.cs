using Microsoft.Playwright;
namespace csharp_framework_demo.Utilities.PageObjects;
public class LoginPage
{
    private readonly IPage _page;
    public LoginPage(IPage page)
    {
        _page = page;
    }
    public async Task NavigateToAsync(string url)
    {
        await _page.GotoAsync(url);
    }
    public async Task LoginAsync(string username, string password)
    {
        await _page.FillAsync("[data-test='username']", username);
        await _page.FillAsync("[data-test='password']", password);
        await _page.ClickAsync("[data-test='login-button']");
    }
}