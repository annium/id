using System;
using Annium.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Annium.Id.Api.Controllers
{
    [Route("users")]
    public class UserController : ServerController
    {
        // register user - by user
        // get own info - authorized
        // get user info - open
        // login user - by user
        // logout user - by user
        // update user token - by user
        // update user info - by user
        // unregister user - by user

        public UserController() { }

        [HttpPut]
        public IActionResult RegisterUserAsync()
        {
            return NoContent();
        }

        [HttpGet]
        // TODO: Auth
        public IActionResult GetUserAsync()
        {
            // add info about organizations, user is member of
            return NoContent();
        }

        [HttpGet("{userId:guid}")]
        public IActionResult GetUserAsync(Guid userId)
        {
            // add info about organizations, user is member of
            return NoContent();
        }

        [HttpPost("login")]
        // TODO: Auth
        public IActionResult LoginAsync()
        {
            return NoContent();
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