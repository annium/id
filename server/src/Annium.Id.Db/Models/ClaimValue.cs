using System;

namespace Annium.Id.Db
{
    public class ClaimValue
    {
        public Guid Id { get; }
        public string Key { get; }
        public string Name { get; }
        public string Value { get; }

        public ClaimValue(
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