using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Queries.Users;

namespace Annium.Id.Api.ViewModels.Requests.Users
{
    public class GetUserRequest : IRequest<GetUserQuery>
    {
        public Guid UserId { get; set; }
    }
}