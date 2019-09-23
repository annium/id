using System.Threading.Tasks;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Tools
{
    public interface ITokenGenerator
    {
        Task<string> GenerateToken(UserLogin login);
    }
}