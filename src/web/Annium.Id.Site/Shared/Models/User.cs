using System;

namespace Annium.Id.Site.Shared.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Login { get; set; } = string.Empty;
    }
}