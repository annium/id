using System;
using Annium.Id.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Annium.Id.Api.AppAuth
{
    internal static class ControllerExtensions
    {
        public const string AppProperty = "appProperty";

        public static App GetApp(this ControllerBase controller)
        {
            if (controller.ControllerContext.ActionDescriptor.Properties.TryGetValue(AppProperty, out var raw))
                return (App) raw;

            throw new InvalidOperationException($"App is not authenticated.");
        }
    }
}