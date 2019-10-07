using System;

namespace Annium.Id.AspNetCore
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class AuthorizeAttribute : Attribute
    {
        public string? PolicyName { get; }

        public AuthorizeAttribute() { }

        public AuthorizeAttribute(string policyName)
        {
            PolicyName = policyName;
        }
    }
}