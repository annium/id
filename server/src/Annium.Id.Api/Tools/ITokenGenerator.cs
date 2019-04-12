using Annium.Id.Db;

namespace Annium.Id.Api.Tools
{
    public interface ITokenGenerator
    {
        string Generate(
            UserLogin login
        );
    }
}