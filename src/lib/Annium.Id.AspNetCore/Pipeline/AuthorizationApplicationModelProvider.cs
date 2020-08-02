using System;
using System.Collections.Generic;
using System.Linq;
using Annium.Id.Core;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Annium.Id.AspNetCore.Pipeline
{
    internal class AuthorizationApplicationModelProvider : IApplicationModelProvider
    {
        public int Order { get; } = -990;
        private readonly Func<AuthorizationFilterOptions, AuthorizationFilter> createAuthorizationFilter;
        private readonly Func<Policy, PolicyFilter> createPolicyFilter;
        private readonly IEnumerable<Policy> policies;
        private readonly IPolicyMapper mapper;

        public AuthorizationApplicationModelProvider(
            Func<AuthorizationFilterOptions, AuthorizationFilter> createAuthorizationFilter,
            Func<Policy, PolicyFilter> createPolicyFilter,
            IEnumerable<Policy> policies,
            IPolicyMapper mapper
        )
        {
            this.createAuthorizationFilter = createAuthorizationFilter;
            this.createPolicyFilter = createPolicyFilter;
            this.policies = policies;
            this.mapper = mapper;
        }

        public void OnProvidersExecuted(ApplicationModelProviderContext context)
        {
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
            var attribute = actionModel.Attributes.OfType<AuthorizeAttribute>().FirstOrDefault();

            //if no Authorize attribute - no filter needed
            if (attribute is null)
                return;

            var options = new AuthorizationFilterOptions(attribute.ValidateAudience, attribute.ValidateExpiration);
            actionModel.Filters.Add(createAuthorizationFilter(options));
            if (attribute.PolicyName is null)
                return;

            var policy = policies.FirstOrDefault(p => p.Name == attribute.PolicyName);
            if (policy is null)
                throw new ArgumentException($"Policy {attribute.PolicyName}, requested by {actionModel.DisplayName} is not registered");

            mapper.EnsureMappable(
                policy,
                actionModel.DisplayName,
                actionModel.Parameters.ToDictionary(p => p.ParameterName, p => p.ParameterType)
            );

            actionModel.Filters.Add(createPolicyFilter(policy));
        }
    }
}