using System;
using Microsoft.AspNetCore.Mvc;

namespace Annium.Id.AspNetCore
{
    public static class ControllerExtensions
    {
        public static Guid GetUserId(this ControllerBase controller)
        {
            if (!controller.ControllerContext.ActionDescriptor.Properties.TryGetValue(Constants.IdTokenProperty, out var raw))
                throw new InvalidOperationException($"User is not authenticated.");

            var token = (IdToken) raw;

            return token.UserId;
        }
    }
}