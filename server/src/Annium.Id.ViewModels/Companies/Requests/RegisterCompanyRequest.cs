using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Companies;

namespace Annium.Id.ViewModels.Companies.Requests
{
    public class RegisterCompanyRequest : IRequest<RegisterCompanyCommand>
    {
        public Guid? ParentId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}