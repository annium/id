using System;

namespace Annium.Id.Api.Views
{
    public class AppPrivateView
    {
        public Guid Id { get; }
        public Guid OwnerId { get; }
        public string Key { get; }
        public string Name { get; }

        public AppPrivateView(
            Guid id,
            Guid ownerId,
            string key,
            string name
        )
        {
            Id = id;
            OwnerId = ownerId;
            Key = key;
            Name = name;
        }
    }
}