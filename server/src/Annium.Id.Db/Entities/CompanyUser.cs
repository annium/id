using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Annium.Id.Db.Entities
{
    [Table(nameof(IContext.CompanyUsers))]
    internal class CompanyUser
    {
        [Required]
        public Guid CompanyId { get; set; }

        [Required]
        public Guid UserId { get; set; }
    }
}