using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Core.Domain.Entities;
using Server.Email.Models;

namespace Server.Email;

public class EmailService : IEmailService
{
    private readonly Configuration _cfg;
    private readonly Annium.Net.Mail.IEmailService _emailService;

    public EmailService(
        Configuration cfg,
        Annium.Net.Mail.IEmailService emailService
    )
    {
        _cfg = cfg;
        _emailService = emailService;
    }

    public async Task<IBooleanResult> SendEmailConfirmationAsync(User user, Uri server)
    {
        using var message = GetMessage("Annium email confirmation");
        message.To.Add(user.Email);

        var data = new ConfirmEmailData
        {
            Server = server.GetLeftPart(UriPartial.Authority),
            Id = user.Id
        };

        return await _emailService.SendAsync(message, "confirm-email", data);
    }

    public async Task<IBooleanResult> SendRestoreAccessAsync(User user, Uri server, Tokens tokens)
    {
        using var message = GetMessage("Annium access restore");
        message.To.Add(user.Email);

        var data = new RestoreAccessData
        {
            Server = server.GetLeftPart(UriPartial.Authority),
            Tokens = tokens
        };

        return await _emailService.SendAsync(message, "restore-access", data);
    }

    private MailMessage GetMessage(string subject)
    {
        return new MailMessage
        {
            From = new MailAddress(_cfg.FromAddress, _cfg.FromDisplay),
            Subject = subject
        };
    }
}