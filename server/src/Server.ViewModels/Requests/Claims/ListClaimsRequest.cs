using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Queries.Claims;

namespace Server.ViewModels.Requests.Claims;

public record ListClaimsRequest : IRequest<ListClaimsQuery>
{
    public Guid AppId { get; set; }
}
