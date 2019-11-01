using NodaTime;

namespace Annium.Id.AspNetCore.Pipeline
{
    internal class AuthorizationFilterOptions
    {
        public bool ValidateAudience { get; }
        public Duration AllowedExpiration { get; }

        public AuthorizationFilterOptions(
            bool validateAudience,
            Duration allowedExpiration
        )
        {
            ValidateAudience = validateAudience;
            AllowedExpiration = allowedExpiration;
        }
    }
}