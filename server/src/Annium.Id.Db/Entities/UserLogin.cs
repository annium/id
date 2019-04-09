using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NodaTime;

namespace Annium.Id.Db.Entities
{
    [Table(nameof(IContext.UserLogins))]
    internal class UserLogin
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Instant LoggedAt { get; set; }

        [Required]
        public string IPAddress { get; set; }

        [Required]
        public string Client { get; set; }

        [Required]
        public Guid RefreshToken { get; set; }

        [Required]
        public Instant RefreshTokenExpires { get; set; }
    }
}