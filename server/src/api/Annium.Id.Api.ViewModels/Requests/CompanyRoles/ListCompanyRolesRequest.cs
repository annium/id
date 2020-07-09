using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Queries.CompanyRoles;

namespace Annium.Id.Api.ViewModels.Requests.CompanyRoles
{
    public class ListCompanyRolesRequest : IRequest<ListCompanyRolesQuery>
    {
        public Guid AppId { get; set; }
    }
}