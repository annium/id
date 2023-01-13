using System.Threading.Tasks;
using Annium.Id.Core;
using Core.Domain.Entities;

namespace Server.Application.Tools;

public interface ITokenGenerator
{
    Task<IdToken> GenerateToken(UserLogin login);
    Task<string> GenerateTokenString(UserLogin login);
}