using Annium.Blazor.Css;

namespace Annium.Id.Site.Public.Pages.Login;

public class Style : RuleSet
{
    public CssRule Title { get; } = Rule.Class()
        .MarginTopRem(1);

    public CssRule Form { get; } = Rule.Class()
        .WidthPercent(100)
        .MarginTopRem(1);

    public CssRule FormItem { get; } = Rule.Class()
        .FontSizeRem(1)
        .MarginBottomRem(0)
        .Inheritor("input", input => input.FontSizeRem(1));

    public CssRule Links { get; } = Rule.Class()
        .FlexRow(AlignItems.Center, JustifyContent.SpaceBetween)
        .FontSizeRem(0.8)
        .MarginTopRem(1);
}