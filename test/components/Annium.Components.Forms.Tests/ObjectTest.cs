using Annium.Testing;
using Xunit;

namespace Annium.Components.Forms.Tests
{
    public class ObjectTest : TestBase
    {
        [Fact]
        public void Init_Ok()
        {
            // arrange
            var factory = GetFactory();
            var value = Arrange();

            // act
            var state = factory.Create(value);

            // assert
            state.Value.IsEqual(new User
            {
                Name = value.Name
            });
            state.At(x => x.Name).Value.IsEqual(value.Name);
            state.HasChanged.IsFalse();
            state.HasBeenTouched.IsFalse();
        }

        private User Arrange() => new User
        {
            Name = "Max",
        };

        private class User
        {
            public string Name { get; set; } = string.Empty;
        }
    }
}