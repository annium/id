using System;
using System.Linq;
using System.Threading.Tasks;
using Annium.AspNetCore.Extensions;
using Annium.Core.Mapper;
using Annium.Id.Api.Payloads;
using Annium.Id.Api.Views;
using Annium.Id.AspNetCore;
using Annium.Id.Db;
using Annium.Id.Db.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Annium.Id.Api.Controllers
{
    [Route("apps")]
    public class AppController : LocalizedServerController
    {
        private readonly IAppRepository appRepository;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public AppController(
            IAppRepository appRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IStringLocalizer<AppController> localizer
        ) : base(localizer)
        {
            this.appRepository = appRepository;
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAppAsync([FromBody] AppPayload appPayload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if ((await appRepository.FindByKeyAsync(appPayload.Key)) != null)
                return Conflict($"Application key {appPayload.Key} is already used");

            var app = new App(
                this.GetBaseId().UserId,
                appPayload.Key,
                appPayload.Name,
                Guid.NewGuid()
            );

            app = await appRepository.CreateAsync(app);

            return Ok(mapper.Map<AppPrivateView>(app));
        }

        [HttpGet("{appId:guid}/token")]
        [Authorize]
        public async Task<IActionResult> GetAppApiTokenAsync(Guid appId)
        {
            var(app, result) = await VerifyAppOwnerAsync(appId, "get api token");
            if (result != null)
                return result;

            return Ok(app.ApiToken);
        }

        [HttpPut("{appId:guid}/token")]
        [Authorize]
        public async Task<IActionResult> UpdateAppApiTokenAsync(Guid appId)
        {
            var(app, result) = await VerifyAppOwnerAsync(appId, "update api token");
            if (result != null)
                return result;

            var apiToken = Guid.NewGuid();
            await appRepository.UpdateApiTokenAsync(app.Id, apiToken);

            return Ok(apiToken);
        }

        [HttpGet]
        public async Task<IActionResult> ListAppsAsync()
        {
            var apps = await appRepository.GetAllAsync();

            return Ok(apps.Select(mapper.Map<AppPublicView>).ToArray());
        }

        [HttpPut("{appId:guid}")]
        [Authorize]
        public async Task<IActionResult> UpdateAppAsync(Guid appId, [FromBody] AppPayload appPayload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var(app, result) = await VerifyAppOwnerAsync(appId, "update application");
            if (result != null)
                return result;

            if (appPayload.Key != app.Key && (await appRepository.FindByKeyAsync(appPayload.Key)) != null)
                return Conflict($"Application key {appPayload.Key} is already used");

            app.Key = appPayload.Key;
            app.Name = appPayload.Name;

            app = await appRepository.UpdateAsync(app);

            return Ok(mapper.Map<AppPrivateView>(app));
        }

        [HttpPut("{appId:guid}/owner/{userId:guid}")]
        [Authorize]
        public async Task<IActionResult> SetAppOwnerAsync(Guid appId, Guid userId)
        {
            var(app, result) = await VerifyAppOwnerAsync(appId, "set application owner");
            if (result != null)
                return result;

            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
                return NotFound("User not found");

            app.OwnerId = user.Id;

            app = await appRepository.UpdateAsync(app);

            return Ok(mapper.Map<AppPrivateView>(app));
        }

        [HttpDelete("{appId:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteAppAsync(Guid appId)
        {
            var(app, result) = await VerifyAppOwnerAsync(appId, "delete application");
            if (result != null)
                return result;

            await appRepository.DeleteByIdAsync(app.Id);

            return NoContent();
        }

        private async Task<ValueTuple<App, IActionResult>> VerifyAppOwnerAsync(Guid appId, string operation)
        {
            var app = await appRepository.GetByIdAsync(appId);
            if (app == null)
                return (null, NotFound("Application not found"));

            if (this.GetBaseId().UserId != app.OwnerId)
                return (null, Forbidden($"Need to be application owner to {operation}"));

            return (app, null);
        }
    }
}