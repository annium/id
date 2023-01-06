using System.Threading.Tasks;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Application.Services;

internal interface ILoginService
{
    Task<Tokens> LogUserInAsync(App app, User user);
}