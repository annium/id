using Annium.Blazor.Css;

namespace Annium.Id.Site.Public.Pages.Login
{
    public class Style : IRuleSet
    {
        public CssRule Title { get; } = Rule.Class()
            .MarginTopRem(1);

        public CssRule Form { get; } = Rule.Class()
            .WidthPercent(100)
            .MarginTopRem(1);

        public CssRule FormItem { get; } = Rule.Class()
            .MarginBottomRem(0);
    }
}