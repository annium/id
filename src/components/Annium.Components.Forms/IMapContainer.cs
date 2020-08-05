using System.Collections.Generic;

namespace Annium.Components.Forms
{
    public interface IMapContainer<TKey, TValue> : IState<IReadOnlyDictionary<TKey, TValue>>
    {
        IState<TValue> At(TKey key);
        IMapContainer<TKey, TValue> Add(TKey key, TValue item);
        IMapContainer<TKey, TValue> Delete(TKey key);
    }
}