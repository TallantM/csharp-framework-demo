using Microsoft.Playwright;

namespace csharp_framework_demo.Utilities.PageObjects;

public class CheckoutPage
{
    private readonly IPage _page;

    public CheckoutPage(IPage page)
    {
        _page = page;
    }

    public async Task FillCheckoutInformationAsync(string firstName, string lastName, string postalCode)
    {
        await _page.FillAsync("[data-test='firstName']", firstName);
        await _page.FillAsync("[data-test='lastName']", lastName);
        await _page.FillAsync("[data-test='postalCode']", postalCode);
    }

    public async Task ClickContinueAsync()
    {
        await _page.ClickAsync("[data-test='continue']");
    }

    public async Task ClickCancelAsync()
    {
        await _page.ClickAsync("[data-test='cancel']");
    }

    public async Task<string> GetSubtotalAsync()
    {
        return await _page.Locator(".summary_subtotal_label").TextContentAsync() ?? "";
    }

    public async Task<string> GetTaxAsync()
    {
        return await _page.Locator(".summary_tax_label").TextContentAsync() ?? "";
    }

    public async Task<string> GetTotalAsync()
    {
        return await _page.Locator(".summary_total_label").TextContentAsync() ?? "";
    }

    public async Task ClickFinishAsync()
    {
        await _page.ClickAsync("[data-test='finish']");
    }

    public async Task<string> GetConfirmationMessageAsync()
    {
        return await _page.Locator(".complete-header").TextContentAsync() ?? "";
    }

    public async Task<string> GetConfirmationDetailsAsync()
    {
        return await _page.Locator(".complete-text").TextContentAsync() ?? "";
    }

    public async Task ClickBackToProductsAsync()
    {
        await _page.ClickAsync("[data-test='back-to-products']");
    }

    public async Task<bool> IsErrorVisibleAsync()
    {
        return await _page.IsVisibleAsync("[data-test='error']");
    }

    public async Task<string> GetErrorMessageAsync()
    {
        return await _page.Locator("[data-test='error']").TextContentAsync() ?? "";
    }
}
