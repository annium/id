using System;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Core.Mediator;
using Annium.Id.AspNetCore;
using Annium.Id.ViewModels.Users.Requests;
using Annium.Id.ViewModels.Users.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Annium.Id.Api.Controllers
{
    [Route("me")]
    public class MeController : ServerController
    {
        public MeController(
            IMediator mediator
        ) : base(mediator)
        {

        }

        [HttpPost]
        public Task<IActionResult> RegisterUserAsync([FromBody] CreateUserRequest request)
        {
            return HandleAsync<CreateUserRequest, Guid>(request);
        }

        [HttpGet]
        [Authorize]
        public Task<IActionResult> GetUserAsync()
        {
            // TODO: perhaps, add info about companies, user is member of
            return  HandleAsync<GetUserRequest, UserPrivateResponse>(new GetUserRequest());
        }

        [HttpPut]
        [Authorize]
        public Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserRequest request)
        {
            return HandleAsync<UpdateUserRequest>(request);
        }

        [HttpDelete]
        [Authorize]
        public Task<IActionResult> UnregisterUserAsync()
        {
            return HandleAsync<DeleteUserRequest>(new DeleteUserRequest());
        }
    }
}