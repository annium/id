using System;
using System.Collections.Generic;
using NodaTime;

namespace Annium.Components.Forms.Internal
{
    internal class StateFactory : IStateFactory
    {
        public IArrayContainer<T> Create<T>(T[] initialValue) => CreateArray(initialValue);

        public IArrayContainer<T> Create<T>(IEnumerable<T> initialValue) => CreateArray(initialValue);

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

        public IAtomicContainer<DateTime> Create(DateTime initialValue) => CreateAtomic(initialValue);

        public IAtomicContainer<DateTimeOffset> Create(DateTimeOffset initialValue) => CreateAtomic(initialValue);

        public IAtomicContainer<Instant> Create(Instant initialValue) => CreateAtomic(initialValue);

        public IMapContainer<T> Create<T>(T initialValue)
        {
            throw new NotImplementedException();
        }

        private IArrayContainer<T> CreateArray<T>(IEnumerable<T> defaultValue)
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