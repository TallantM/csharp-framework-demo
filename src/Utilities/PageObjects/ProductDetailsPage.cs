using Microsoft.Playwright;

namespace csharp_framework_demo.Utilities.PageObjects;

public class ProductDetailsPage
{
    private readonly IPage _page;

    public ProductDetailsPage(IPage page)
    {
        _page = page;
    }

    public async Task<string> GetProductNameAsync()
    {
        return await _page.Locator(".inventory_details_name").TextContentAsync() ?? "";
    }

    public async Task<string> GetProductDescriptionAsync()
    {
        return await _page.Locator(".inventory_details_desc").TextContentAsync() ?? "";
    }

    public async Task<string> GetProductPriceAsync()
    {
        return await _page.Locator(".inventory_details_price").TextContentAsync() ?? "";
    }

    public async Task AddToCartAsync()
    {
        await _page.ClickAsync("[data-test='add-to-cart']");
    }

    public async Task RemoveFromCartAsync()
    {
        await _page.ClickAsync("[data-test='remove']");
    }

    public async Task<bool> IsProductInCartAsync()
    {
        return await _page.IsVisibleAsync("[data-test='remove']");
    }

    public async Task ClickBackToProductsAsync()
    {
        await _page.ClickAsync("[data-test='back-to-products']");
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

    public async Task<bool> IsImageVisibleAsync()
    {
        return await _page.IsVisibleAsync(".inventory_details_img");
    }
}
