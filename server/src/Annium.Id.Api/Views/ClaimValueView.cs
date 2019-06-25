using System;

namespace Annium.Id.Api.Views
{
    public class ClaimValueView
    {
        public Guid Id { get; }
        public string Key { get; }
        public string Name { get; }
        public string Value { get; }

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