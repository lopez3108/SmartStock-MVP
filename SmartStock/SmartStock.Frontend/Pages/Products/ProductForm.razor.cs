using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Localization;
using SmartStock.Frontend.Repositories;
using SmartStock.Shared.DTOs;
using SmartStock.Shared.Entites;
using SmartStock.Shared.Resources;
using System.Diagnostics.Metrics;

namespace SmartStock.Frontend.Pages.Products;

public partial class ProductForm
{
    private EditContext editContext = null!;

    protected override void OnInitialized()
    {
        editContext = new(ProductDTO);
    }

    [EditorRequired, Parameter] public ProductDTO ProductDTO { get; set; } = null!;
    [EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }
    [EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }

    public bool FormPostedSuccessfully { get; set; } = false;

    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private IRepository Repository { get; set; } = null!;

    private List<Category>? categories;
    private string? imageUrl;

    protected override async Task OnInitializedAsync()
    {
        await LoadCategoriesAsync();
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        if (!string.IsNullOrEmpty(ProductDTO.Image))
        {
            imageUrl = ProductDTO.Image;
            ProductDTO.Image = null;
        }
    }

    private async Task LoadCategoriesAsync()
    {
        var responseHttp = await Repository.GetAsync<List<Category>>("/api/categories/combo");
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        categories = responseHttp.Response;
    }

    private void ImageSelected(string imagenBase64)
    {
        ProductDTO.Image = imagenBase64;
        imageUrl = null;
    }

    private async Task OnBeforeInternalNavigation(LocationChangingContext context)
    {
        var formWasEdited = editContext.IsModified();

        if (!formWasEdited || FormPostedSuccessfully)
        {
            return;
        }

        var result = await SweetAlertService.FireAsync(new SweetAlertOptions
        {
            Title = Localizer["confirmation"],
            Text = Localizer["leave_and_lose_changes"],
            Icon = SweetAlertIcon.Warning,
            ShowCancelButton = true,
            CancelButtonText = Localizer["cancel"],
        });

        var confirm = !string.IsNullOrEmpty(result.Value);
        if (confirm)
        {
            return;
        }

        context.PreventNavigation();
    }
}