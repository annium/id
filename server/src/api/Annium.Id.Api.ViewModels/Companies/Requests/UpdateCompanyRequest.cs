using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Commands.Companies;

namespace Annium.Id.Api.ViewModels.Companies.Requests
{
    public class UpdateCompanyRequest : UpdateCompanyRequestBase, IRequest<UpdateCompanyCommand>
    {
        public Guid CompanyId { get; set; }
    }

    public class UpdateCompanyRequestBase
    {
        public Guid? ParentId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}