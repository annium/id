using System;

namespace Annium.Id.Api.Views
{
    public class CompanyClaimView
    {
        public Guid Id { get; }
        public Guid AppId { get; }
        public string Key { get; }
        public string Name { get; }

        public CompanyClaimView(
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