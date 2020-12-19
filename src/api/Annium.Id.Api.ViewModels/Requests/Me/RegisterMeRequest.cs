using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Domain.Commands.Me;

namespace Annium.Id.Api.ViewModels.Requests.Me
{
    public class RegisterMeRequest : IRequest<RegisterMeCommand>
    {
        public string Server { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public Guid? ReferralId { get; set; }
    }
}