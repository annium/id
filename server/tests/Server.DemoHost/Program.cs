using Annium.AspNetCore.Extensions;
using Annium.Core.DependencyInjection;
using Annium.Infrastructure.Hosting;
using Annium.Logging.Microsoft;
using Annium.XRest.Sources.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Server.DemoHost;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServicePack<ServicePack>();
builder.Logging.ConfigureLoggingBridge();
builder.WebHost.UseKestrelDefaults();

var app = builder.Build();

app.UseExceptionMiddleware();
app.UseXRest();
app.UseRouting();
app.UseCorsDefaults();
app.MapControllers();

await app.RunAsync();

namespace Server.DemoHost
{
    public partial class Demo { }
}
