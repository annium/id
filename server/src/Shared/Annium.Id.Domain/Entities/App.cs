using System;

namespace Annium.Id.Domain.Entities
{
    public class App
    {
        public Guid Id { get; }
        public Guid OwnerId { get; set; }
        public string Name { get; set; }
        public Guid ApiToken { get; set; }

        public App(
            Guid ownerId,
            string name,
            Guid apiToken
        )
        {
            OwnerId = ownerId;
            Name = name;
            ApiToken = apiToken;
        }

        internal App(
            Guid id,
            Guid ownerId,
            string name,
            Guid apiToken
        ) : this(ownerId, name, apiToken)
        {
            Id = id;
        }
    }
}