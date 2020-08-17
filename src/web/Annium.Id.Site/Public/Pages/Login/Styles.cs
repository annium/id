using Annium.Blazor.Css;

namespace Annium.Id.Site.Public.Pages.Login
{
    public class Styles : IRuleSet
    {
        public CssRule Title { get; } = Rule.Class()
            .MarginTopRem(1);
    }
}