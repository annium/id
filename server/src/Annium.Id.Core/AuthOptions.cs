using NodaTime;

namespace Annium.Id.Core
{
    public class AuthOptions
    {
        public string Audience { get; set; } = string.Empty;
        public string PrivateKeyFile { get; set; } = string.Empty;
        public string PublicKeyFile { get; set; } = string.Empty;
        public Duration AccessTokenLifeTime { get; set; } = Duration.FromMinutes(30);

        internal AuthOptions() { }
    }
}