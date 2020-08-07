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
            var initialValue = Arrange();

            // act
            var state = factory.Create(initialValue);

            // assert
            state.Value.IsEqual(new User
            {
                Name = initialValue.Name
            });
            state.At(x => x.Name).Value.IsEqual(initialValue.Name);
            state.HasChanged.IsFalse();
            state.HasBeenTouched.IsFalse();
        }

        [Fact]
        public void Set_Ok()
        {
            // arrange
            var factory = GetFactory();
            var initialValue = Arrange();
            var otherValue = new User
            {
                Name = "Lex",
            };
            var state = factory.Create(initialValue);

            // act
            state.Set(otherValue);

            // assert
            state.Value.IsEqual(otherValue);
            state.At(x => x.Name).Value.IsEqual(otherValue.Name);
            state.HasChanged.IsTrue();
            state.HasBeenTouched.IsTrue();

            // act
            state.Set(initialValue);

            // assert
            state.Value.IsEqual(initialValue);
            state.At(x => x.Name).Value.IsEqual(initialValue.Name);
            state.HasChanged.IsFalse();
            state.HasBeenTouched.IsTrue();
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