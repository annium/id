using System;
using NodaTime;

namespace Annium.Id.Core
{
    public class AuthOptions
    {
        public Guid Audience { get; set; }
        public string PrivateKeyFile { get; set; } = string.Empty;
        public string PublicKeyFile { get; set; } = string.Empty;
        public Duration AccessTokenLifeTime { get; set; } = Duration.FromMinutes(30);

        internal AuthOptions() { }
    }
}