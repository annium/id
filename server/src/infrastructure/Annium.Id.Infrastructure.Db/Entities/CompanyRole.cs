using System;
using System.Collections.Generic;

namespace Annium.Id.Infrastructure.Db.Entities
{
    internal class CompanyRole : BaseIdEntity
    {
        public Guid AppId { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public ICollection<CompanyRoleClaim> Claims { get; set; } = null!;
    }
}