using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Annium.Id.Db.Entities
{
    [Table(nameof(IContext.UserRoles))]
    internal class UserRole
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid RoleId { get; set; }
    }
}