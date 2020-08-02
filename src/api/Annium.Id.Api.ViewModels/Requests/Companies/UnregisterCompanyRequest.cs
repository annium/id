using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Commands.Companies;

namespace Annium.Id.Api.ViewModels.Requests.Companies
{
    public class UnregisterCompanyRequest : IRequest<UnregisterCompanyCommand>
    {
        public Guid CompanyId { get; set; }
    }
}