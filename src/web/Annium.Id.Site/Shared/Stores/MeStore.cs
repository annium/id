using System;
using System.Threading.Tasks;
using Annium.Blazor.Storage;
using Annium.Components.State.Forms;
using Annium.Components.State.Operations;
using Annium.Core.Mapper;
using Annium.Data.Operations;
using Annium.Id.Site.Shared.Api.Server.Services;
using Annium.Id.Site.Shared.Models;

namespace Annium.Id.Site.Shared.Stores
{
    internal class MeStore : IMeStore
    {
        public IState<bool> HasLoadedOnce { get; }
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
            HasLoadedOnce = stateFactory.Create(false);
            _tokenStore = tokenStore;
            _loginService = loginService;
            _meService = meService;
            _mapper = mapper;
        }

        public Task Load()
        {
            Console.WriteLine("MeStore.Load");

            return Load(false);
        }

        public Task Reload()
        {
            Console.WriteLine("MeStore.Reload");

            return Load(true);
        }

        public async Task LogOut()
        {
            Console.WriteLine("MeStore.LogOut: start");
            State.Start();

            Console.WriteLine("MeStore.LogOut: api start");
            await _loginService.LogOut();
            Console.WriteLine("MeStore.LogOut: api end");

            State.Reset();
            Console.WriteLine("MeStore.LogOut: reset");
        }


        private async Task Load(bool force)
        {
            // fail immediately if no tokens
            if (await _tokenStore.GetAsync() is null)
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
            var result = await _meService.GetMe();
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
                await _loginService.LogOut();
                Console.WriteLine("MeStore.Load: logout api end");
                State.Fail(result);
                Console.WriteLine("MeStore.Load: failure");
            }

            HasLoadedOnce.Set(true);
            Console.WriteLine("MeStore.Load: end");
        }
    }

    public interface IMeStore : IStore
    {
        IState<bool> HasLoadedOnce { get; }
        bool HasAccess { get; }
        IOperationState<Me> State { get; }
        Task Load();
        Task Reload();
        Task LogOut();
    }
}