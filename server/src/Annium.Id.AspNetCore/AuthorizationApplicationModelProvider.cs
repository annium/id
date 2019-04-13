using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Annium.Id.AspNetCore
{
    internal class AuthorizationApplicationModelProvider : IApplicationModelProvider
    {
        private readonly Func<AuthorizeIdAttribute, AuthorizationFilter> createAuthorizationFilter;

        public int Order { get; } = -990;

        public AuthorizationApplicationModelProvider(
            Func<AuthorizeIdAttribute, AuthorizationFilter> createAuthorizationFilter
        )
        {
            this.createAuthorizationFilter = createAuthorizationFilter;
        }

        public void OnProvidersExecuted(ApplicationModelProviderContext context)
        {
            //Intentionally empty
        }

        public void OnProvidersExecuting(ApplicationModelProviderContext context)
        {
            foreach (var controllerModel in context.Result.Controllers)
                ProcessControllerModel(controllerModel);
        }

        private void ProcessControllerModel(ControllerModel controllerModel)
        {
            foreach (var actionModel in controllerModel.Actions)
                ProcessActionModel(actionModel);
        }

        private void ProcessActionModel(ActionModel actionModel)
        {
            var attribute = actionModel.Attributes.OfType<AuthorizeIdAttribute>().FirstOrDefault();

            //if no Authorize attribute - no filter needed
            if (attribute == null)
                return;

            actionModel.Filters.Add(createAuthorizationFilter(attribute));
        }
    }
}