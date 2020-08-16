using Annium.Testing;
using Xunit;

namespace Annium.Blazor.Css.Tests
{
    public class BasicRuleTest
    {
        [Fact]
        public void RuleWithGeneratedName_Ok()
        {
            // arrange
            var rule = Rule.Class().WidthPx(10);

            // act
            var name = rule.ToString();
            var css = rule.ToCss();

            // assert
            name.IsNotDefault();
            css.IsEqual(new[] { "width: 10px" });
        }
    }
}