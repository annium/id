namespace Annium.Id.Api.Application.Tools;

public interface ISecurityManager
{
    string Hash(string data);
}