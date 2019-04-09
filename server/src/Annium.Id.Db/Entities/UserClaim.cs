using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Annium.Id.Db.Entities
{
    [Table(nameof(IContext.UserClaims))]
    internal class UserClaim
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid ClaimId { get; set; }

        [Required]
        public string Value { get; set; }
    }
}