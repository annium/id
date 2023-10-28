using Annium.Architecture.ViewModel;
using Server.Domain.Queries.Apps;

namespace Server.ViewModels.Requests.Apps;

public record ListMyAppsRequest : IRequest<ListMyAppsQuery> { }
