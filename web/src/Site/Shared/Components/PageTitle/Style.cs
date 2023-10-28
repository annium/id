using Annium.Blazor.Css;

namespace Site.Shared.Components.PageTitle;

public class Style : RuleSet
{
    public CssRule Title { get; }

    public Style(Theme theme)
    {
        Title = Rule.TagClass("h2")
            .MarginTopRem(0.5)
            .MarginBottomRem(0)
            .FontFamily(theme.FontFamily)
            .FontSizeRem(1.3)
            .FontWeightNormal();
    }
}
