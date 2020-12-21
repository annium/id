using Annium.Blazor.Css;

namespace Annium.Id.Site.Shared
{
    public class Style : RuleSet
    {
        private readonly CssRule _html;

        public Style(Theme theme)
        {
            _html = Rule.Tag("html")
                .FontFamily(theme.FontFamily)
                .FontWeightNormal();
        }
    }
}