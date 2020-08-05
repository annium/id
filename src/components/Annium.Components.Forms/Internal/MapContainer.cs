using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Annium.Components.Forms.Internal
{
    internal class MapContainer<T> : IMapContainer<T>
    {
        public T Value { get; private set; }
        public bool HasChanged => !Value.Equals(_initialValue);
        public bool HasBeenTouched { get; private set; }
        private readonly T _initialValue;

        public MapContainer(
            IStateFactory stateFactory,
            T initialValue
        )
        {
            // TODO: init inner states
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

        public IState<F> At<F>(Expression<Func<T, F>> ex)
        {
            throw new NotImplementedException();
        }

        public IArrayContainer<F> At<F>(Expression<Func<T, IEnumerable<F>>> ex)
        {
            throw new NotImplementedException();
        }
    }
}