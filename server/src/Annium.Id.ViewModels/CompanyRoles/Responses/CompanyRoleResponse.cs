using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Domain.Entities;
using Annium.Id.ViewModels.CompanyClaims.Responses;

namespace Annium.Id.ViewModels.CompanyRoles.Responses
{
    public class CompanyRoleResponse : IResponse<CompanyRole>
    {
        public Guid Id { get; set; }
        public Guid AppId { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public CompanyClaimValueResponse[] Claims { get; set; }
    }
}