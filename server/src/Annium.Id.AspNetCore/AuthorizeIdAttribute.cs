using System;

namespace Annium.Id.AspNetCore
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class AuthorizeIdAttribute : Attribute
    {
        public string PolicyName { get; }

        public string[] Parameters { get; }

        public AuthorizeIdAttribute() { }

        public AuthorizeIdAttribute(string policyName, params string[] parameters)
        {
            PolicyName = policyName;
            Parameters = parameters;
        }
    }
}