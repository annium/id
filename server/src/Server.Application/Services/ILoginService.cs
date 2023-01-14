using System.Threading.Tasks;
using Server.Domain.Models;

namespace Server.Application.Services;

internal interface ILoginService
{
    Task<Tokens> LogUserInAsync(App app, User user);
}