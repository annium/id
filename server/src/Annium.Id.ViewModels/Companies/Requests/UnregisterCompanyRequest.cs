using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Companies;

namespace Annium.Id.ViewModels.Companies.Requests
{
    public class UnregisterCompanyRequest : IRequest<UnregisterCompanyCommand>
    {
        public Guid CompanyId { get; set; }
    }
}