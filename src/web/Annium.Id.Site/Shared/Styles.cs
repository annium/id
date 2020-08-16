using Annium.Blazor.Css;

namespace Annium.Id.Site.Shared
{
    public class Styles : IRuleSet
    {
        private CssRule _html = Rule.Tag("html")
            .Set("display", "flex")
            .Set("width", "100%")
            .Set("min-height", "100vh");
    }
}