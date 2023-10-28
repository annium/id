namespace Annium.Id.Core;

public interface ITokenWriter
{
    string WriteToken(IdToken token);
}
