using System;

namespace Annium.Id.Api.Views
{
    public class CompanyPrivateView
    {
        public Guid Id { get; }
        public Guid OwnerId { get; }
        public Guid? ParentId { get; }
        public string Key { get; }
        public string Name { get; }

        public CompanyPrivateView(
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