using System;
using Annium.Core.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Annium.Id.Infrastructure.DbMigrator
{
    internal static class Program
    {
        private static void Main()
        {
            Console.WriteLine("Migrator is run only from ef tools");
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder()
                .UseServiceProviderFactory(new ServiceProviderFactory(b => b.UseServicePack<ServicePack>()));
        }
    }
}