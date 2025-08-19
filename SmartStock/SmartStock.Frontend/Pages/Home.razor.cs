using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using SmartStock.Shared.Resources;

namespace SmartStock.Frontend.Pages;

public partial class Home
{
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
}