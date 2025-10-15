using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using SmartStock.Frontend.Pages.Products;
using SmartStock.Frontend.Repositories;
using SmartStock.Shared.Entities;
using SmartStock.Shared.Resources;
using System.Diagnostics.Metrics;

namespace SmartStock.Frontend.Pages.Categories;

public partial class CategoryCreate
{
    private CategoryForm? categoryForm;

    private Category category = new();

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

    private async Task CreateAsync()
    {
        var responseHttp = await Repository.PostAsync("/api/categories", category);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[message!], Severity.Error);
            return;
        }

        Return();
        Snackbar.Add(Localizer["record_created_ok"], Severity.Success);
    }

    private void Return()
    {
        categoryForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo("/categories");
    }
}