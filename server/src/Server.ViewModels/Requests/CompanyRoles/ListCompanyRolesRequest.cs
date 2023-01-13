using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Queries.CompanyRoles;

namespace Server.ViewModels.Requests.CompanyRoles;

public class ListCompanyRolesRequest : IRequest<ListCompanyRolesQuery>
{
    public Guid AppId { get; set; }
}