using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using SmartStock.Shared.Resources;

namespace SmartStock.Frontend.Shared;

public partial class ConfirmDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Parameter] public string Message { get; set; } = null!;

    private void Accept()
    {
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel()
    {
        MudDialog.Close(DialogResult.Cancel());
    }
}