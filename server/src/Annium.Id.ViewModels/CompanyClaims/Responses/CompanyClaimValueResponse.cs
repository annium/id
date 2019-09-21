using System;

namespace Annium.Id.ViewModels.CompanyClaims.Responses
{
    public class CompanyClaimValueResponse
    {
        public Guid Id { get; }
        public string Key { get; }
        public string Name { get; }
        public string Value { get; }

        public CompanyClaimValueResponse(
            Guid id,
            string key,
            string name,
            string value
        )
        {
            Id = id;
            Key = key;
            Name = name;
            Value = value;
        }
    }
}