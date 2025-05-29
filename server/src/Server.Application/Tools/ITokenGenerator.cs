using System.Threading.Tasks;
using Annium.Id.Core;
using Server.Domain.Models;

namespace Server.Application.Tools;

public interface ITokenGenerator
{
    Task<IdToken> GenerateTokenAsync(UserLogin login);
    Task<string> GenerateTokenStringAsync(UserLogin login);
}
