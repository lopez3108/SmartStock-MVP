using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using SmartStock.Frontend.Repositories;
using SmartStock.Shared.Entites;
using SmartStock.Shared.Resources;

namespace SmartStock.Frontend.Pages.Categories;

public partial class CategoriesIndex
{
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private IRepository Repository { get; set; } = null!;

    private List<Category>? Categories { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var responseHppt = await Repository.GetAsync<List<Category>>("api/categories");
        Categories = responseHppt.Response!;
    }
}