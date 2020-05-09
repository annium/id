using System;

namespace Annium.Id.ViewModels.CompanyClaims.Responses
{
    public class CompanyClaimValueResponse
    {
        public Guid Id { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}