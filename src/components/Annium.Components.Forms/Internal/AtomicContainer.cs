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
        private Status _status;

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
            _status = Status.None;
        }

        public void SetStatus(Status status)
        {
            _status = status;
        }

        public bool HasOnlyStatuses(params Status[] statuses)
        {
            foreach (var status in statuses)
                if (_status == status)
                    return true;

            return false;
        }

        public bool HasAllStatuses(params Status[] statuses)
        {
            foreach (var status in statuses)
                if (_status != status)
                    return false;

            return true;
        }

        public bool HasAnyStatus(params Status[] statuses)
        {
            foreach (var status in statuses)
                if (_status == status)
                    return true;

            return false;
        }
    }
}