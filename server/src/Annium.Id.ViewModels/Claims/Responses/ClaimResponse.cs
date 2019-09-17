using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Domain.Entities;

namespace Annium.Id.ViewModels.Claims.Responses
{
    public class ClaimResponse : IResponse<Claim>
    {
        public Guid Id { get; }
        public Guid AppId { get; }
        public string Key { get; }
        public string Name { get; }

        public ClaimResponse(
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