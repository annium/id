using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Domain.Entities;
using Annium.Id.Api.ViewModels.CompanyClaims.Responses;

namespace Annium.Id.Api.ViewModels.CompanyRoles.Responses
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