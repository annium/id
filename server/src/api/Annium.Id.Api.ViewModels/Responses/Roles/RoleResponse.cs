using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.ViewModels.Responses.Claims;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.ViewModels.Responses.Roles
{
    public class RoleResponse : IResponse<Role>
    {
        public Guid Id { get; set; }
        public Guid AppId { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public ClaimValueResponse[] Claims { get; set; } = Array.Empty<ClaimValueResponse>();
    }
}