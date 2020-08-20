using System;
using System.Threading.Tasks;
using Annium.Components.State.Forms;
using Annium.Components.State.Forms.Extensions;
using Annium.Extensions.Validation;
using Annium.Id.Site.Shared.Stores;

namespace Annium.Id.Site.Public.Pages.Login
{
    public class Store : IStore
    {
        public IObjectContainer<LoginData> State { get; }
        public bool CanLogin => State.HasStatus(Status.Error, Status.Loading, Status.Validating) && State.HasBeenTouched;

        public Store(
            IStateFactory stateFactory,
            IValidator<LoginData> validator
        )
        {
            State = stateFactory.Create(new LoginData());
            State.UseValidator(validator);
        }

        public async Task Login()
        {
            Console.WriteLine($"Login: {CanLogin}");
            await Task.CompletedTask;
        }
    }
}