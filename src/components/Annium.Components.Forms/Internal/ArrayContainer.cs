using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NodaTime;

namespace Annium.Components.Forms.Internal
{
    internal class ArrayContainer<T> : IArrayContainer<T>
    {
        public T[] Value { get; }
        public bool HasChanged { get; }
        public bool HasBeenTouched { get; }
        public void Set(T[] value)
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public IArrayContainer<TI> At<TI>(Expression<Func<T[], IEnumerable<TI>>> ex)
        {
            throw new NotImplementedException();
        }

        public IMapContainer<TK, TV> At<TK, TV>(Expression<Func<T[], IEnumerable<KeyValuePair<TK, TV>>>> ex) where TK : notnull
        {
            throw new NotImplementedException();
        }

        public IAtomicContainer<sbyte> At(Expression<Func<T[], sbyte>> ex)
        {
            throw new NotImplementedException();
        }

        public IAtomicContainer<short> At(Expression<Func<T[], short>> ex)
        {
            throw new NotImplementedException();
        }

        public IAtomicContainer<int> At(Expression<Func<T[], int>> ex)
        {
            throw new NotImplementedException();
        }

        public IAtomicContainer<long> At(Expression<Func<T[], long>> ex)
        {
            throw new NotImplementedException();
        }

        public IAtomicContainer<byte> At(Expression<Func<T[], byte>> ex)
        {
            throw new NotImplementedException();
        }

        public IAtomicContainer<ushort> At(Expression<Func<T[], ushort>> ex)
        {
            throw new NotImplementedException();
        }

        public IAtomicContainer<uint> At(Expression<Func<T[], uint>> ex)
        {
            throw new NotImplementedException();
        }

        public IAtomicContainer<ulong> At(Expression<Func<T[], ulong>> ex)
        {
            throw new NotImplementedException();
        }

        public IAtomicContainer<decimal> At(Expression<Func<T[], decimal>> ex)
        {
            throw new NotImplementedException();
        }

        public IAtomicContainer<float> At(Expression<Func<T[], float>> ex)
        {
            throw new NotImplementedException();
        }

        public IAtomicContainer<double> At(Expression<Func<T[], double>> ex)
        {
            throw new NotImplementedException();
        }

        public IAtomicContainer<string> At(Expression<Func<T[], string>> ex)
        {
            throw new NotImplementedException();
        }

        public IAtomicContainer<bool> At(Expression<Func<T[], bool>> ex)
        {
            throw new NotImplementedException();
        }

        public IAtomicContainer<DateTime> At(Expression<Func<T[], DateTime>> ex)
        {
            throw new NotImplementedException();
        }

        public IAtomicContainer<DateTimeOffset> At(Expression<Func<T[], DateTimeOffset>> ex)
        {
            throw new NotImplementedException();
        }

        public IAtomicContainer<Instant> At(Expression<Func<T[], Instant>> ex)
        {
            throw new NotImplementedException();
        }

        public IObjectContainer<TI> At<TI>(Expression<Func<T[], TI>> ex)
        {
            throw new NotImplementedException();
        }

        public IArrayContainer<T> Add(T item)
        {
            throw new NotImplementedException();
        }

        public IArrayContainer<T> Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public IArrayContainer<T> Delete(int index)
        {
            throw new NotImplementedException();
        }
    }
}