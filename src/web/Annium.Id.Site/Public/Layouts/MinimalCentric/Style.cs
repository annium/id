using Annium.Blazor.Css;

namespace Annium.Id.Site.Public.Layouts.MinimalCentric
{
    public class Style : IRuleSet
    {
        public readonly CssRule Column;

        public readonly CssRule Container;

        public readonly CssRule Logo;

        public readonly CssRule Credentials;
        public readonly CssRule Link;

        public Style(Theme theme)
        {
            Column = Rule.Class()
                .Margin("0", "auto")
                .Media(theme.Media.Xs, rule => rule.WidthRem(26))
                .Media(theme.Media.Sm, rule => rule.WidthRem(24))
                .Media(theme.Media.Md, rule => rule.WidthRem(22))
                .Media(theme.Media.Lg, rule => rule.WidthRem(20))
                .Media(theme.Media.Xl, rule => rule.WidthRem(18))
                .Media(theme.Media.Xxl, rule => rule.WidthRem(16));
            Container = Rule.Class()
                .FlexColumn(AlignItems.Center, JustifyContent.FlexStart)
                .MarginTopRem(6)
                .PaddingRem(1.5)
                .BorderRadiusRem(1)
                .BackgroundColor(theme.Palette.Paper)
                .FontSizeRem(1);
            Logo = Rule.Class()
                .FontSizeRem(3);
            Credentials = Rule.Class()
                .MarginTopRem(1)
                .Color(theme.Palette.Gray8)
                .TextAlign(TextAlign.Center);
            Link = Rule.Class()
                .Color(theme.Palette.Gray8);
        }
    }
}