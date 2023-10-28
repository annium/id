using Annium.Architecture.ViewModel;
using Server.Domain.Queries.Apps;

namespace Server.ViewModels.Requests.Apps;

public record FindAppsRequest : IRequest<FindAppsQuery>
{
    public string Query { get; set; } = string.Empty;
}
