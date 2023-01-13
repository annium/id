using System;
using System.Threading.Tasks;
using Annium.Components.State.Forms;
using Annium.Components.State.Forms.Extensions;
using Annium.Components.State.Operations;
using Annium.Extensions.Validation;
using Site.Shared.Api.Server.Services;
using Site.Shared.Stores;

namespace Site.Public.Pages.Login;

internal class Store : IStore
{
    public IObjectContainer<LoginData> Form { get; }
    public bool CanLogin => !Form.HasStatus(Status.Error, Status.Loading, Status.Validating) && Form.HasBeenTouched;
    public IOperationState State { get; } = OperationState.New();
    private readonly ILoginService _loginService;
    private readonly IMeStore _meStore;

    public Store(
        IStateFactory stateFactory,
        IValidator<LoginData> validator,
        ILoginService loginService,
        IMeStore meStore
    )
    {
        _loginService = loginService;
        _meStore = meStore;
        Form = stateFactory.Create(new LoginData());
        Form.UseValidator(validator);
    }

    public async Task LogIn()
    {
        State.Start();

        var result = await _loginService.LogIn(Form.At(x => x.Login).Value, Form.At(x => x.Password).Value);

        if (result.HasErrors)
        {
            State.Fail(result);
            _meStore.State.Reset();

            // TODO: set validation from IResult
            // Form..setStatus(resultToStatus(result))
            // TODO: show in notifications store
            if (result.PlainErrors.Count > 0)
                Console.WriteLine(string.Join(", ", result.PlainErrors));
            else if (result.LabeledErrors.ContainsKey("user"))
                Console.WriteLine(string.Join(", ", result.LabeledErrors["user"]));

            return;
        }

        await _meStore.Load();
        if (_meStore.HasAccess)
        {
            State.Succeed();
            Form.Reset();
            // TODO: go to home
        }
        else
            State.Fail(_meStore.State);
    }
}

public interface IStore : Annium.Blazor.Storage.IStore
{
    IObjectContainer<LoginData> Form { get; }
    bool CanLogin { get; }
    Task LogIn();
}