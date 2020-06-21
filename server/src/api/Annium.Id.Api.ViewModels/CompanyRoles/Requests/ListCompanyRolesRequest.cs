using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Queries.CompanyRoles;

namespace Annium.Id.Api.ViewModels.CompanyRoles.Requests
{
    public class ListCompanyRolesRequest : IRequest<ListCompanyRolesQuery>
    {
        public Guid AppId { get; set; }
    }
}