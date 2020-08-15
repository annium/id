using AntDesign;

namespace Annium.Id.Site.Public.Layouts
{
    public partial class MinimalCentricLayout
    {
        private EmbeddedProperty _xs = new EmbeddedProperty { Span = "20", Offset = "2" };
        private EmbeddedProperty _sm = new EmbeddedProperty { Span = "14", Offset = "5" };
        private EmbeddedProperty _md = new EmbeddedProperty{ Span = "12", Offset = "6" };
        private EmbeddedProperty _lg = new EmbeddedProperty{ Span = "10", Offset = "7" };
        private EmbeddedProperty _xl = new EmbeddedProperty{ Span = "8", Offset = "8" };
        private EmbeddedProperty _xxl = new EmbeddedProperty{ Span = "6", Offset = "9" };

        private string? Page { get; set; }
        private string? ContainerClass { get; set; }
        private string? LogoClass { get; set; }
        private string? CredentialsClass { get; set; }
        private string? LinkClass { get; set; }
    }
}