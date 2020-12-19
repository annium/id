using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Domain.Commands.Companies;

namespace Annium.Id.Api.ViewModels.Requests.Companies
{
    public class RegisterCompanyRequest : IRequest<RegisterCompanyCommand>
    {
        public Guid? ParentId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}