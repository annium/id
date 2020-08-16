using Annium.Blazor.Css;
using Annium.Blazor.Css.Internal;
using AntDesign;

namespace Annium.Id.Site.Public.Layouts
{
    public partial class MinimalCentricLayout
    {
        private EmbeddedProperty _xs = new EmbeddedProperty { Span = "20", Offset = "2" };
        private EmbeddedProperty _sm = new EmbeddedProperty { Span = "14", Offset = "5" };
        private EmbeddedProperty _md = new EmbeddedProperty { Span = "12", Offset = "6" };
        private EmbeddedProperty _lg = new EmbeddedProperty { Span = "10", Offset = "7" };
        private EmbeddedProperty _xl = new EmbeddedProperty { Span = "8", Offset = "8" };
        private EmbeddedProperty _xxl = new EmbeddedProperty { Span = "6", Offset = "9" };
    }

    public class Styles : IRuleSet
    {
        public readonly CssRule Container;

        public readonly CssRule Logo = Rule.Class()
            .FontSizeRem(5);

        public readonly CssRule Credentials = Rule.Class();
        public readonly CssRule Link = Rule.Class();

        public Styles(Theme theme)
        {
            Container = Rule.Class()
                .FlexColumn(AlignItems.Center, JustifyContent.FlexStart)
                .MarginTopRem(6)
                .PaddingRem(1.5)
                .BorderRadiusRem(1.5)
                .BackgroundColor(theme.Palette.Paper)
                .FontSizeRem(1);
        }
    }
}