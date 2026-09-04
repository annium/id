using Annium.Net.Http;
using Annium.Net.Mail;
using Annium.Net.Mail.Testing;

namespace Server.Host.TestClient.Clients;

public static class HttpRequestExtensions
{
    public static ExtendedClient ApiClient(this IHttpRequest request, TestEmailService emailService)
    {
        return new ExtendedClient(request, emailService);
    }
}
