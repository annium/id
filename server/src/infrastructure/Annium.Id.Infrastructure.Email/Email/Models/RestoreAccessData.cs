using Annium.Id.Domain.Entities;

namespace Annium.Id.Infrastructure.Email.Email.Models
{
    public class RestoreAccessData
    {
        public string Server { get; set; } = string.Empty;
        public Tokens Tokens { get; set; } = null!;
    }
}