using System;
using System.IO;
using Annium.Id.Core;
using Annium.Id.Core.Internal;

// ReSharper disable once CheckNamespace
namespace Annium.Core.DependencyInjection;

public static class ServiceContainerExtensions
{
    public static IServiceContainer AddIdAuthorizationCoreServices(
        this IServiceContainer container,
        Action<IServiceProvider, AuthOptions> configure
    )
    {
        container
            .Add(sp =>
            {
                var options = new AuthOptions();

                configure(sp, options);
                Validate(options);

                return options;
            })
            .AsSelf()
            .Singleton();

        return container.AddIdAuthorizationCoreServicesBase();
    }

    private static IServiceContainer AddIdAuthorizationCoreServicesBase(this IServiceContainer container)
    {
        container.Add<IPolicyMapper, PolicyMapper>().Singleton();
        container.Add<ITokenReader, TokenReader>().Singleton();
        container.Add<ITokenWriter, TokenWriter>().Singleton();

        return container;
    }

    private static void Validate(AuthOptions options)
    {
        if (options.Audience == Guid.Empty)
            throw new Exception($"{nameof(AuthOptions.Audience)} is mandatory");

        if (string.IsNullOrWhiteSpace(options.PublicKeyFile))
            throw new Exception($"{nameof(AuthOptions.PublicKeyFile)} is mandatory");

        if (!File.Exists(options.PublicKeyFile))
            throw new Exception(
                $"{nameof(AuthOptions.PublicKeyFile)} {Path.GetFullPath(options.PublicKeyFile)} missing in file system"
            );

        if (!string.IsNullOrEmpty(options.PrivateKeyFile) && !File.Exists(options.PrivateKeyFile))
            throw new Exception(
                $"{nameof(AuthOptions.PrivateKeyFile)} {Path.GetFullPath(options.PrivateKeyFile)} missing in file system"
            );
    }
}
