using System;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Core.Mediator;
using Annium.Id.AspNetCore;
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
            return HandleAsync<RegisterMeRequest, Guid>(request);
        }

        [HttpGet]
        [Authorize]
        public Task<IActionResult> GetMe()
        {
            // TODO: perhaps, add info about companies, user is member of
            return  HandleAsync<GetMeRequest, MeResponse>(new GetMeRequest());
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