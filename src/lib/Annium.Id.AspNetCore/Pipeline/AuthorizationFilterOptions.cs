namespace Annium.Id.AspNetCore.Pipeline
{
    internal class AuthorizationFilterOptions
    {
        public bool ValidateAudience { get; }
        public bool ValidateExpiration { get; }

        public AuthorizationFilterOptions(
            bool validateAudience,
            bool validateExpiration
        )
        {
            ValidateAudience = validateAudience;
            ValidateExpiration = validateExpiration;
        }
    }
}