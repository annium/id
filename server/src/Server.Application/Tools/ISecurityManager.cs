namespace Server.Application.Tools;

public interface ISecurityManager
{
    string Hash(string data);
}
