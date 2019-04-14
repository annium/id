using System;
using Annium.Id.Db;
using Newtonsoft.Json;

namespace Annium.Id.Api.Views
{
    public class OrganizationView
    {
        public Guid Id { get; }

        public Guid OwnerId { get; }

        public Guid? ParentId { get; }

        public string Key { get; }

        public string Name { get; }

        public OrganizationView(Organization organization)
        {
            Id = organization.Id;
            OwnerId = organization.OwnerId;
            ParentId = organization.ParentId;
            Key = organization.Key;
            Name = organization.Name;
        }

        [JsonConstructor]
        public OrganizationView(
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