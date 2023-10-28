using Annium.Architecture.ViewModel;
using Server.Domain.Commands.Me;

namespace Server.ViewModels.Requests.Me;

public record UnregisterMeRequest : IRequest<UnregisterMeCommand> { }
