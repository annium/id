using Annium.Testing;
using Xunit;

namespace Annium.Components.Forms.Tests
{
    public class SampleTest
    {
        [Fact]
        public void True_IsTrue()
        {
            // arrange
            var value = true;

            // assert
            value.IsTrue();
        }
    }
}