using Microsoft.Playwright;

namespace csharp_framework_demo.Utilities.PageObjects;

public class BurgerMenuPage
{
    private readonly IPage _page;

    public BurgerMenuPage(IPage page)
    {
        _page = page;
    }

    public async Task OpenMenuAsync()
    {
        await _page.ClickAsync("#react-burger-menu-btn");
    }

    public async Task CloseMenuAsync()
    {
        await _page.ClickAsync("#react-burger-cross-btn");
    }

    public async Task<bool> IsMenuOpenAsync()
    {
        var closeButton = _page.Locator("#react-burger-cross-btn");
        return await closeButton.IsVisibleAsync();
    }

    public async Task ClickAllItemsAsync()
    {
        await _page.ClickAsync("#inventory_sidebar_link");
    }

    public async Task ClickAboutAsync()
    {
        await _page.ClickAsync("#about_sidebar_link");
    }

    public async Task ClickLogoutAsync()
    {
        await _page.ClickAsync("#logout_sidebar_link");
    }

    public async Task ClickResetAppAsync()
    {
        await _page.ClickAsync("#reset_sidebar_link");
    }

    public async Task LogoutAsync()
    {
        await OpenMenuAsync();
        await ClickLogoutAsync();
    }

    public async Task<bool> IsLogoutLinkVisibleAsync()
    {
        return await _page.IsVisibleAsync("#logout_sidebar_link");
    }
}
