using System;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Data.Operations;
using Annium.Id.Api.Payloads;
using Annium.Id.Api.Tools;
using Annium.Id.Db;
using Microsoft.AspNetCore.Mvc;

namespace Annium.Id.Api.Controllers
{
    [Route("app")]
    public class AppController : ServerController
    {
        private readonly IAppRepository appRepository;
        private readonly ISecurityManager securityManager;

        public AppController(
            IAppRepository appRepository,
            ISecurityManager securityManager
        )
        {
            this.appRepository = appRepository;
            this.securityManager = securityManager;
        }

        [HttpGet]
        public async Task<IActionResult> ListAsync()
        {
            var apps = await appRepository.GetAllAsync();

            return Ok(apps);
        }

        [HttpPut]
        public async Task<IActionResult> CreateAsync([FromBody] AppPayload appPayload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var current = await appRepository.FindByLoginAsync(appPayload.Login);
            if (current != null)
                return Conflict(Result.Failure().Error("Application already exists"));

            var app = new App(
                appPayload.Login,
                securityManager.Hash(appPayload.Password),
                Guid.NewGuid(),
                appPayload.Name,
                appPayload.Email
            );

            app = await appRepository.CreateAsync(app);

            return Ok(app);
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