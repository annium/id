using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Annium.Id.Db.Entities
{
    [Table(nameof(IContext.UserOrganizations))]
    internal class UserOrganization
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid OrganizationId { get; set; }
    }
}