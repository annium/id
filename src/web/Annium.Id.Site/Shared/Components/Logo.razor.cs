using Annium.Blazor.Core.Tools;
using Microsoft.AspNetCore.Components;

namespace Annium.Id.Site.Shared.Components
{
    public partial class Logo
    {
        [Parameter]
        public string? Class { get; set; }

        [Parameter]
        public EventCallback OnClick { get; set; }

        public string ClassName => ClassBuilder.With(LogoClass).With(Class).Build();

        public string? LogoClass { get; set; }
    }
}