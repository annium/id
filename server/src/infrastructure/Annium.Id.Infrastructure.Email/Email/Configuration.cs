namespace Annium.Id.Infrastructure.Email.Email
{
    public class Configuration : Net.Mail.Configuration
    {
        public string FromAddress { get; set; } = string.Empty;
        public string FromDisplay { get; set; } = string.Empty;
    }
}