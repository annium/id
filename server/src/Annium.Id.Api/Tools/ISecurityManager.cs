namespace Annium.Id.Api.Tools
{
    public interface ISecurityManager
    {
        string Hash(string data);
    }
}