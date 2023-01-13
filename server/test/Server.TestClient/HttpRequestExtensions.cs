using Annium.Net.Http;
using Annium.Net.Mail;

namespace Server.TestClient;

public static class HttpRequestExtensions
{
    public static ExtendedClient ApiClient(
        this IHttpRequest request,
        TestEmailService emailService
    )
    {
        return new ExtendedClient(request, emailService);
    }
}