using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Annium.Id.Db.Entities
{
    [Table(nameof(IContext.CompanyRoleClaims))]
    internal class CompanyRoleClaim
    {
        [Required]
        public Guid RoleId { get; set; }
        [Required]
        public Guid ClaimId { get; set; }
        [Required]
        public string Value { get; set; }
    }
}