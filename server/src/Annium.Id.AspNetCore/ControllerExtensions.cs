using System;
using Microsoft.AspNetCore.Mvc;

namespace Annium.Id.AspNetCore
{
    public static class ControllerExtensions
    {
        public static IdToken GetId(this ControllerBase controller)
        {
            if (!controller.ControllerContext.ActionDescriptor.Properties.TryGetValue(Constants.IdTokenProperty, out var raw))
                throw new InvalidOperationException($"User is not authenticated.");

            return (IdToken) raw;
        }
    }
}