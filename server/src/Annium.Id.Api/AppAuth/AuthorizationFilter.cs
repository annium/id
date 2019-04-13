using System;
using System.Net;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Id.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;

namespace Annium.Id.Api.AppAuth
{
    internal class AuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly IServiceProvider serviceProvider;

        private readonly ITokenAccessor tokenAccessor;

        private readonly Func<Instant> getInstant;

        public AuthorizationFilter(
            IServiceProvider serviceProvider,
            ITokenAccessor tokenAccessor,
            Func<Instant> getInstant
        )
        {
            this.serviceProvider = serviceProvider;
            this.tokenAccessor = tokenAccessor;
            this.getInstant = getInstant;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var result = await HandleAuthorizationAsync(context);
            if (result != null)
                context.Result = result;
        }

        private async Task<IActionResult> HandleAuthorizationAsync(AuthorizationFilterContext context)
        {
            using(var scope = serviceProvider.CreateScope())
            {
                // try get token
                var(token, result) = tokenAccessor.GetToken(context.HttpContext.Request);
                if (result != null)
                    return result;

                var appRepository = scope.ServiceProvider.GetRequiredService<IAppRepository>();

                // try to find app
                var app = await appRepository.FindByApiTokenAsync(token);
                if (app == null)
                    return new ObjectResult(Result.Failure().Error("No app found with this token.")) { StatusCode = (int) HttpStatusCode.Forbidden };

                context.ActionDescriptor.Properties[ControllerExtensions.AppProperty] = app;

                return null;
            }
        }
    }
}