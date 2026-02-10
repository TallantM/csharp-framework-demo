using Microsoft.Playwright;

namespace csharp_framework_demo.Utilities.PageObjects;

public class InventoryPage
{
    private readonly IPage _page;

    public InventoryPage(IPage page)
    {
        _page = page;
    }

    public async Task<int> GetProductCountAsync()
    {
        return await _page.Locator(".inventory_item").CountAsync();
    }

    public async Task<List<string>> GetProductNamesAsync()
    {
        var names = await _page.Locator(".inventory_item_name").AllTextContentsAsync();
        return names.ToList();
    }

    public async Task AddToCartAsync(string productName)
    {
        await _page.ClickAsync($"[data-test='add-to-cart-{productName}']");
    }

    public async Task RemoveFromCartAsync(string productName)
    {
        await _page.ClickAsync($"[data-test='remove-{productName}']");
    }

    public async Task<int> GetCartItemCountAsync()
    {
        var badge = _page.Locator(".shopping_cart_badge");
        var isVisible = await badge.IsVisibleAsync();

        if (!isVisible)
        {
            return 0;
        }

        var text = await badge.TextContentAsync();
        return int.Parse(text ?? "0");
    }

    public async Task ClickProductAsync(string productName)
    {
        await _page.Locator(".inventory_item_name", new PageLocatorOptions { HasTextString = productName }).ClickAsync();
    }

    public async Task NavigateToCartAsync()
    {
        await _page.ClickAsync(".shopping_cart_link");
    }

    public async Task SortProductsAsync(string sortOption)
    {
        await _page.SelectOptionAsync(".product_sort_container", sortOption);
    }

    public async Task<bool> IsProductInCartAsync(string productName)
    {
        return await _page.IsVisibleAsync($"[data-test='remove-{productName}']");
    }
}
