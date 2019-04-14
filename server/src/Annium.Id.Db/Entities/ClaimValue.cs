using System;

namespace Annium.Id.Db.Entities
{
    internal class ClaimValue
    {
        public Guid Id { get; set; }

        public string Key { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}