using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Domain.Entities;

namespace Annium.Id.ViewModels.Apps.Responses
{
    public class AppPrivateResponse : IResponse<App>
    {
        public Guid Id { get; }
        public Guid OwnerId { get; }
        public string Key { get; }
        public string Name { get; }

        public AppPrivateResponse(
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