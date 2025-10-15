using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Localization;
using SmartStock.Frontend.Repositories;
using SmartStock.Shared.DTOs;
using SmartStock.Shared.Entities;
using SmartStock.Shared.Resources;
using System.Diagnostics.Metrics;

namespace SmartStock.Frontend.Pages.Products;

public partial class ProductForm
{
    private EditContext editContext = null!;

    private Category selectedCategory = new();
    private List<Category>? categories;
    private string? imageUrl;
    private string? shapeImageMessage;

    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private IRepository Repository { get; set; } = null!;

    [EditorRequired, Parameter] public ProductDTO ProductDTO { get; set; } = null!;
    [EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }
    [EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }

    public bool FormPostedSuccessfully { get; set; } = false;

    protected override void OnInitialized()
    {
        editContext = new(ProductDTO);
    }

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
        //shapeImageMessage = ProductDTO.IsImageSquare ? Localizer["ImageIsSquare"] : Localizer["ImageIsRectangular"];
    }

    //private void OnToggledChanged(bool toggled)
    //{
    //    ProductDTO.IsImageSquare = toggled;
    //    shapeImageMessage = ProductDTO.IsImageSquare ? Localizer["ImageIsSquare"] : Localizer["ImageIsRectangular"];
    //}

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

    private async Task<IEnumerable<Category>> SearchCategory(string searchText, CancellationToken cancellationToken)
    {
        await Task.Delay(5);
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return categories!;
        }

        return categories!
            .Where(x => x.CategoryName.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }

    private void CategoryChanged(Category category)
    {
        selectedCategory = category;
        ProductDTO.CategoryId = category.CategoryId;
    }
}