using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Domain.Entities;

namespace Annium.Id.ViewModels.Companies.Responses
{
    public class CompanyResponse : IResponse<Company>
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
    }
}