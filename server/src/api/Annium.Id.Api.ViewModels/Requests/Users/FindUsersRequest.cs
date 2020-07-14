using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Queries.Users;

namespace Annium.Id.Api.ViewModels.Requests.Users
{
    public class FindUsersRequest : IRequest<FindUsersQuery>
    {
        public string Query { get; set; } = string.Empty;
        public int Limit { get; set; }
    }
}