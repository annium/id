using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Annium.Id.Db.Entities
{
    [Table(nameof(IContext.Companies))]
    internal class Company
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        [Required]
        public Guid OwnerId { get; set; }

        public Guid? ParentId { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public string Name { get; set; }
    }
}