using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Queries.Companies;

namespace Annium.Id.ViewModels.Companies.Requests
{
    public class GetCompanyUsersRequest : IRequest<GetCompanyUsersQuery>
    {
        public Guid CompanyId { get; set; }
    }
}