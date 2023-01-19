using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Queries.CompanyRoles;

namespace Server.ViewModels.Requests.CompanyRoles;

public record ListCompanyRolesRequest : IRequest<ListCompanyRolesQuery>
{
    public Guid AppId { get; set; }
}