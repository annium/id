using System;
using Annium.Id.Core;
using Microsoft.AspNetCore.Mvc;

namespace Annium.Id.AspNetCore
{
    public static class ControllerExtensions
    {
        public static IdBaseToken GetBaseId(this ControllerBase controller)
        {
            if (!controller.HttpContext.Items.TryGetValue(Constants.IdBaseTokenProperty, out var raw))
                throw new InvalidOperationException($"User is not authenticated.");

            return (IdBaseToken) raw;
        }

        public static IdAppToken GetAppId(this ControllerBase controller)
        {
            if (!controller.HttpContext.Items.TryGetValue(Constants.IdAppTokenProperty, out var raw))
                throw new InvalidOperationException($"User is not authenticated.");

            return (IdAppToken) raw;
        }
    }
}