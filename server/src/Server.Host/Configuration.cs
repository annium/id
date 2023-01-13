using Annium.Core.DependencyInjection;

namespace Server.Host;

public sealed record Configuration
{
    public WebHostConfiguration Host { get; set; } = new();
}