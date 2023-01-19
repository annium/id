using Annium.Architecture.ViewModel;
using Server.Domain.Queries.Users;

namespace Server.ViewModels.Requests.Users;

public record FindUsersRequest : IRequest<FindUsersQuery>
{
    public string Query { get; set; } = string.Empty;
    public int Limit { get; set; }
}