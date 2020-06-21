using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.ViewModels.Claims.Responses
{
    public class ClaimResponse : IResponse<Claim>
    {
        public Guid Id { get; set; }
        public Guid AppId { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}