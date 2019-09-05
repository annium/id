using Annium.Architecture.CQRS.Queries;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Queries.Users
{
    public class GetUserProfileQuery : IQuery
    {
        public User User { get; private set; }

        public GetUserProfileQuery()
        {

        }
    }
}