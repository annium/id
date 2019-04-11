using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Annium.Id.Db.Entities
{
    [Table(nameof(IContext.OrganizationClaims))]
    internal class OrganizationClaim
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        [Required]
        public Guid AppId { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public string Name { get; set; }
    }
}