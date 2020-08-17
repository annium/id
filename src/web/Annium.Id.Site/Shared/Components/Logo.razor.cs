using Annium.Blazor.Core.Tools;
using Annium.Blazor.Css;
using Microsoft.AspNetCore.Components;

namespace Annium.Id.Site.Shared.Components
{
    public partial class Logo
    {
        [Parameter]
        public string? Class { get; set; }

        [Parameter]
        public EventCallback OnClick { get; set; }

        public string ClassName => ClassBuilder.With(Style.Logo).With(Class).Build();

        public class Styles : IRuleSet
        {
            public readonly CssRule Logo = Rule.Class()
                .WidthEm(1)
                .HeightEm(1);
        }
    }
}