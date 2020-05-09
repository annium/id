using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Queries.Companies;

namespace Annium.Id.Api.ViewModels.Companies.Requests
{
    public class GetCompanyUsersRequest : IRequest<GetCompanyUsersQuery>
    {
        public Guid CompanyId { get; set; }
    }
}