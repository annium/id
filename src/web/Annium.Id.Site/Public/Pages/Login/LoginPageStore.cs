using Annium.Components.State;
using Annium.Extensions.Validation;
using Annium.Id.Site.Shared.Stores;

namespace Annium.Id.Site.Public.Pages.Login
{
    public class LoginPageStore : IStore
    {
        public IObjectContainer<LoginData> State { get; }

        public LoginPageStore(
            IStateFactory stateFactory,
            IValidator<LoginData> validator
        )
        {
            State = stateFactory.Create(new LoginData());
            State.UseValidator(validator);
        }
    }
}