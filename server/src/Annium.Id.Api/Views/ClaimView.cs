using System;

namespace Annium.Id.Api.Views
{
    public class ClaimView
    {
        public Guid Id { get; }
        public Guid AppId { get; }
        public string Key { get; }
        public string Name { get; }

        public ClaimView(
            Guid id,
            Guid appId,
            string key,
            string name
        )
        {
            Id = id;
            AppId = appId;
            Key = key;
            Name = name;
        }
    }
}