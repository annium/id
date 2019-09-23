using System;
using System.Linq;
using Annium.Core.DependencyInjection;
using Annium.Core.Mediator;
using Annium.Extensions.DependencyInjection;
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
            Add<ViewModels.ServicePack>();
        }

        public override void Register(IServiceCollection services, IServiceProvider provider)
        {
            services.AddSingleton<Func<Instant>>(() => SystemClock.Instance.GetCurrentInstant());

            // auth
            services.AddIdAuthorization(opts => opts.Audience = Constants.IdApp);

            // tools
            services.AddSingleton<IIdentityDataAccessor, IdentityDataAccessor>();

            var ignored = new [] { "ChainBuilder", "PipeHandler" };
            services.AddLogging(route => route
                // .UseConsole());
                .For(m => !ignored.Any(m.Source.Name.Contains)).UseConsole());
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