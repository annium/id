using Annium.Blazor.Css;

namespace Site.Shared;

public class Style : RuleSet
{
    public readonly CssRule Html;

    public Style(Theme theme)
    {
        Html = Rule.Tag("html").FontFamily(theme.FontFamily).FontWeightNormal();
    }
}
