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
    [Route("apps")]
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

        [HttpPut]
        public async Task<IActionResult> CreateAppAsync([FromBody] AppPayload appPayload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var current = await appRepository.FindByKeyAsync(appPayload.Key);
            if (current != null)
                return Conflict(Result.Failure().Error("Application already exists"));

            var app = new App(
                Guid.NewGuid(), // TODO: real user id
                appPayload.Key,
                appPayload.Name,
                Guid.NewGuid()
            );

            app = await appRepository.CreateAsync(app);

            return Ok(app);
        }

        [HttpGet]
        public async Task<IActionResult> ListAppsAsync()
        {
            var apps = await appRepository.GetAllAsync();

            return Ok(apps);
        }

        [HttpPost("{appId:guid}")]
        public IActionResult UpdateAppAsync(Guid appId)
        {
            return Ok($"update app {appId}");
        }

        [HttpPost("{appId:guid}/set-owner/{userId:guid}")]
        // TODO: Auth
        public IActionResult SetAppOwnerAsync(Guid appId, Guid userId)
        {
            return NoContent();
        }

        [HttpDelete("{appId:guid}")]
        public IActionResult DeleteAppAsync(Guid appId)
        {
            return Ok($"delete app {appId}");
        }
    }
}