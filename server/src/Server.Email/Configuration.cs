namespace Server.Email;

public class Configuration : Annium.Net.Mail.Configuration
{
    public string FromAddress { get; set; } = string.Empty;
    public string FromDisplay { get; set; } = string.Empty;
}
