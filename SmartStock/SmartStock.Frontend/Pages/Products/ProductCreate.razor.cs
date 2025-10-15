using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using SmartStock.Frontend.Repositories;
using SmartStock.Shared.DTOs;
using SmartStock.Shared.Resources;

namespace SmartStock.Frontend.Pages.Products;

public partial class ProductCreate
{   

    private ProductForm? productForm;
    private ProductDTO productDTO = new();

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

    private async Task CreateAsync()
    {
        var responseHttp = await Repository.PostAsync("/api/products/full", productDTO);
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
        productForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo("/products");
    }
}