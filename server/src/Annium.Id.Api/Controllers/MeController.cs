using System;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Core.Mediator;
using Annium.Id.AspNetCore;
using Annium.Id.ViewModels.Login.Responses;
using Annium.Id.ViewModels.Me.Requests;
using Annium.Id.ViewModels.Me.Responses;
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
        public Task<IActionResult> RegisterMe([FromBody] RegisterMeRequest request)
        {
            return HandleAsync(request);
        }

        [HttpPost("{appId:guid}/confirm-email")]
        public Task<IActionResult> ConfirmMyEmail(Guid appId, [FromBody] ConfirmMyEmailRequestBase requestBase)
        {
            var request = new ConfirmMyEmailRequest
            {
                AppId = appId,
                Id = requestBase.Id,
            };

            return HandleAsync<ConfirmMyEmailRequest, TokensResponse>(request);
        }

        [HttpPost("{appId:guid}/restore-access")]
        public Task<IActionResult> RestoreMyAccess(Guid appId, [FromBody] RestoreMyAccessRequestBase requestBase)
        {
            var request = new RestoreMyAccessRequest
            {
                AppId = appId,
                Server = requestBase.Server,
                Email = requestBase.Email,
            };

            return HandleAsync(request);
        }

        [HttpGet]
        [Authorize(validateAudience: false)]
        public Task<IActionResult> GetMe()
        {
            // TODO: perhaps, add info about companies, user is member of
            return HandleAsync<GetMeRequest, MeResponse>(new GetMeRequest());
        }

        [HttpPut]
        [Authorize]
        public Task<IActionResult> UpdateMe([FromBody] UpdateMeRequest request)
        {
            return HandleAsync(request);
        }

        [HttpDelete]
        [Authorize]
        public Task<IActionResult> UnregisterMe()
        {
            return HandleAsync(new UnregisterMeRequest());
        }
    }
}