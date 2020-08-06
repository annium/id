using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NodaTime;

namespace Annium.Components.Forms
{
    public interface IMapContainer<TKey, TValue> : IState<IReadOnlyDictionary<TKey, TValue>>
        where TKey : notnull
    {
        IArrayContainer<TI> At<TI>(Expression<Func<IReadOnlyDictionary<TKey, TValue>, IEnumerable<TI>>> ex);
        IMapContainer<TK, TV> At<TK, TV>(Expression<Func<IReadOnlyDictionary<TKey, TValue>, IEnumerable<KeyValuePair<TK, TV>>>> ex) where TK : notnull;
        IAtomicContainer<sbyte> At(Expression<Func<IReadOnlyDictionary<TKey, TValue>, sbyte>> ex);
        IAtomicContainer<short> At(Expression<Func<IReadOnlyDictionary<TKey, TValue>, short>> ex);
        IAtomicContainer<int> At(Expression<Func<IReadOnlyDictionary<TKey, TValue>, int>> ex);
        IAtomicContainer<long> At(Expression<Func<IReadOnlyDictionary<TKey, TValue>, long>> ex);
        IAtomicContainer<byte> At(Expression<Func<IReadOnlyDictionary<TKey, TValue>, byte>> ex);
        IAtomicContainer<ushort> At(Expression<Func<IReadOnlyDictionary<TKey, TValue>, ushort>> ex);
        IAtomicContainer<uint> At(Expression<Func<IReadOnlyDictionary<TKey, TValue>, uint>> ex);
        IAtomicContainer<ulong> At(Expression<Func<IReadOnlyDictionary<TKey, TValue>, ulong>> ex);
        IAtomicContainer<decimal> At(Expression<Func<IReadOnlyDictionary<TKey, TValue>, decimal>> ex);
        IAtomicContainer<float> At(Expression<Func<IReadOnlyDictionary<TKey, TValue>, float>> ex);
        IAtomicContainer<double> At(Expression<Func<IReadOnlyDictionary<TKey, TValue>, double>> ex);
        IAtomicContainer<string> At(Expression<Func<IReadOnlyDictionary<TKey, TValue>, string>> ex);
        IAtomicContainer<bool> At(Expression<Func<IReadOnlyDictionary<TKey, TValue>, bool>> ex);
        IAtomicContainer<DateTime> At(Expression<Func<IReadOnlyDictionary<TKey, TValue>, DateTime>> ex);
        IAtomicContainer<DateTimeOffset> At(Expression<Func<IReadOnlyDictionary<TKey, TValue>, DateTimeOffset>> ex);
        IAtomicContainer<Instant> At(Expression<Func<IReadOnlyDictionary<TKey, TValue>, Instant>> ex);
        IObjectContainer<TI> At<TI>(Expression<Func<IReadOnlyDictionary<TKey, TValue>, TI>> ex);
        IMapContainer<TKey, TValue> Add(TKey key, TValue item);
        IMapContainer<TKey, TValue> Delete(TKey key);
    }
}