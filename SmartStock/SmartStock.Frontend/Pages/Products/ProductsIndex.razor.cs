using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using SmartStock.Frontend.Repositories;
using SmartStock.Shared.Entites;
using SmartStock.Shared.Resources;
using System.Diagnostics.Metrics;

namespace SmartStock.Frontend.Pages.Products;

public partial class ProductsIndex
{
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private IRepository Repository { get; set; } = null!;

    private List<Product>? Products { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var responseHppt = await Repository.GetAsync<List<Product>>("api/products");
        Products = responseHppt.Response!;
    }
}