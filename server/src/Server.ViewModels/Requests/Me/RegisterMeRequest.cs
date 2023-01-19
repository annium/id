using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.Me;

namespace Server.ViewModels.Requests.Me;

public record RegisterMeRequest : IRequest<RegisterMeCommand>
{
    public string Server { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public Guid? ReferralId { get; set; }
}