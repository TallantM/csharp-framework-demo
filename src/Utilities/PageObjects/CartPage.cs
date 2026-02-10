using Microsoft.Playwright;

namespace csharp_framework_demo.Utilities.PageObjects;

public class CartPage
{
    private readonly IPage _page;

    public CartPage(IPage page)
    {
        _page = page;
    }

    public async Task<int> GetCartItemCountAsync()
    {
        return await _page.Locator(".cart_item").CountAsync();
    }

    public async Task<List<string>> GetCartItemNamesAsync()
    {
        var names = await _page.Locator(".cart_item .inventory_item_name").AllTextContentsAsync();
        return names.ToList();
    }

    public async Task RemoveItemAsync(string productName)
    {
        await _page.ClickAsync($"[data-test='remove-{productName}']");
    }

    public async Task<bool> IsItemInCartAsync(string productName)
    {
        var items = await GetCartItemNamesAsync();
        return items.Contains(productName);
    }

    public async Task<string> GetItemPriceAsync(string productName)
    {
        var cartItems = _page.Locator(".cart_item");
        var count = await cartItems.CountAsync();

        for (int i = 0; i < count; i++)
        {
            var item = cartItems.Nth(i);
            var name = await item.Locator(".inventory_item_name").TextContentAsync();

            if (name == productName)
            {
                var price = await item.Locator(".inventory_item_price").TextContentAsync();
                return price ?? "";
            }
        }

        return "";
    }

    public async Task ClickContinueShoppingAsync()
    {
        await _page.ClickAsync("[data-test='continue-shopping']");
    }

    public async Task ClickCheckoutAsync()
    {
        await _page.ClickAsync("[data-test='checkout']");
    }

    public async Task<bool> IsCartEmptyAsync()
    {
        var count = await GetCartItemCountAsync();
        return count == 0;
    }

    public async Task ClickProductNameAsync(string productName)
    {
        await _page.Locator(".cart_item .inventory_item_name", new PageLocatorOptions { HasTextString = productName }).ClickAsync();
    }
}
