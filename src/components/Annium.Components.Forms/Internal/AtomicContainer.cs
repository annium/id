using System;

namespace Annium.Components.Forms.Internal
{
    internal class AtomicContainer<T> : IAtomicContainer<T>
        where T : IEquatable<T>
    {
        public T Value { get; private set; }
        public bool HasChanged => !Value.Equals(_initialValue);
        public bool HasBeenTouched { get; private set; }
        private readonly T _initialValue;

        public AtomicContainer(T initialValue)
        {
            Value = _initialValue = initialValue;
        }

        public void Set(T value)
        {
            Value = value;
            HasBeenTouched = true;
        }

        public void Reset()
        {
            Value = _initialValue;
            HasBeenTouched = false;
        }
    }
}