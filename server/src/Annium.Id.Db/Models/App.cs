using System;
using Newtonsoft.Json;

namespace Annium.Id.Db
{
    public class App
    {
        public Guid Id { get; }

        public Guid OwnerId { get; }

        public string Key { get; set; }

        public string Name { get; set; }

        public Guid ApiToken { get; set; }

        public App(
            Guid ownerId,
            string key,
            string name,
            Guid apiToken
        )
        {
            OwnerId = ownerId;
            Key = key;
            Name = name;
            ApiToken = apiToken;
        }

        [JsonConstructor]
        internal App(
            Guid id,
            Guid ownerId,
            string key,
            string name,
            Guid apiToken
        ) : this(ownerId, key, name, apiToken)
        {
            Id = id;
        }
    }
}