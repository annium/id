using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Queries.Companies;

namespace Annium.Id.Api.ViewModels.Requests.Companies
{
    public class GetCompanyRequest : IRequest<GetCompanyQuery>
    {
        public Guid CompanyId { get; set; }
    }
}