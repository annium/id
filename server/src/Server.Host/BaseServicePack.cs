using System;
using Annium.Core.DependencyInjection;
using Annium.Core.Mediator;
using Annium.Core.Runtime.Types;
using Annium.Id.AspNetCore;
using Annium.Id.Core;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;
using Server.Application.Tools;
using Server.Host.Tools;

namespace Server.Host;

internal class BaseServicePack : ServicePackBase
{
    public override void Configure(IServiceContainer container)
    {
        container.AddRuntime(GetType().Assembly);
    }

    public override void Register(IServiceContainer container, IServiceProvider provider)
    {
        container.AddTime().WithRealTime().SetDefault();
        container.AddHttpRequestFactory(true);
        container.AddSerializers()
            .WithJson(opts => opts.ConfigureForOperations().ConfigureForNodaTime(), isDefault: true);
        container.AddXRest();
        container.AddLocalization(opts => opts.UseYamlStorage());
        container.AddComposition();
        container.AddValidation();
        container.AddMapper();
        container.AddMediatorConfiguration(ConfigureMediator);
        container.AddMediator();

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

        // host
        container.Collection.AddCors();
        container.Collection.AddControllers()
            .AddDefaultJsonOptions();
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