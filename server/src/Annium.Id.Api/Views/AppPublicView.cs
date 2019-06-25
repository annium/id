using System;

namespace Annium.Id.Api.Views
{
    public class AppPublicView
    {
        public Guid Id { get; }
        public string Key { get; }
        public string Name { get; }

        public AppPublicView(
            Guid id,
            string key,
            string name
        )
        {
            Id = id;
            Key = key;
            Name = name;
        }
    }
}