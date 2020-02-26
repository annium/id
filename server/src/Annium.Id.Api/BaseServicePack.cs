using System;
using Annium.Core.DependencyInjection;
using Annium.Core.Mediator;
using Annium.Id.Api.Tools;
using Annium.Id.Application.Tools;
using Annium.Id.Core;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;

namespace Annium.Id.Api
{
    internal class BaseServicePack : ServicePackBase
    {
        public override void Register(IServiceCollection services, IServiceProvider provider)
        {
            services.AddSingleton<Func<Instant>>(() => SystemClock.Instance.GetCurrentInstant());

            // auth
            services.AddIdAuthorization(opts =>
            {
                var cfg = provider.GetRequiredService<Application.Configuration>();
                opts.Audience = Constants.IdAppId;
                opts.PublicKeyFile = cfg.PublicKeyFile;
                opts.PrivateKeyFile = cfg.PrivateKeyFile;
                opts.AccessTokenLifeTime = Duration.FromMinutes(30);
                opts.RefreshTokenLifeTime = Duration.FromDays(1);
            });
            services.AddIdPolicy<Guid>(AuthPolicy.CanRefreshToken, (token, appId) => token.App.Id == appId);
            services.AddIdPolicy<Guid>(AuthPolicy.CanLogOut, (token, appId) => token.App.Id == appId);

            // tools
            services.AddSingleton<IIdentityDataAccessor, IdentityDataAccessor>();

            services.AddLocalization(opts => opts.UseYamlStorage());
            services.AddComposition();
            services.AddValidation();
            services.AddMapper();
            services.AddMediatorConfiguration(ConfigureMediator);
            services.AddMediator();
        }

        private void ConfigureMediator(MediatorConfiguration cfg)
        {
            cfg.AddLoggingHandler();
            cfg.AddHttpStatusPipeHandler();
            cfg.AddModelStatePipeHandler();
            cfg.AddExceptionHandler();
            cfg.AddViewMappingHandlers();
            cfg.AddValidationHandler();
            cfg.AddCompositionHandler();

            cfg.AddCommandQueryHandlers();
        }
    }
}