using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Commands.Companies;

namespace Annium.Id.Api.ViewModels.Requests.Companies
{
    public class UpdateCompanyRequest : UpdateCompanyRequestBody, IRequest<UpdateCompanyCommand>
    {
        public Guid CompanyId { get; set; }
    }

    public class UpdateCompanyRequestBody
    {
        public Guid? ParentId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}