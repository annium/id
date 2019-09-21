using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Queries.Companies;

namespace Annium.Id.ViewModels.Companies.Requests
{
    public class GetCompanyRequest : IRequest<GetCompanyQuery>
    {
        public Guid CompanyId { get; set; }
    }
}