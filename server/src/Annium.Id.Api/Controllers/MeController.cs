using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Core.Mapper;
using Annium.Id.Api.Payloads;
using Annium.Id.Api.Tools;
using Annium.Id.Api.Views;
using Annium.Id.AspNetCore;
using Annium.Id.Db;
using Annium.Id.Db.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Annium.Id.Api.Controllers
{
    [Route("me")]
    public class MeController : LocalizedServerController
    {
        private readonly IUserRepository userRepository;
        private readonly IUserLoginRepository userLoginRepository;
        private readonly ISecurityManager securityManager;
        private readonly IMapper mapper;

        public MeController(
            IUserRepository userRepository,
            IUserLoginRepository userLoginRepository,
            ISecurityManager securityManager,
            IMapper mapper,
            IStringLocalizer<MeController> localizer
        ) : base(localizer)
        {
            this.userRepository = userRepository;
            this.userLoginRepository = userLoginRepository;
            this.securityManager = securityManager;
            this.mapper = mapper;
        }

        [HttpPut]
        public async Task<IActionResult> RegisterUserAsync([FromBody] UserPayload userPayload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if ((await userRepository.FindByLoginAsync(userPayload.Login)) != null)
                return Conflict($"Login {userPayload.Login} is already used");

            if ((await userRepository.FindByEmailAsync(userPayload.Email)) != null)
                return Conflict($"Email {userPayload.Email} is already used");

            var passwordHash = securityManager.Hash(userPayload.Password);

            var user = new User(
                userPayload.Login,
                passwordHash,
                userPayload.Email
            );

            user = await userRepository.CreateAsync(user);

            return Ok(user);
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

        [HttpPost]
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
    }
}