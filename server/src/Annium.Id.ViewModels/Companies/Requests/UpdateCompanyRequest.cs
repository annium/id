using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Companies;

namespace Annium.Id.ViewModels.Companies.Requests
{
    public class UpdateCompanyRequest : IRequest<UpdateCompanyCommand>
    {
        public Guid CompanyId { get; set; }
        public Guid? ParentId { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
    }
}