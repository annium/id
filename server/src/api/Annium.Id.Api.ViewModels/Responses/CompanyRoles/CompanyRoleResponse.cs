using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.ViewModels.Responses.CompanyClaims;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.ViewModels.Responses.CompanyRoles
{
    public class CompanyRoleResponse : IResponse<CompanyRole>
    {
        public Guid Id { get; set; }
        public Guid AppId { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public CompanyClaimValueResponse[] Claims { get; set; } = Array.Empty<CompanyClaimValueResponse>();
    }
}