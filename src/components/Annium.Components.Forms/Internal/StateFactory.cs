using System;
using System.Collections.Generic;
using NodaTime;

namespace Annium.Components.Forms.Internal
{
    internal class StateFactory : IStateFactory
    {
        public IAtomicContainer<sbyte> Create(sbyte initialValue) => CreateAtomic(initialValue);

        public IAtomicContainer<short> Create(short initialValue) => CreateAtomic(initialValue);

        public IAtomicContainer<int> Create(int initialValue) => CreateAtomic(initialValue);

        public IAtomicContainer<long> Create(long initialValue) => CreateAtomic(initialValue);

        public IAtomicContainer<byte> Create(byte initialValue) => CreateAtomic(initialValue);

        public IAtomicContainer<ushort> Create(ushort initialValue) => CreateAtomic(initialValue);

        public IAtomicContainer<uint> Create(uint initialValue) => CreateAtomic(initialValue);

        public IAtomicContainer<ulong> Create(ulong initialValue) => CreateAtomic(initialValue);

        public IAtomicContainer<decimal> Create(decimal initialValue) => CreateAtomic(initialValue);

        public IAtomicContainer<float> Create(float initialValue) => CreateAtomic(initialValue);

        public IAtomicContainer<double> Create(double initialValue) => CreateAtomic(initialValue);

        public IAtomicContainer<string> Create(string initialValue) => CreateAtomic(initialValue);

        public IAtomicContainer<bool> Create(bool initialValue) => CreateAtomic(initialValue);

        public IAtomicContainer<DateTime> Create(DateTime initialValue) => CreateAtomic(initialValue);

        public IAtomicContainer<DateTimeOffset> Create(DateTimeOffset initialValue) => CreateAtomic(initialValue);

        public IAtomicContainer<Instant> Create(Instant initialValue) => CreateAtomic(initialValue);

        public IMapContainer<TKey, TValue> Create<TKey, TValue>(IDictionary<TKey, TValue> initialValue) where TKey : notnull where TValue : new() => CreateMap(initialValue);

        public IMapContainer<TKey, TValue> Create<TKey, TValue>(IReadOnlyDictionary<TKey, TValue> initialValue) where TKey : notnull where TValue : new() => CreateMap(initialValue);

        public IMapContainer<TKey, TValue> Create<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> initialValue) where TKey : notnull where TValue : new() => CreateMap(initialValue);

        public IArrayContainer<T> Create<T>(IEnumerable<T> initialValue)
            where T : new()
        {
            throw new NotImplementedException();
        }

        public IObjectContainer<T> Create<T>(T initialValue) where T : new() => new ObjectContainer<T>(this, initialValue);

        private IMapContainer<TKey, TValue> CreateMap<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> initialValue)
            where TKey : notnull
        {
            throw new NotImplementedException();
        }

        private IAtomicContainer<T> CreateAtomic<T>(T defaultValue)
            where T : IEquatable<T>
        {
            return new AtomicContainer<T>(defaultValue);
        }
    }
}