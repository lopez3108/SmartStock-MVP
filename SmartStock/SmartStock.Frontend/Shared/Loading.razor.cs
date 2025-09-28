using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using SmartStock.Shared.Resources;

namespace SmartStock.Frontend.Shared;

public partial class Loading
{
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Parameter] public string? Label { get; set; }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        if (string.IsNullOrEmpty(Label))
        {
            Label = Localizer["please_wait"];
        }
    }
}