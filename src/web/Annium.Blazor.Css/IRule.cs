using System.Collections.Generic;

namespace Annium.Blazor.Css
{
    public interface IRule
    {
        IRule Set(string property, string value);
        IReadOnlyCollection<string> ToCss();
    }
}