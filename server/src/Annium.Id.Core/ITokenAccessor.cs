namespace Annium.Id.Core
{
    public interface ITokenAccessor
    {
        IdBaseToken GetBaseToken();

        IdAppToken GetAppToken();
    }
}