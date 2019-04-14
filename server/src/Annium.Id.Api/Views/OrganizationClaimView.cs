using System;
using Annium.Id.Db;
using Newtonsoft.Json;

namespace Annium.Id.Api.Views
{
    public class OrganizationClaimView
    {
        public Guid Id { get; }

        public Guid AppId { get; }

        public string Key { get; }

        public string Name { get; }

        public OrganizationClaimView(OrganizationClaim claim)
        {
            Id = claim.Id;
            AppId = claim.AppId;
            Key = claim.Key;
            Name = claim.Name;
        }

        [JsonConstructor]
        public OrganizationClaimView(
            Guid id,
            Guid appId,
            string key,
            string name
        )
        {
            Id = id;
            AppId = appId;
            Key = key;
            Name = name;
        }
    }
}