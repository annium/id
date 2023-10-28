using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Queries.Roles;

namespace Server.ViewModels.Requests.Roles;

public record ListRolesRequest : IRequest<ListRolesQuery>
{
    public Guid AppId { get; set; }
}
