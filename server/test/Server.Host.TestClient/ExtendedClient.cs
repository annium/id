using Annium.Net.Http;
using Annium.Net.Mail;

namespace Server.Host.TestClient;

public class ExtendedClient : Root
{
    public TestEmailService EmailService { get; }
    internal IHttpRequest Request { get; }

    public ExtendedClient(
        IHttpRequest request,
        TestEmailService emailService
    ) : base(request)
    {
        Request = request;
        EmailService = emailService;
    }
}