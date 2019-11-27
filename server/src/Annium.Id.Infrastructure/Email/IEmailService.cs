using System.Threading.Tasks;
using Annium.Data.Operations;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Infrastructure.Email
{
    public interface IEmailService
    {
        Task<IBooleanResult> SendEmailConfirmationAsync(User user);
        Task<IBooleanResult> SendRestoreAccessAsync(User user, Tokens tokens);
    }
}