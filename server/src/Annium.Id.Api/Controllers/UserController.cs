using System;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Id.Api.Views;
using Annium.Id.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Annium.Id.Api.Controllers
{
    [Route("users")]
    public class UserController : LocalizedServerController
    {
        private readonly IUserRepository userRepository;

        public UserController(
            IUserRepository userRepository,
            IStringLocalizer<UserController> localizer
        ) : base(localizer)
        {
            this.userRepository = userRepository;
        }

        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetUserAsync(Guid userId)
        {
            var user = await userRepository.GetById(userId);
            if (user == null)
                return NotFound();

            // TODO: perhaps, add info about organizations, user is member of
            return Ok(new UserView(user));
        }
    }
}