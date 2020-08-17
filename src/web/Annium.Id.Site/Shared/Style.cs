using Annium.Blazor.Css;

namespace Annium.Id.Site.Shared
{
    public class Style : IRuleSet
    {
        private readonly CssRule _html;

        public Style(Theme theme)
        {
            _html = Rule.Tag("html")
                .FlexColumn(AlignItems.Stretch, JustifyContent.FlexStart)
                .WidthPercent(100)
                .WidthPercent(100)
                .MinHeight("100vh")
                .FontFamily(theme.FontFamily)
                .FontWeightNormal();
        }
    }
}