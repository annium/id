using System;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.IdentityServer.Db;
using Microsoft.AspNetCore.Mvc;

namespace Annium.IdentityServer.Controllers
{
    [Route("app")]
    public class AppController : ServerController
    {
        private readonly IAppRepository appRepository;

        public AppController(
            IAppRepository appRepository
        )
        {
            this.appRepository = appRepository;
        }

        [HttpGet]
        public async Task<IActionResult> ListAsync()
        {
            var apps = await appRepository.GetAllAsync();

            return Ok(apps);
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