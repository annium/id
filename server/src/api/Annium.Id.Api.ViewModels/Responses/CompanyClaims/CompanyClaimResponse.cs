using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.ViewModels.Responses.CompanyClaims
{
    public class CompanyClaimResponse : IResponse<CompanyClaim>
    {
        public Guid Id { get; set; }
        public Guid AppId { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}