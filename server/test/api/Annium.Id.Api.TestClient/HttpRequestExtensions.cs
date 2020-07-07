using Annium.Net.Http;
using Annium.Net.Mail;

namespace Annium.Id.Api.TestClient
{
    public static class HttpRequestExtensions
    {
        public static ExtendedClient ApiClient(
            this IHttpRequest request,
            TestEmailService emailService
        ) => new ExtendedClient(request, emailService);
    }
}