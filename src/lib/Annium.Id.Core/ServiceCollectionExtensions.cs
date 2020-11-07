using System;
using System.IO;
using Annium.Id.Core.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.Id.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdAuthorizationCoreServices(
            this IServiceCollection services,
            Action<IServiceProvider, AuthOptions> configure
        )
        {
            services.AddSingleton(sp =>
            {
                var options = new AuthOptions();

                configure(sp, options);
                Validate(options);

                return options;
            });

            return services.AddIdAuthorizationCoreServicesBase();
        }

        private static IServiceCollection AddIdAuthorizationCoreServicesBase(
            this IServiceCollection services
        )
        {
            services.AddSingleton<IPolicyMapper, PolicyMapper>();
            services.AddSingleton<ITokenReader, TokenReader>();
            services.AddSingleton<ITokenWriter, TokenWriter>();

            return services;
        }

        private static void Validate(AuthOptions options)
        {
            if (options.Audience == Guid.Empty)
                throw new Exception($"{nameof(AuthOptions.Audience)} is mandatory");

            if (string.IsNullOrWhiteSpace(options.PublicKeyFile))
                throw new Exception($"{nameof(AuthOptions.PublicKeyFile)} is mandatory");

            if (!File.Exists(options.PublicKeyFile))
                throw new Exception($"{nameof(AuthOptions.PublicKeyFile)} {Path.GetFullPath(options.PublicKeyFile)} missing in file system");

            if (!string.IsNullOrEmpty(options.PrivateKeyFile) && !File.Exists(options.PrivateKeyFile))
                throw new Exception($"{nameof(AuthOptions.PrivateKeyFile)} {Path.GetFullPath(options.PrivateKeyFile)} missing in file system");
        }
    }
}