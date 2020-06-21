using Annium.Id.Domain.Entities;

namespace Annium.Id.Infrastructure.Email.Models
{
    public class RestoreAccessData
    {
        public string Server { get; set; } = string.Empty;
        public Tokens Tokens { get; set; } = null!;
    }
}