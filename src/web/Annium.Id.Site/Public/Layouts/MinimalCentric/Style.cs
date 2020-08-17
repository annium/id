using Annium.Blazor.Css;

namespace Annium.Id.Site.Public.Layouts.MinimalCentric
{
    public class Style : IRuleSet
    {
        public readonly CssRule Container;

        public readonly CssRule Logo = Rule.Class()
            .FontSizeRem(3);

        public readonly CssRule Credentials;
        public readonly CssRule Link;

        public Style(Theme theme)
        {
            Container = Rule.Class()
                .FlexColumn(AlignItems.Center, JustifyContent.FlexStart)
                .MarginTopRem(6)
                .PaddingRem(1.5)
                .BorderRadiusRem(1)
                .BackgroundColor(theme.Palette.Paper)
                .FontSizeRem(1);
            Credentials = Rule.Class()
                .MarginTopRem(1)
                .Color(theme.Palette.Gray8)
                .TextAlign(TextAlign.Center);
            Link = Rule.Class()
                .Color(theme.Palette.Gray8);
        }
    }
}