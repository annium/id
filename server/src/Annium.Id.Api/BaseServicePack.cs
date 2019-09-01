using System;
using Annium.Core.DependencyInjection;
using Annium.Core.Mediator;
using Annium.Extensions.DependencyInjection;
using Annium.Id.Api.Tools;
using Annium.Id.Application.CommandHandlers;
using Annium.Logging.Abstractions;
using Microsoft.AspNetCore.Http;
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

        public override void Register(IServiceCollection services, IServiceProvider provider)
        {
            services.AddSingleton<Func<Instant>>(() => SystemClock.Instance.GetCurrentInstant());

            // auth
            services.AddIdAuthorization();

            // helpers
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // tools
            services.AddSingleton<IIdentityDataAccessor, IdentityDataAccessor>();

            services.AddSingleton(new LoggerConfiguration(LogLevel.Trace));
            services.AddConsoleLogger();
            services.AddLocalization(opts => opts.UseYamlStorage());
            services.AddValidation();
            services.AddMapper();
            services.AddMediatorConfiguration(ConfigureMediator);
            services.AddMediator();

        }

        private void ConfigureMediator(MediatorConfiguration cfg)
        {
            cfg.AddLoggingHandler();
            cfg.Add(typeof(HttpActionPipeHandler<,>));
            cfg.AddExceptionHandler();
            cfg.AddViewMappingHandlers();
            cfg.AddValidationHandler();

            cfg.Add(typeof(UserCommandHandler));
        }
    }
}