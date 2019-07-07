using System.Threading.Tasks;
using Annium.Id.Db;

namespace Annium.Id.Api.Tools
{
    public interface ITokenGenerator
    {
        string GenerateBaseToken(UserLogin login);

        Task<string> GenerateAppToken(UserAppLogin login);
    }
}