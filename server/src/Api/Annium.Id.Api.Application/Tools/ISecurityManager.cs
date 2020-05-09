namespace Annium.Id.Application.Tools
{
    public interface ISecurityManager
    {
        string Hash(string data);
    }
}