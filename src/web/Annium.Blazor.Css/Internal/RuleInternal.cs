using System.Collections.Generic;
using System.Linq;

namespace Annium.Blazor.Css.Internal
{
    internal class RuleInternal : IRule
    {
        private readonly string _tag;
        private readonly RuleType _type;
        private readonly string _name;
        private readonly IDictionary<string, string> _properties = new Dictionary<string, string>();

#if DEBUG
        private static string PropertyToCss(KeyValuePair<string, string> pair) => $"{pair.Key}: {pair.Value}";
#else
        private static string PropertyToCss(KeyValuePair<string, string> pair) => $"{pair.Key}:{pair.Value}";
#endif

        public RuleInternal(string tag, RuleType type, string name)
        {
            _tag = tag;
            _type = type;
            _name = name;
        }

        public IRule Set(string property, string value)
        {
            _properties[property] = value;

            return this;
        }

        public override string ToString() => $"{_tag}{_type}{_name}";

        public IReadOnlyCollection<string> ToCss() => _properties.Select(PropertyToCss).ToArray();
    }
}