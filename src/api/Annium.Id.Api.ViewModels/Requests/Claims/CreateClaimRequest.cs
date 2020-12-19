using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Domain.Commands.Claims;

namespace Annium.Id.Api.ViewModels.Requests.Claims
{
    public class CreateClaimRequest : IRequest<CreateClaimCommand>
    {
        public Guid AppId { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}