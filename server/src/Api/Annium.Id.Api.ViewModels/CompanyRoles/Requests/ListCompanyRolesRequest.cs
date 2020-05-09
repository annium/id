using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Queries.CompanyRoles;

namespace Annium.Id.ViewModels.CompanyRoles.Requests
{
    public class ListCompanyRolesRequest : IRequest<ListCompanyRolesQuery>
    {
        public Guid AppId { get; set; }
    }
}