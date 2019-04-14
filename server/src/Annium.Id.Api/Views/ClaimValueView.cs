using System;
using Newtonsoft.Json;

namespace Annium.Id.Db
{
    public class ClaimValueView
    {
        public Guid Id { get; }

        public string Key { get; }

        public string Name { get; }

        public string Value { get; }

        public ClaimValueView(ClaimValue claim)
        {
            Id = claim.Id;
            Key = claim.Key;
            Name = claim.Name;
            Value = claim.Value;
        }

        [JsonConstructor]
        public ClaimValueView(
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