using System;
using System.Threading.Tasks;
using Annium.Blazor.State;
using Annium.Components.State.Forms;
using Annium.Components.State.Operations;
using Annium.Core.Mapper;
using Annium.Data.Operations;
using Site.Shared.Api.Server.Services;
using Site.Shared.Models;

namespace Site.Shared.Stores;

internal class MeStore : StateBase, IMeStore
{
    public IValueTrackedState<bool> HasLoadedOnce { get; }
    public bool HasAccess => State.HasSucceed;
    public IOperationState<Me> State { get; } = OperationState.New<Me>();
    private readonly ITokenStore _tokenStore;
    private readonly ILoginService _loginService;
    private readonly IMeService _meService;
    private readonly IMapper _mapper;

    public MeStore(
        IStateFactory stateFactory,
        ITokenStore tokenStore,
        ILoginService loginService,
        IMeService meService,
        IMapper mapper
    )
    {
        HasLoadedOnce = stateFactory.CreateAtomic(false);
        _tokenStore = tokenStore;
        _loginService = loginService;
        _meService = meService;
        _mapper = mapper;
    }

    public Task LoadAsync()
    {
        Console.WriteLine("MeStore.Load");

        return LoadAsync(false);
    }

    public Task ReloadAsync()
    {
        Console.WriteLine("MeStore.Reload");

        return LoadAsync(true);
    }

    public async Task LogOutAsync()
    {
        Console.WriteLine("MeStore.LogOut: start");
        State.Start();

        Console.WriteLine("MeStore.LogOut: api start");
        await _loginService.LogOutAsync();
        Console.WriteLine("MeStore.LogOut: api end");

        State.Reset();
        Console.WriteLine("MeStore.LogOut: reset");
    }

    private async Task LoadAsync(bool force)
    {
        // fail immediately if no tokens
        if (_tokenStore.Get() is null)
        {
            Console.WriteLine("MeStore.Load: no tokens");
            State.Fail(Result.New().Error("Tokens missing"));
            HasLoadedOnce.Set(true);

            return;
        }

        // cache call, if allowed
        if (!force && HasAccess)
        {
            Console.WriteLine("MeStore.Load: not force, had access - cached success");
            HasLoadedOnce.Set(true);

            return;
        }

        Console.WriteLine("MeStore.Load: start");
        State.Start();

        Console.WriteLine("MeStore.Load: api start");
        var result = await _meService.GetMeAsync();
        Console.WriteLine("MeStore.Load: api end");

        if (result.IsOk)
        {
            State.Succeed(_mapper.Map<Me>(result.Data));
            HasLoadedOnce.Set(true);
            Console.WriteLine("MeStore.Load: success");
        }
        else
        {
            Console.WriteLine("MeStore.Load: logout api start");
            await _loginService.LogOutAsync();
            Console.WriteLine("MeStore.Load: logout api end");
            State.Fail(result);
            Console.WriteLine("MeStore.Load: failure");
        }

        HasLoadedOnce.Set(true);
        Console.WriteLine("MeStore.Load: end");
    }
}

public interface IMeStore
{
    IValueTrackedState<bool> HasLoadedOnce { get; }
    bool HasAccess { get; }
    IOperationState<Me> State { get; }
    Task LoadAsync();
    Task ReloadAsync();
    Task LogOutAsync();
}
