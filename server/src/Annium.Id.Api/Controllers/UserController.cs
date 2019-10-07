using System;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Core.Mediator;
using Annium.Id.AspNetCore;
using Annium.Id.ViewModels.Users.Requests;
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
        public Task<IActionResult> AddRoleToUser(Guid userId, Guid roleId)
        {
            var request = new AddRoleToUserRequest { UserId = userId, RoleId = roleId };

            return HandleAsync(request);
        }

        [HttpDelete("roles/{roleId:guid}")]
        [Authorize]
        public Task<IActionResult> DeleteRoleFromUser(Guid userId, Guid roleId)
        {
            var request = new DeleteRoleFromUserRequest { UserId = userId, RoleId = roleId };

            return HandleAsync(request);
        }

        [HttpPost("claims/{claimId:guid}")]
        [Authorize]
        public Task<IActionResult> AddClaimToUser(Guid userId, Guid claimId, [FromBody] AddClaimToUserRequestBase requestBase)
        {
            var request = new AddClaimToUserRequest
            {
                UserId = userId,
                ClaimId = claimId,
                Value = requestBase.Value,
            };

            return HandleAsync(request);
        }

        [HttpDelete("claims/{claimId:guid}")]
        [Authorize]
        public Task<IActionResult> DeleteClaimFromUser(Guid userId, Guid claimId)
        {
            var request = new DeleteClaimFromUserRequest { UserId = userId, ClaimId = claimId };

            return HandleAsync(request);
        }
    }
}