using System;
using Annium.Id.AspNetCore.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Annium.Id.AspNetCore
{
    internal class AuthorizationFilter : IAuthorizationFilter
    {
        private readonly IServiceProvider serviceProvider;

        private readonly AuthorizeIdAttribute attribute;

        private readonly ITokenAccessor tokenAccessor;

        private readonly ITokenParser tokenParser;

        public AuthorizationFilter(
            IServiceProvider serviceProvider,
            AuthorizeIdAttribute attribute
        )
        {
            this.serviceProvider = serviceProvider;
            this.attribute = attribute;
            this.tokenAccessor = serviceProvider.GetRequiredService<ITokenAccessor>();
            this.tokenParser = serviceProvider.GetRequiredService<ITokenParser>();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var result = HandleAuthorization(context);
            if (result != null)
                context.Result = result;
        }

        private IActionResult HandleAuthorization(AuthorizationFilterContext context)
        {
            var(tokenString, readResult) = tokenAccessor.GetToken(context.HttpContext.Request);
            if (readResult != null)
                return readResult;

            var(token, parseResult) = tokenParser.ParseToken(tokenString);
            if (parseResult != null)
                return parseResult;

            context.ActionDescriptor.Properties[Constants.IdAttributeProperty] = attribute;
            context.ActionDescriptor.Properties[Constants.IdTokenProperty] = token;

            return null;
        }
    }
}