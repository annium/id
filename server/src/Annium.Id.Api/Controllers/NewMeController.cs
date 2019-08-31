using System;
using System.Net;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Core.Mapper;
using Annium.Core.Mediator;
using Annium.Data.Operations;
using Annium.Id.Api.Payloads;
using Annium.Id.Api.Views;
using Annium.Id.Application.Tools;
using Annium.Id.AspNetCore;
using Annium.Id.Db.Repositories;
using Annium.Id.ViewModels.User.Requests;
using Annium.Localization.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Annium.Id.Api.Controllers
{
    [Route("new/me")]
    public class NewMeController : LocalizedServerController
    {
        private readonly IUserRepository userRepository;
        private readonly IUserLoginRepository userLoginRepository;
        private readonly ISecurityManager securityManager;
        private readonly IMapper mapper;
        private readonly IMediator mediator;

        public NewMeController(
            IUserRepository userRepository,
            IUserLoginRepository userLoginRepository,
            ISecurityManager securityManager,
            IMapper mapper,
            IMediator mediator,
            ILocalizer<NewMeController> localizer
        ) : base(localizer)
        {
            this.userRepository = userRepository;
            this.userLoginRepository = userLoginRepository;
            this.securityManager = securityManager;
            this.mapper = mapper;
            this.mediator = mediator;
        }

        [HttpPost]
        public Task<IActionResult> RegisterUserAsync([FromBody] CreateUpdateUserRequest request)
        {
            return HandleAsync<CreateUpdateUserRequest, Guid>(request);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserAsync()
        {
            var user = await userRepository.GetByIdAsync(this.GetBaseId().UserId);
            if (user == null)
                return NotFound("User not found");

            // TODO: perhaps, add info about companies, user is member of
            return Ok(mapper.Map<UserPrivateView>(user));
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UserPayload userPayload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userRepository.GetByIdAsync(this.GetBaseId().UserId);

            if (userPayload.Login != user.Login && (await userRepository.FindByLoginAsync(userPayload.Login)) != null)
                return Conflict($"Login {userPayload.Login} is already used");

            if (userPayload.Email != user.Email && (await userRepository.FindByEmailAsync(userPayload.Email)) != null)
                return Conflict($"Email {userPayload.Email} is already used");

            user.Login = userPayload.Login;
            user.PasswordHash = securityManager.Hash(userPayload.Password);
            user.Email = userPayload.Email;

            user = await userRepository.UpdateAsync(user);

            return Ok(mapper.Map<UserPrivateView>(user));
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> UnregisterUserAsync()
        {
            var userId = this.GetBaseId().UserId;

            await userLoginRepository.DeleteAllByUserIdAsync(userId);
            await userRepository.DeleteByIdAsync(userId);

            return NoContent();
        }

        protected async Task<IActionResult> HandleAsync<TRequest, TResponse>(TRequest request)
        {
            var result = await mediator.SendAsync<ValueTuple<ModelStateDictionary, TRequest>, IStatusResult<HttpStatusCode, TResponse>>((ModelState, request));

            return new ObjectResult(result) { StatusCode = (int) result.Status };
        }
    }
}