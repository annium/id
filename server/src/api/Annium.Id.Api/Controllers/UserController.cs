using System;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Core.Mediator;
using Annium.Data.Operations;
using Annium.Id.AspNetCore;
using Annium.Id.Api.ViewModels.Users.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Annium.Id.Api.Controllers
{
    [Route("users/{userId:guid}")]
    public class UserController : ServerController
    {
        public UserController(
            IMediator mediator
        ) : base(mediator)
        {

        }

        [HttpPost("roles/{roleId:guid}")]
        [Authorize]
        public Task<IResult> AddRoleToUser(Guid userId, Guid roleId)
        {
            var request = new AddRoleToUserRequest { UserId = userId, RoleId = roleId };

            return HandleAsync(request);
        }

        [HttpDelete("roles/{roleId:guid}")]
        [Authorize]
        public Task<IResult> DeleteRoleFromUser(Guid userId, Guid roleId)
        {
            var request = new DeleteRoleFromUserRequest { UserId = userId, RoleId = roleId };

            return HandleAsync(request);
        }

        [HttpPost("claims/{claimId:guid}")]
        [Authorize]
        public Task<IResult> AddClaimToUser(Guid userId, Guid claimId, [FromBody] AddClaimToUserRequestBody requestBody)
        {
            var request = new AddClaimToUserRequest
            {
                UserId = userId,
                ClaimId = claimId,
                Value = requestBody.Value,
            };

            return HandleAsync(request);
        }

        [HttpDelete("claims/{claimId:guid}")]
        [Authorize]
        public Task<IResult> DeleteClaimFromUser(Guid userId, Guid claimId)
        {
            var request = new DeleteClaimFromUserRequest { UserId = userId, ClaimId = claimId };

            return HandleAsync(request);
        }
    }
}