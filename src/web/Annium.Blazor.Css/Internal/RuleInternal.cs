using System.Collections.Generic;
using System.Linq;

namespace Annium.Blazor.Css.Internal
{
    internal class RuleInternal : IRule
    {
        private readonly string _selector;
        private readonly IDictionary<string, string> _properties = new Dictionary<string, string>();

#if DEBUG
        private static string PropertyToCss(KeyValuePair<string, string> pair) => $"{pair.Key}: {pair.Value}";
#else
        private static string PropertyToCss(KeyValuePair<string, string> pair) => $"{pair.Key}:{pair.Value}";
#endif

        public RuleInternal(string tag, RuleType type, string name)
        {
            _selector = $"{tag}{type}{name}";
        }

        public RuleInternal(string selector)
        {
            _selector = selector;
        }

        public IRule Set(string property, string value)
        {
            _properties[property] = value;

            return this;
        }

        public override string ToString() => _selector;

        public IReadOnlyCollection<string> ToCss() => _properties.Select(PropertyToCss).ToArray();
    }
}