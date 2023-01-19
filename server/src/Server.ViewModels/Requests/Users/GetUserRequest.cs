using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Queries.Users;

namespace Server.ViewModels.Requests.Users;

public record GetUserRequest : IRequest<GetUserQuery>
{
    public Guid UserId { get; set; }
}