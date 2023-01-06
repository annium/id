using Annium.Core.DependencyInjection;

namespace Annium.Id.Api;

public sealed record Configuration
{
    public WebHostConfiguration Host { get; set; } = new();
}