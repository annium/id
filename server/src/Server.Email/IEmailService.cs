using System;
using System.Threading.Tasks;
using Annium.Data.Operations;
using Server.Domain.Models;

namespace Server.Email;

public interface IEmailService
{
    Task<IBooleanResult> SendEmailConfirmationAsync(User user, Uri server);
    Task<IBooleanResult> SendRestoreAccessAsync(User user, Uri server, Tokens tokens);
}