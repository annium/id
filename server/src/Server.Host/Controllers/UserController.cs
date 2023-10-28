using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Core.Mediator;
using Annium.Data.Operations;
using Annium.Id.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Server.ViewModels.Requests.Users;
using Server.ViewModels.Responses.Users;

namespace Server.Host.Controllers;

[Route("users")]
public class UserController : ServerController
{
    public UserController(IMediator mediator, IServiceProvider sp)
        : base(mediator, sp) { }

    [HttpGet]
    public Task<IResult<IEnumerable<UserResponse>>> FindUsers([FromQuery] FindUsersRequest request)
    {
        return HandleAsync<FindUsersRequest, IEnumerable<UserResponse>>(request);
    }

    [HttpGet("{userId:guid}")]
    public Task<IResult<UserResponse>> GetUser(Guid userId)
    {
        var request = new GetUserRequest { UserId = userId };

        return HandleAsync<GetUserRequest, UserResponse>(request);
    }

    [HttpPost("{userId:guid}/roles/{roleId:guid}")]
    [Authorize]
    public Task<IResult> AddRoleToUser(Guid userId, Guid roleId)
    {
        var request = new AddRoleToUserRequest { UserId = userId, RoleId = roleId };

        return HandleAsync(request);
    }

    [HttpDelete("{userId:guid}/roles/{roleId:guid}")]
    [Authorize]
    public Task<IResult> DeleteRoleFromUser(Guid userId, Guid roleId)
    {
        var request = new DeleteRoleFromUserRequest { UserId = userId, RoleId = roleId };

        return HandleAsync(request);
    }

    [HttpPost("{userId:guid}/claims/{claimId:guid}")]
    [Authorize]
    public Task<IResult> AddClaimToUser(Guid userId, Guid claimId, [FromBody] AddClaimToUserRequestBody requestBody)
    {
        var request = new AddClaimToUserRequest
        {
            UserId = userId,
            ClaimId = claimId,
            Value = requestBody.Value
        };

        return HandleAsync(request);
    }

    [HttpDelete("{userId:guid}/claims/{claimId:guid}")]
    [Authorize]
    public Task<IResult> DeleteClaimFromUser(Guid userId, Guid claimId)
    {
        var request = new DeleteClaimFromUserRequest { UserId = userId, ClaimId = claimId };

        return HandleAsync(request);
    }
}
