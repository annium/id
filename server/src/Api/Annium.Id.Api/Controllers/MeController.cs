using System;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Core.Mediator;
using Annium.Data.Operations;
using Annium.Id.AspNetCore;
using Annium.Id.Api.ViewModels.Login.Responses;
using Annium.Id.Api.ViewModels.Me.Requests;
using Annium.Id.Api.ViewModels.Me.Responses;
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
        public Task<IResult> RegisterMe([FromBody] RegisterMeRequest request)
        {
            return HandleAsync(request);
        }

        [HttpPost("{appId:guid}/confirm-email")]
        public Task<IResult<TokensResponse>> ConfirmMyEmail(Guid appId, [FromBody] ConfirmMyEmailRequestBase requestBase)
        {
            var request = new ConfirmMyEmailRequest
            {
                AppId = appId,
                Id = requestBase.Id,
            };

            return HandleAsync<ConfirmMyEmailRequest, TokensResponse>(request);
        }

        [HttpPost("{appId:guid}/restore-access")]
        public Task<IResult> RestoreMyAccess(Guid appId, [FromBody] RestoreMyAccessRequestBase requestBase)
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
        public Task<IResult<MeResponse>> GetMe()
        {
            // TODO: perhaps, add info about companies, user is member of
            return HandleAsync<GetMeRequest, MeResponse>(new GetMeRequest());
        }

        [HttpPut("profile")]
        [Authorize(validateAudience: false)]
        public Task<IResult> UpdateMyProfile([FromBody] UpdateMyProfileRequest request)
        {
            return HandleAsync(request);
        }

        [HttpPut("password")]
        [Authorize(validateAudience: false)]
        public Task<IResult> UpdateMyPassword([FromBody] UpdateMyPasswordRequest request)
        {
            return HandleAsync(request);
        }

        [HttpDelete]
        [Authorize(validateAudience: false)]
        public Task<IResult> UnregisterMe()
        {
            return HandleAsync(new UnregisterMeRequest());
        }
    }
}