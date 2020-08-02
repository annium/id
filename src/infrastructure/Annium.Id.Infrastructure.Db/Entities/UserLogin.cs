using System;

namespace Annium.Id.Infrastructure.Db.Entities
{
    internal class UserLogin : BaseIdEntity
    {
        public Guid AppId { get; set; }
        public Guid UserId { get; set; }
        public DateTime LoggedAt { get; set; }
        public string IPAddress { get; set; } = string.Empty;
        public string Client { get; set; } = string.Empty;
        public Guid RefreshToken { get; set; }
        public DateTime RefreshTokenExpires { get; set; }
    }
}