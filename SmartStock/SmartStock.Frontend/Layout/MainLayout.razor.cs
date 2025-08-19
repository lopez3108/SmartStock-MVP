using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using SmartStock.Shared.Resources;

namespace SmartStock.Frontend.Layout;

public partial class MainLayout
{
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
}