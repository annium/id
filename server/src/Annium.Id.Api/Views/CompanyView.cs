using System;
using Annium.Id.Db;
using Newtonsoft.Json;

namespace Annium.Id.Api.Views
{
    public class CompanyView
    {
        public Guid Id { get; }

        public Guid OwnerId { get; }

        public Guid? ParentId { get; }

        public string Key { get; }

        public string Name { get; }

        public CompanyView(Company company)
        {
            Id = company.Id;
            OwnerId = company.OwnerId;
            ParentId = company.ParentId;
            Key = company.Key;
            Name = company.Name;
        }

        [JsonConstructor]
        public CompanyView(
            Guid id,
            Guid ownerId,
            Guid? parentId,
            string key,
            string name
        )
        {
            Id = id;
            OwnerId = ownerId;
            ParentId = parentId;
            Key = key;
            Name = name;
        }
    }
}