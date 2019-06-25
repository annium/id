using System;

namespace Annium.Id.Api.Views
{
    public class CompanyRoleView
    {
        public Guid Id { get; }
        public Guid AppId { get; }
        public string Key { get; }
        public string Name { get; }
        public ClaimValueView[] Claims { get; }

        public CompanyRoleView(
            Guid id,
            Guid appId,
            string key,
            string name,
            ClaimValueView[] claims
        )
        {
            Id = id;
            AppId = appId;
            Key = key;
            Name = name;
            Claims = claims;
        }
    }
}