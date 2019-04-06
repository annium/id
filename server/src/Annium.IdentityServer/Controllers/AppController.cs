using System;
using Annium.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Annium.IdentityServer.Controllers
{
    [Route("app")]
    public class AppController : ServerController
    {
        public AppController()
        {

        }

        [HttpGet]
        public IActionResult ListAsync()
        {
            return Ok("get list of apps");
        }

        [HttpPut]
        public IActionResult CreateAsync()
        {
            return Ok("create app");
        }

        [HttpPost("{id:guid}")]
        public IActionResult UpdateAsync(Guid id)
        {
            return Ok($"update app {id}");
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteAsync(Guid id)
        {
            return Ok($"delete app {id}");
        }
    }
}