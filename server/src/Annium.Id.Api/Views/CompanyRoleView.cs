using System;
using System.Linq;
using Annium.Id.Db;
using Newtonsoft.Json;

namespace Annium.Id.Api.Views
{
    public class CompanyRoleView
    {
        public Guid Id { get; }

        public Guid AppId { get; }

        public string Key { get; }

        public string Name { get; }

        public ClaimValueView[] Claims { get; }

        public CompanyRoleView(CompanyRole role)
        {
            Id = role.Id;
            AppId = role.AppId;
            Key = role.Key;
            Name = role.Name;
            Claims = role.Claims.Select(c => new ClaimValueView(c)).ToArray();
        }

        [JsonConstructor]
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