using System;
using System.IO;
using Annium.Id.Core;
using Annium.Id.Core.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.Core.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdAuthorizationCoreServices(
            this IServiceCollection services,
            Action<AuthOptions> configure
        )
        {
            var options = new AuthOptions();
            configure(options);
            Validate(options);
            services.AddSingleton(options);

            services.AddSingleton<IPolicyMapper, PolicyMapper>();
            services.AddSingleton<ITokenReader, TokenReader>();
            services.AddSingleton<ITokenWriter, TokenWriter>();

            return services;
        }

        private static void Validate(AuthOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.Audience))
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