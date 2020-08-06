using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NodaTime;

namespace Annium.Components.Forms.Internal
{
    internal class MapContainer<TKey, TValue> : IMapContainer<TKey, TValue>
        where TKey : notnull
    {
        public IReadOnlyDictionary<TKey, TValue> Value { get; private set; }
        public bool HasChanged => !Value.Equals(_initialValue);
        public bool HasBeenTouched { get; private set; }
        private readonly IStateFactory _stateFactory;
        private readonly IReadOnlyDictionary<TKey, TValue> _initialValue;

        public MapContainer(
            IStateFactory stateFactory,
            IReadOnlyDictionary<TKey, TValue> initialValue
        )
        {
            _stateFactory = stateFactory;
            // TODO: init inner states
            Value = _initialValue = initialValue;
        }

        public void Set(IReadOnlyDictionary<TKey, TValue> value)
        {
            Value = value;
            HasBeenTouched = true;
        }

        public void Reset()
        {
            Value = _initialValue;
            HasBeenTouched = false;
        }

        public IArrayContainer<TI> At<TI>(Expression<Func<IReadOnlyDictionary<TKey, TValue>, IEnumerable<TI>>> ex)
        {
            throw new NotImplementedException();
        }

        public IMapContainer<TK, TV> At<TK, TV>(Expression<Func<IReadOnlyDictionary<TKey, TValue>, IEnumerable<KeyValuePair<TK, TV>>>> ex) where TK : notnull
        {
            throw new NotImplementedException();
        }

        public IAtomicContainer<sbyte> At(Expression<Func<IReadOnlyDictionary<TKey, TValue>, sbyte>> ex)
        {
            throw new NotImplementedException();
        }

        public IAtomicContainer<short> At(Expression<Func<IReadOnlyDictionary<TKey, TValue>, short>> ex)
        {
            throw new NotImplementedException();
        }

        public IAtomicContainer<int> At(Expression<Func<IReadOnlyDictionary<TKey, TValue>, int>> ex)
        {
            throw new NotImplementedException();
        }

        public IAtomicContainer<long> At(Expression<Func<IReadOnlyDictionary<TKey, TValue>, long>> ex)
        {
            throw new NotImplementedException();
        }

        public IAtomicContainer<byte> At(Expression<Func<IReadOnlyDictionary<TKey, TValue>, byte>> ex)
        {
            throw new NotImplementedException();
        }

        public IAtomicContainer<ushort> At(Expression<Func<IReadOnlyDictionary<TKey, TValue>, ushort>> ex)
        {
            throw new NotImplementedException();
        }

        public IAtomicContainer<uint> At(Expression<Func<IReadOnlyDictionary<TKey, TValue>, uint>> ex)
        {
            throw new NotImplementedException();
        }

        public IAtomicContainer<ulong> At(Expression<Func<IReadOnlyDictionary<TKey, TValue>, ulong>> ex)
        {
            throw new NotImplementedException();
        }

        public IAtomicContainer<decimal> At(Expression<Func<IReadOnlyDictionary<TKey, TValue>, decimal>> ex)
        {
            throw new NotImplementedException();
        }

        public IAtomicContainer<float> At(Expression<Func<IReadOnlyDictionary<TKey, TValue>, float>> ex)
        {
            throw new NotImplementedException();
        }

        public IAtomicContainer<double> At(Expression<Func<IReadOnlyDictionary<TKey, TValue>, double>> ex)
        {
            throw new NotImplementedException();
        }

        public IAtomicContainer<string> At(Expression<Func<IReadOnlyDictionary<TKey, TValue>, string>> ex)
        {
            throw new NotImplementedException();
        }

        public IAtomicContainer<bool> At(Expression<Func<IReadOnlyDictionary<TKey, TValue>, bool>> ex)
        {
            throw new NotImplementedException();
        }

        public IAtomicContainer<DateTime> At(Expression<Func<IReadOnlyDictionary<TKey, TValue>, DateTime>> ex)
        {
            throw new NotImplementedException();
        }

        public IAtomicContainer<DateTimeOffset> At(Expression<Func<IReadOnlyDictionary<TKey, TValue>, DateTimeOffset>> ex)
        {
            throw new NotImplementedException();
        }

        public IAtomicContainer<Instant> At(Expression<Func<IReadOnlyDictionary<TKey, TValue>, Instant>> ex)
        {
            throw new NotImplementedException();
        }

        public IObjectContainer<TI> At<TI>(Expression<Func<IReadOnlyDictionary<TKey, TValue>, TI>> ex)
        {
            throw new NotImplementedException();
        }

        public IMapContainer<TKey, TValue> Add(TKey key, TValue item)
        {
            throw new NotImplementedException();
        }

        public IMapContainer<TKey, TValue> Delete(TKey key)
        {
            throw new NotImplementedException();
        }
    }
}