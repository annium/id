using System;

namespace Annium.Id.Db.Entities
{
    internal class App : BaseIdEntity
    {
        public Guid OwnerId { get; set; }
        public User Owner { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public Guid ApiToken { get; set; }
    }
}