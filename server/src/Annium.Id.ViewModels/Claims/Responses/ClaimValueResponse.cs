using System;

namespace Annium.Id.ViewModels.Claims.Responses
{
    public class ClaimValueResponse
    {
        public Guid Id { get; }
        public string Key { get; }
        public string Name { get; }
        public string Value { get; }

        public ClaimValueResponse(
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