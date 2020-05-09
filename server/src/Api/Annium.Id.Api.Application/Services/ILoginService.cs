using System.Threading.Tasks;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Services
{
    internal interface ILoginService
    {
        Task<Tokens> LogUserInAsync(App app, User user);
    }
}