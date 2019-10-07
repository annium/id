using System.Threading.Tasks;
using Annium.Id.Apps.AspNetCore.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Annium.Id.Apps.AspNetCore.Pipeline
{
    internal class AuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly RequestTokenReader tokenReader;

        public AuthorizationFilter(
            RequestTokenReader tokenReader
        )
        {
            this.tokenReader = tokenReader;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var result = await HandleAuthorizationAsync(context);
            if (result != null)
                context.Result = result;
        }

        private async Task<IActionResult> HandleAuthorizationAsync(AuthorizationFilterContext context)
        {
            // TODO: use JWT here, with single claim, containing AppId
            await Task.CompletedTask;

            return new NoContentResult();
        }
    }
}