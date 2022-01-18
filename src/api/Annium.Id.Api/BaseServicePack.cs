using System;
using Annium.Core.DependencyInjection;
using Annium.Core.Mediator;
using Annium.Core.Runtime.Types;
using Annium.Id.Api.Application.Tools;
using Annium.Id.Api.Tools;
using Annium.Id.Core;
using NodaTime;

namespace Annium.Id.Api
{
    internal class BaseServicePack : ServicePackBase
    {
        public override void Configure(IServiceContainer container)
        {
            container.AddRuntimeTools(GetType().Assembly, false);
        }

        public override void Register(IServiceContainer container, IServiceProvider provider)
        {
            container.AddTime().WithRealTime().SetDefault();

            // auth
            container.AddIdAuthorization((sp, opts) =>
            {
                var cfg = sp.Resolve<Application.Configuration>();
                opts.Audience = Constants.IdAppId;
                opts.PublicKeyFile = cfg.PublicKeyFile;
                opts.PrivateKeyFile = cfg.PrivateKeyFile;
                opts.AccessTokenLifeTime = Duration.FromMinutes(30);
                opts.RefreshTokenLifeTime = Duration.FromDays(1);
            });
            container.AddIdPolicy<Guid>(AuthPolicy.CanRefreshToken, (token, appId) => token.App.Id == appId);
            container.AddIdPolicy<Guid>(AuthPolicy.CanLogOut, (token, appId) => token.App.Id == appId);

            // tools
            container.Add<IIdentityDataAccessor, IdentityDataAccessor>().Singleton();

            container.AddHttpRequestFactory().SetDefault();
            container.AddJsonSerializers()
                .Configure(opts => opts.ConfigureForOperations().ConfigureForNodaTime())
                .SetDefault();
            container.AddXRest();
            container.AddLocalization(opts => opts.UseYamlStorage());
            container.AddComposition();
            container.AddValidation();
            container.AddMapper();
            container.AddMediatorConfiguration(ConfigureMediator);
            container.AddMediator();
        }

        private void ConfigureMediator(MediatorConfiguration cfg, ITypeManager typeManager)
        {
            cfg.AddLoggingHandler();
            cfg.AddHttpStatusPipeHandler();
            cfg.AddExceptionHandler();
            cfg.AddViewMappingHandlers();
            cfg.AddValidationHandler();
            cfg.AddCompositionHandler();

            cfg.AddCommandQueryHandlers(typeManager);
        }
    }
}