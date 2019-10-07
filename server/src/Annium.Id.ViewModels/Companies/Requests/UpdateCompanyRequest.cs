using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Companies;

namespace Annium.Id.ViewModels.Companies.Requests
{
    public class UpdateCompanyRequest : UpdateCompanyRequestBase, IRequest<UpdateCompanyCommand>
    {
        public Guid CompanyId { get; set; }
    }

    public class UpdateCompanyRequestBase
    {
        public Guid? ParentId { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}