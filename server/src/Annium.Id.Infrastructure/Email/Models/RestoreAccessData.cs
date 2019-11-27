using Annium.Id.Domain.Entities;

namespace Annium.Id.Infrastructure.Email.Models
{
    public class RestoreAccessData
    {
        public string Token { get; set; } = null!;
    }
}