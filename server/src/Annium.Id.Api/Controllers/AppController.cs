using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Id.Api.Payloads;
using Annium.Id.Api.Tools;
using Annium.Id.Api.Views;
using Annium.Id.AspNetCore;
using Annium.Id.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Annium.Id.Api.Controllers
{
    [Route("apps")]
    public class AppController : LocalizedServerController
    {
        private readonly IAppRepository appRepository;

        private readonly IUserRepository userRepository;

        private readonly ISecurityManager securityManager;

        public AppController(
            IAppRepository appRepository,
            IUserRepository userRepository,
            ISecurityManager securityManager,
            IStringLocalizer<AppController> localizer
        ) : base(localizer)
        {
            this.appRepository = appRepository;
            this.userRepository = userRepository;
            this.securityManager = securityManager;
        }

        [HttpPut]
        [AuthorizeId]
        public async Task<IActionResult> CreateAppAsync([FromBody] AppPayload appPayload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if ((await appRepository.FindByKeyAsync(appPayload.Key)) != null)
                return Conflict($"Application key {appPayload.Key} is already used");

            var app = new App(
                this.GetId().UserId,
                appPayload.Key,
                appPayload.Name,
                Guid.NewGuid()
            );

            app = await appRepository.CreateAsync(app);

            return Ok(new AppView(app));
        }

        [HttpGet("{appId:guid}/token")]
        [AuthorizeId]
        public async Task<IActionResult> GetAppApiTokenAsync(Guid appId)
        {
            var app = await appRepository.GetByIdAsync(appId);
            if (app == null)
                return NotFound();

            if (this.GetId().UserId != app.OwnerId)
                return Forbidden("Need to be owner to get api token");

            return Ok(app.ApiToken);
        }

        [HttpPost("{appId:guid}/token")]
        [AuthorizeId]
        public async Task<IActionResult> UpdateAppApiTokenAsync(Guid appId)
        {
            var app = await appRepository.GetByIdAsync(appId);
            if (app == null)
                return NotFound();

            if (this.GetId().UserId != app.OwnerId)
                return Forbidden("Need to be owner to update api token");

            var apiToken = Guid.NewGuid();
            await appRepository.UpdateApiTokenAsync(app.Id, apiToken);

            return Ok(apiToken);
        }

        [HttpGet]
        public async Task<IActionResult> ListAppsAsync()
        {
            var apps = await appRepository.GetAllAsync();

            return Ok(apps.Select(a => new AppView(a)).ToArray());
        }

        [HttpPost("{appId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> UpdateAppAsync(Guid appId, [FromBody] AppPayload appPayload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var app = await appRepository.GetByIdAsync(appId);
            if (app == null)
                return NotFound();

            if (this.GetId().UserId != app.OwnerId)
                return Forbidden("Need to be owner to update app");

            if (appPayload.Key != app.Key && (await appRepository.FindByKeyAsync(appPayload.Key)) != null)
                return Conflict($"Application key {appPayload.Key} is already used");

            app.Key = appPayload.Key;
            app.Name = appPayload.Name;

            app = await appRepository.UpdateAsync(app);

            return Ok(new AppView(app));
        }

        [HttpPost("{appId:guid}/owner/{userId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> SetAppOwnerAsync(Guid appId, Guid userId)
        {
            var app = await appRepository.GetByIdAsync(appId);
            if (app == null)
                return NotFound();

            if (this.GetId().UserId != app.OwnerId)
                return Forbidden("Need to be owner to change app owner");

            var user = await userRepository.GetById(userId);
            if (user == null)
                return NotFound();

            app.OwnerId = user.Id;

            app = await appRepository.UpdateAsync(app);

            return Ok(new AppView(app));
        }

        [HttpDelete("{appId:guid}")]
        [AuthorizeId]
        public async Task<IActionResult> DeleteAppAsync(Guid appId)
        {
            var app = await appRepository.GetByIdAsync(appId);
            if (app == null)
                return NotFound();

            if (this.GetId().UserId != app.OwnerId)
                return Forbidden("Need to be owner to delete app");

            await appRepository.DeleteByIdAsync(app.Id);

            return NoContent();
        }
    }
}