using System;

namespace Annium.Id.AspNetCore
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class AuthorizeAttribute : Attribute
    {
        public string? PolicyName { get; }
        public bool ValidateAudience { get; } = true;
        public bool ValidateExpiration { get; } = true;

        public AuthorizeAttribute()
        {
        }

        public AuthorizeAttribute(
            bool validateAudience = true,
            bool validateExpiration = true
        )
        {
            ValidateAudience = validateAudience;
            ValidateExpiration = validateExpiration;
        }

        public AuthorizeAttribute(
            string policyName
        )
        {
            PolicyName = policyName;
        }

        public AuthorizeAttribute(
            string policyName,
            bool validateAudience = true,
            bool validateExpiration = true
        )
        {
            PolicyName = policyName;
            ValidateAudience = validateAudience;
            ValidateExpiration = validateExpiration;
        }
    }
}