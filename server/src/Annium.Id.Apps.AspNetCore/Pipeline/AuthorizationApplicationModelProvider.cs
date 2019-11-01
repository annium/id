using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Annium.Id.Apps.AspNetCore.Pipeline
{
    internal class AuthorizationApplicationModelProvider : IApplicationModelProvider
    {
        private readonly AuthorizationFilter authorizationFilter;

        public int Order { get; } = -990;

        public AuthorizationApplicationModelProvider(
            AuthorizationFilter authorizationFilter
        )
        {
            this.authorizationFilter = authorizationFilter;
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
            var attribute = actionModel.Attributes.OfType<AuthorizeAppAttribute>().FirstOrDefault();

            //if no Authorize attribute - no filter needed
            if (attribute is null)
                return;

            actionModel.Filters.Add(authorizationFilter);
        }
    }
}