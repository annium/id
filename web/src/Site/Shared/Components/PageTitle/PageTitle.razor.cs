using System.Collections.Generic;
using Annium.Blazor.Core.Tools;
using Microsoft.AspNetCore.Components;

namespace Site.Shared.Components.PageTitle;

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
}