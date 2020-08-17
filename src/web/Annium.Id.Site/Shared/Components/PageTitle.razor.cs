using System.Collections.Generic;
using Annium.Blazor.Core.Tools;
using Annium.Blazor.Css;
using Microsoft.AspNetCore.Components;

namespace Annium.Id.Site.Shared.Components
{
    public partial class PageTitle
    {
        [Parameter]
        public string? Class { get; set; }

        [Parameter]
        public int Level { get; set; }

        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, object> Attributes { get; set; } = default!;

        public string ClassName => ClassBuilder.With(Style.Title).With(Class).Build();

        public class Styles : IRuleSet
        {
            public CssRule Title { get; }

            public Styles(Theme theme)
            {
                Title = Rule.TagClass("h2")
                    .FontFamily(theme.FontFamily)
                    .FontSizeRem(1.5)
                    .FontWeightNormal();
            }
        }
    }
}