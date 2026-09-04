using System;
using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.CQRS;
using Annium.Architecture.Http;
using Annium.Architecture.Mediator;
using Annium.Architecture.ViewModel;
using Annium.AspNetCore.Extensions;
using Annium.Core.DependencyInjection;
using Annium.Core.Mapper;
using Annium.Core.Mediator;
using Annium.Core.Runtime;
using Annium.Core.Runtime.Types;
using Annium.Data.Operations.Serialization.Json;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Localization.Abstractions;
using Annium.Localization.Yaml;
using Annium.Net.Http;
using Annium.NodaTime.Serialization.Json;
using Annium.Serialization.Abstractions;
using Annium.Serialization.Json;
using Annium.XRest.Sources.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;
using Server.Application.Tools;
using Server.Host.Tools;

namespace Server.Host;

internal class BaseServicePack : ServicePackBase
{
    public override Task ConfigureAsync(IServiceContainer container, CancellationToken ct)
    {
        container.AddRuntime(GetType().Assembly);

        return Task.CompletedTask;
    }

    public override Task RegisterAsync(IServiceContainer container, IServiceProvider provider, CancellationToken ct)
    {
        container.AddTime().WithRealTime().SetDefault();
        container.AddHttpRequestFactory(true);
        container
            .AddSerializers()
            .WithJson(opts => opts.ConfigureForOperations().ConfigureForNodaTime(), isDefault: true);
        container.AddXRest();
        container.AddLocalization(opts => opts.UseYamlStorage());
        container.AddComposition();
        container.AddValidation();
        container.AddMapper();
        container.AddMediatorConfiguration(ConfigureMediator);
        container.AddMediator();

        // auth
        container.AddIdAuthorization(
            (sp, opts) =>
            {
                var cfg = sp.Resolve<Application.Configuration>();
                opts.Audience = Annium.Id.Core.Constants.IdAppId;
                opts.PublicKeyFile = cfg.PublicKeyFile;
                opts.PrivateKeyFile = cfg.PrivateKeyFile;
                opts.AccessTokenLifeTime = Duration.FromMinutes(30);
                opts.RefreshTokenLifeTime = Duration.FromDays(1);
            }
        );
        container.AddIdPolicy<Guid>(AuthPolicy.CanRefreshToken, (token, appId) => token.App.Id == appId);
        container.AddIdPolicy<Guid>(AuthPolicy.CanLogOut, (token, appId) => token.App.Id == appId);

        // tools
        container.Add<IIdentityDataAccessor, IdentityDataAccessor>().Singleton();

        // host
        container.Collection.AddCors();
        container.Collection.AddControllers().AddDefaultJsonOptions();

        return Task.CompletedTask;
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
