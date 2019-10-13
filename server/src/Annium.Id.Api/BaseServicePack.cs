using System;
using System.IO;
using Annium.Configuration.Abstractions;
using Annium.Core.DependencyInjection;
using Annium.Core.Mediator;
using Annium.Id.Api.Tools;
using Annium.Id.Application.Tools;
using Annium.Id.Core;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;

namespace Annium.Id.Api
{
    public class BaseServicePack : ServicePackBase
    {
        public BaseServicePack()
        {
            Add<Application.ServicePack>();
        }

        public override void Configure(IServiceCollection services)
        {
            var cfg = new ConfigurationBuilder()
                .AddYamlFile(Path.Combine("configuration", "api.yml"))
                .Build<Application.Configuration>();
            services.AddSingleton(cfg);
        }

        public override void Register(IServiceCollection services, IServiceProvider provider)
        {
            services.AddSingleton<Func<Instant>>(() => SystemClock.Instance.GetCurrentInstant());

            // auth
            services.AddIdAuthorization(opts =>
            {
                var cfg = provider.GetRequiredService<Application.Configuration>();
                opts.Audience = Constants.IdApp;
                opts.PublicKeyFile = cfg.PublicKeyFile;
            });

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