using System.Threading.Tasks;
using Annium.Id.Core;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Application.Tools
{
    public interface ITokenGenerator
    {
        Task<IdToken> GenerateToken(UserLogin login);
        Task<string> GenerateTokenString(UserLogin login);
    }
}