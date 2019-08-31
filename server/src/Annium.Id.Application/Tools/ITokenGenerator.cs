using System.Threading.Tasks;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Tools
{
    public interface ITokenGenerator
    {
        string GenerateBaseToken(UserLogin login);

        Task<string> GenerateAppToken(UserAppLogin login);
    }
}