using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Queries.Users;

namespace Server.ViewModels.Requests.Users;

public class GetUserRequest : IRequest<GetUserQuery>
{
    public Guid UserId { get; set; }
}