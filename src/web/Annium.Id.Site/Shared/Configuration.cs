using System;

namespace Annium.Id.Site.Shared;

public class Configuration
{
    public Guid AppId { get; set; }
    public Uri Server { get; set; } = new Uri("http://localhost");
}