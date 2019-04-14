using System;
using Annium.Id.Db;
using Newtonsoft.Json;

namespace Annium.Id.Api.Views
{
    public class AppView
    {
        public Guid Id { get; }

        public Guid OwnerId { get; }

        public string Key { get; }

        public string Name { get; }

        public AppView(App app)
        {
            Id = app.Id;
            OwnerId = app.OwnerId;
            Key = app.Key;
            Name = app.Name;
        }

        [JsonConstructor]
        public AppView(
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