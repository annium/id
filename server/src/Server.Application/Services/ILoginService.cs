using System.Threading.Tasks;
using Core.Domain.Entities;

namespace Server.Application.Services;

internal interface ILoginService
{
    Task<Tokens> LogUserInAsync(App app, User user);
}