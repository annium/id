using NodaTime;

namespace Annium.Id.Core
{
    public class TokenReadOptions
    {
        public bool ValidateAudience { get; set; } = true;
        public bool ValidateExpiration { get; set; } = true;
    }
}