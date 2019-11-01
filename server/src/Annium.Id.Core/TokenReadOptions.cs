using NodaTime;

namespace Annium.Id.Core
{
    public class TokenReadOptions
    {
        public bool ValidateAudience { get; set; } = true;
        public Duration AllowedExpiration { get; set; } = Duration.Zero;
    }
}