using System;

namespace Annium.Id.Api.Views
{
    public class CompanyPublicView
    {
        public Guid Id { get; }
        public string Key { get; }
        public string Name { get; }

        public CompanyPublicView(
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