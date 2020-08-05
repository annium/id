using Xunit;

namespace Annium.Components.Forms.Tests
{
    public class SetTest : TestBase
    {
        [Fact]
        public void Init_Ok()
        {
            // arrange
            var factory = GetFactory();
            var value = new User
            {
                Login = "login_one",
                Pass = "pass_one",
                Remember = true,
            };
            //
            // // act
            // var state = factory.Create(value);
            //
            // // assert
            // state.Value.IsEqual(value);
            // state.At(x => x.Login).Value.IsEqual(value.Login);
        }

        private class User
        {
            public string Login { get; set; } = string.Empty;
            public string Pass { get; set; } = string.Empty;
            public bool Remember { get; set; }
        }
    }
}