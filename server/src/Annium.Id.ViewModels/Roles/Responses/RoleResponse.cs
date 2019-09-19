using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Domain.Entities;
using Annium.Id.ViewModels.Claims.Responses;

namespace Annium.Id.ViewModels.Roles.Responses
{
    public class RoleResponse : IResponse<Role>
    {
        public Guid Id { get; set; }
        public Guid AppId { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public ClaimValueResponse[] Claims { get; set; }
    }
}