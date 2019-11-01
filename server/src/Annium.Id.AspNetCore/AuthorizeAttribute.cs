using System;
using NodaTime;

namespace Annium.Id.AspNetCore
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class AuthorizeAttribute : Attribute
    {
        public string? PolicyName { get; }
        public bool ValidateAudience { get; } = true;
        public Duration AllowedExpiration { get; } = Duration.Zero;

        public AuthorizeAttribute() { }

        public AuthorizeAttribute(
            bool validateAudience = true,
            int allowedExpirationInMinutes = 0
        )
        {
            ValidateAudience = validateAudience;
            AllowedExpiration = Duration.FromMinutes(allowedExpirationInMinutes);
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
            int allowedExpirationInMinutes = 0
        )
        {
            PolicyName = policyName;
            ValidateAudience = validateAudience;
            AllowedExpiration = Duration.FromMinutes(allowedExpirationInMinutes);
        }
    }
}