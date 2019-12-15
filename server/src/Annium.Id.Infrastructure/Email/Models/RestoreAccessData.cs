namespace Annium.Id.Infrastructure.Email.Models
{
    public class RestoreAccessData
    {
        public string Server { get; set; } = string.Empty;
        public string Token { get; set; } = null!;
    }
}