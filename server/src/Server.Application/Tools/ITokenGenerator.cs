using System.Threading.Tasks;
using Annium.Id.Core;
using Server.Domain.Models;

namespace Server.Application.Tools;

public interface ITokenGenerator
{
    Task<IdToken> GenerateToken(UserLogin login);
    Task<string> GenerateTokenString(UserLogin login);
}