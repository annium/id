using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Annium.Id.Db.Entities
{
    [Table(nameof(IContext.OrganizationUsers))]
    internal class OrganizationUser
    {
        [Required]
        public Guid OrganizationId { get; set; }

        [Required]
        public Guid UserId { get; set; }
    }
}