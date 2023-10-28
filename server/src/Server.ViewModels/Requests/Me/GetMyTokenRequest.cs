using Annium.Architecture.ViewModel;
using Server.Domain.Queries.Me;

namespace Server.ViewModels.Requests.Me;

public record GetMyTokenRequest : IRequest<GetMyTokenQuery> { }
