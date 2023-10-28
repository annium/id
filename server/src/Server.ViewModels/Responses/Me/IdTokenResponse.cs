using System;
using System.Collections.Generic;
using Annium.Architecture.ViewModel;
using Annium.Id.Core;

namespace Server.ViewModels.Responses.Me;

public record IdTokenResponse : IResponse<IdToken>
{
    public Guid UserId { get; set; }
    public Guid LoginId { get; set; }
    public AppTokenResponse App { get; set; } = default!;

    public IReadOnlyCollection<CompanyTokenResponse> Companies { get; set; } = Array.Empty<CompanyTokenResponse>();
}

public record AppTokenResponse
{
    public Guid Id { get; set; }
    public IReadOnlyCollection<string> Roles { get; set; } = Array.Empty<string>();
    public IReadOnlyDictionary<string, string> Claims { get; set; } = new Dictionary<string, string>();
}

public record CompanyTokenResponse
{
    public Guid Id { get; set; }
    public IReadOnlyCollection<string> Roles { get; set; } = Array.Empty<string>();
    public IReadOnlyDictionary<string, string> Claims { get; set; } = new Dictionary<string, string>();
}
