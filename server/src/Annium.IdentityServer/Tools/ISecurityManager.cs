namespace Annium.IdentityServer.Tools
{
    public interface ISecurityManager
    {
        string Hash(string data);
    }
}