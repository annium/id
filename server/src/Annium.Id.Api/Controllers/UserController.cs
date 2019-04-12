using System;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Id.Api.Payloads;
using Annium.Id.Api.Tools;
using Annium.Id.AspNetCore.Tools;
using Annium.Id.Db;
using Microsoft.AspNetCore.Mvc;
using NodaTime;

namespace Annium.Id.Api.Controllers
{
    [Route("users")]
    public class UserController : ServerController
    {
        private static readonly Duration refreshTokenLifeTime = Duration.FromDays(1);

        private readonly IUserRepository userRepository;

        private readonly IUserLoginRepository userLoginRepository;

        private readonly IIdentityDataAccessor identityDataAccessor;

        private readonly ISecurityManager securityManager;

        private readonly ITokenGenerator tokenGenerator;

        private readonly Func<Instant> getInstant;

        public UserController(
            IUserRepository userRepository,
            IUserLoginRepository userLoginRepository,
            IIdentityDataAccessor identityDataAccessor,
            ISecurityManager securityManager,
            ITokenGenerator tokenGenerator,
            Func<Instant> getInstant
        )
        {
            this.userRepository = userRepository;
            this.userLoginRepository = userLoginRepository;
            this.identityDataAccessor = identityDataAccessor;
            this.securityManager = securityManager;
            this.tokenGenerator = tokenGenerator;
            this.getInstant = getInstant;
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
                userPayload.FirstName,
                userPayload.LastName,
                userPayload.Email
            );

            user = await userRepository.CreateAsync(user);

            return Ok(user);
        }

        [HttpGet]
        // TODO: Auth
        public IActionResult GetUserAsync()
        {
            // add info about organizations, user is member of
            return NoContent();
        }

        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetUserAsync(Guid userId)
        {
            var user = await userRepository.GetById(userId);
            if (user == null)
                return NotFound();

            // TODO: perhaps, add info about organizations, user is member of
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserLoginPayload loginPayload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userRepository.FindByLoginAsync(loginPayload.Login);
            if (user == null)
                return Forbidden("Invalid login");

            if (securityManager.Hash(loginPayload.Password) != user.PasswordHash)
                return Forbidden("Invalid password");

            var instant = getInstant();
            var(ipAddress, client) = identityDataAccessor.GetIdentityData();
            var login = new UserLogin(user.Id, instant, ipAddress.ToString(), client, Guid.NewGuid(), instant + refreshTokenLifeTime);

            login = await userLoginRepository.CreateAsync(login);

            var token = tokenGenerator.Generate(login);

            return Ok(token);
        }

        [HttpPost("logout")]
        // TODO: Auth
        public IActionResult LogoutAsync()
        {
            return NoContent();
        }

        [HttpPost("token")]
        // TODO: Auth
        public IActionResult UpdateTokenAsync()
        {
            return NoContent();
        }

        [HttpPost]
        // TODO: Auth
        public IActionResult UpdateUserAsync()
        {
            return NoContent();
        }

        [HttpDelete]
        // TODO: Auth
        public IActionResult UnregisterUserAsync()
        {
            return NoContent();
        }
    }
}