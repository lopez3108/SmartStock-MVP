using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using SmartStock.Frontend.Repositories;
using SmartStock.Frontend.Shared;
using SmartStock.Shared.Entites;
using SmartStock.Shared.Resources;
using System.Diagnostics.Metrics;
using System.Net;

namespace SmartStock.Frontend.Pages.Categories;

public partial class CategoriesIndex
{
    //    [Inject] private IRepository Repository { get; set; } = null!;
    //    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    //    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    //    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

    //    private List<Category>? Categories { get; set; }

    //    protected override async Task OnInitializedAsync()
    //    {
    //        await LoadAsync();
    //    }

    //    /// <summary>
    //    /// Get Async categories
    //    /// </summary>
    //    /// <returns></returns>
    //    ///
    //    private async Task LoadAsync()
    //    {
    //        var responseHppt = await Repository.GetAsync<List<Category>>("api/categories");
    //        if (responseHppt.Error)
    //        {
    //            var message = await responseHppt.GetErrorMessageAsync();
    //            await SweetAlertService.FireAsync(Localizer["Error"], message, SweetAlertIcon.Error);
    //            return;
    //        }
    //        Categories = responseHppt.Response!;
    //    }

    //    /// <summary>
    //    /// Delete Async category
    //    /// </summary>
    //    /// <param name="category"></param>
    //    /// <returns></returns>
    //    ///
    //    private async Task DeleteAsync(Category category)
    //    {
    //        var result = await SweetAlertService.FireAsync(new SweetAlertOptions
    //        {
    //            Title = Localizer["confirmation"],
    //            Text = string.Format(Localizer["delete_confirm"], Localizer["category"], category.CategoryName),
    //            Icon = SweetAlertIcon.Question,
    //            ShowCancelButton = true,
    //            CancelButtonText = Localizer["cancel"]
    //        });

    //        var confirm = string.IsNullOrEmpty(result.Value);

    //        if (confirm)
    //        {
    //            return;
    //        }

    //        var responseHttp = await Repository.DeleteAsync($"api/categories/{category.CategoryId}");
    //        if (responseHttp.Error)
    //        {
    //            if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
    //            {
    //                NavigationManager.NavigateTo("/");
    //            }
    //            else
    //            {
    //                var menssageError = await responseHttp.GetErrorMessageAsync();
    //                await SweetAlertService.FireAsync(Localizer["Error"], Localizer[menssageError!], SweetAlertIcon.Error);
    //            }
    //            return;
    //        }

    //        await LoadAsync();
    //        var toast = SweetAlertService.Mixin(new SweetAlertOptions
    //        {
    //            Toast = true,
    //            Position = SweetAlertPosition.BottomEnd,
    //            ShowConfirmButton = true,
    //            Timer = 3000,
    //            ConfirmButtonText = Localizer["yes"]
    //        });
    //        await toast.FireAsync(icon: SweetAlertIcon.Success, message: Localizer["record_deleted_ok"]);
    //    }
    //}

    private List<Category>? Categories { get; set; }
    private MudTable<Category> table = new();
    private readonly int[] pageSizeOptions = { 10, 25, 50, int.MaxValue };
    private int totalRecords = 0;
    private bool loading;
    private const string baseUrl = "api/categories";
    private string infoFormat = "{first_item}-{last_item} => {all_items}";

    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    [Parameter, SupplyParameterFromQuery] public string Filter { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        await LoadTotalRecordsAsync();
    }

    private async Task LoadTotalRecordsAsync()
    {
        loading = true;
        var url = $"{baseUrl}/totalRecordsPaginated";

        if (!string.IsNullOrWhiteSpace(Filter))
        {
            url += $"?filter={Filter}";
        }

        var responseHttp = await Repository.GetAsync<int>(url);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[message!], Severity.Error);
            return;
        }

        totalRecords = responseHttp.Response;
        loading = false;
    }

    private async Task<TableData<Category>> LoadListAsync(TableState state, CancellationToken cancellationToken)
    {
        int page = state.Page + 1;
        int pageSize = state.PageSize;
        var url = $"{baseUrl}/paginated/?page={page}&recordsnumber={pageSize}";

        if (!string.IsNullOrWhiteSpace(Filter))
        {
            url += $"&filter={Filter}";
        }

        var responseHttp = await Repository.GetAsync<List<Category>>(url);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[message!], Severity.Error);
            return new TableData<Category> { Items = [], TotalItems = 0 };
        }
        if (responseHttp.Response == null)
        {
            return new TableData<Category> { Items = [], TotalItems = 0 };
        }
        return new TableData<Category>
        {
            Items = responseHttp.Response,
            TotalItems = totalRecords
        };
    }

    private async Task SetFilterValue(string value)
    {
        Filter = value;
        await LoadTotalRecordsAsync();
        await table.ReloadServerData();
    }

    private async Task ShowModalAsync(int id = 0, bool isEdit = false)
    {
        var options = new DialogOptions() { CloseOnEscapeKey = true, CloseButton = true };
        IDialogReference? dialog;
        if (isEdit)
        {
            var parameters = new DialogParameters
                {
                    { "Id", id }
                };
            dialog = await DialogService.ShowAsync<CategoryEdit>($"{Localizer["edit"]} {Localizer["category"]}", parameters, options);
        }
        else
        {
            dialog = await DialogService.ShowAsync<CategoryCreate>($"{Localizer["new"]} {Localizer["category"]}", options);
        }

        var result = await dialog.Result;
        if (result!.Canceled)
        {
            await LoadTotalRecordsAsync();
            await table.ReloadServerData();
        }
    }

    private async Task DeleteAsync(Category category)
    {
        var parameters = new DialogParameters
            {
                { "Message", string.Format(Localizer["delete_confirm"], Localizer["category"], category.CategoryName) }
            };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall, CloseOnEscapeKey = true };
        var dialog = await DialogService.ShowAsync<ConfirmDialog>(Localizer["confirmation"], parameters, options);
        var result = await dialog.Result;
        if (result!.Canceled)
        {
            return;
        }

        var responseHttp = await Repository.DeleteAsync($"{baseUrl}/{category.CategoryId}");
        if (responseHttp.Error)
        {
            if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                NavigationManager.NavigateTo("/categories");
            }
            else
            {
                var message = await responseHttp.GetErrorMessageAsync();
                Snackbar.Add(Localizer[message!], Severity.Error);
            }
            return;
        }
        await LoadTotalRecordsAsync();
        await table.ReloadServerData();
        Snackbar.Add(Localizer["record_deleted_ok"], Severity.Success);
    }
}