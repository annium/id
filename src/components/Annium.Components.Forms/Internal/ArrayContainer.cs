using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Annium.Core.Mapper;
using Annium.Data.Models.Extensions;
using NodaTime;

namespace Annium.Components.Forms.Internal
{
    internal class ArrayContainer<T> : IArrayContainer<T>
        where T : notnull, new()
    {
        private static MethodInfo Factory { get; } = typeof(IStateFactory).GetMethod(nameof(IStateFactory.Create), new[] { typeof(T) });
        public T[] Value => CreateValue();
        public bool HasChanged => !Value.IsShallowEqual(_initialValue, _mapper);
        public bool HasBeenTouched => _hasBeenTouched || _states.Any(x => x.HasBeenTouched);
        private readonly IStateFactory _stateFactory;
        private readonly IEnumerable<T> _initialValue;
        private readonly IMapper _mapper;
        private readonly IList<IState<T>> _states;
        private bool _hasBeenTouched;

        public ArrayContainer(
            IStateFactory stateFactory,
            IEnumerable<T> initialValue,
            IMapper mapper
        )
        {
            _stateFactory = stateFactory;
            _initialValue = initialValue;
            _mapper = mapper;
            _states = new List<IState<T>>();
            Reset();
        }

        public void Set(T[] value)
        {
            var updated = Math.Min(_states.Count, value.Length);
            for (int i = 0; i < updated; i++)
                _states[i].Set(value[i]);

            var added = Math.Max(value.Length - _states.Count, 0) + updated;
            for (int i = updated; i < added; i++)
                _states.Add((IState<T>) Factory.Invoke(_stateFactory, new[] { (object) value[i] }));

            var removed = Math.Max(_states.Count - value.Length, 0) + updated;
            for (int i = updated; i < removed; i++)
                _states.RemoveAt(i);

            _hasBeenTouched = true;
        }

        public void Reset()
        {
            _states.Clear();
            foreach (var item in _initialValue)
                _states.Add((IState<T>) Factory.Invoke(_stateFactory, new[] { (object) item }));

            _hasBeenTouched = false;
        }

        public bool IsStatus(params Status[] statuses)
        {
            foreach (var state in _states)
                if (!state.IsStatus(statuses))
                    return false;

            return true;
        }

        public bool HasStatus(params Status[] statuses)
        {
            foreach (var state in _states)
                if (state.HasStatus(statuses))
                    return true;

            return false;
        }

        public IArrayContainer<TI> At<TI>(Expression<Func<T[], IEnumerable<TI>>> ex) where TI : notnull, new() => At<IArrayContainer<TI>>(ex);
        public IMapContainer<TK, TV> At<TK, TV>(Expression<Func<T[], IEnumerable<KeyValuePair<TK, TV>>>> ex) where TK : notnull where TV : notnull, new() => At<IMapContainer<TK, TV>>(ex);
        public IAtomicContainer<sbyte> At(Expression<Func<T[], sbyte>> ex) => At<IAtomicContainer<sbyte>>(ex);
        public IAtomicContainer<short> At(Expression<Func<T[], short>> ex) => At<IAtomicContainer<short>>(ex);
        public IAtomicContainer<int> At(Expression<Func<T[], int>> ex) => At<IAtomicContainer<int>>(ex);
        public IAtomicContainer<long> At(Expression<Func<T[], long>> ex) => At<IAtomicContainer<long>>(ex);
        public IAtomicContainer<byte> At(Expression<Func<T[], byte>> ex) => At<IAtomicContainer<byte>>(ex);
        public IAtomicContainer<ushort> At(Expression<Func<T[], ushort>> ex) => At<IAtomicContainer<ushort>>(ex);
        public IAtomicContainer<uint> At(Expression<Func<T[], uint>> ex) => At<IAtomicContainer<uint>>(ex);
        public IAtomicContainer<ulong> At(Expression<Func<T[], ulong>> ex) => At<IAtomicContainer<ulong>>(ex);
        public IAtomicContainer<decimal> At(Expression<Func<T[], decimal>> ex) => At<IAtomicContainer<decimal>>(ex);
        public IAtomicContainer<float> At(Expression<Func<T[], float>> ex) => At<IAtomicContainer<float>>(ex);
        public IAtomicContainer<double> At(Expression<Func<T[], double>> ex) => At<IAtomicContainer<double>>(ex);
        public IAtomicContainer<string> At(Expression<Func<T[], string>> ex) => At<IAtomicContainer<string>>(ex);
        public IAtomicContainer<bool> At(Expression<Func<T[], bool>> ex) => At<IAtomicContainer<bool>>(ex);
        public IAtomicContainer<DateTime> At(Expression<Func<T[], DateTime>> ex) => At<IAtomicContainer<DateTime>>(ex);
        public IAtomicContainer<DateTimeOffset> At(Expression<Func<T[], DateTimeOffset>> ex) => At<IAtomicContainer<DateTimeOffset>>(ex);
        public IAtomicContainer<Instant> At(Expression<Func<T[], Instant>> ex) => At<IAtomicContainer<Instant>>(ex);
        public IObjectContainer<TI> At<TI>(Expression<Func<T[], TI>> ex) where TI : notnull, new() => At<IObjectContainer<TI>>(ex);

        public void Add(T item)
        {
            _states.Add((IState<T>) Factory.Invoke(_stateFactory, new[] { (object) item }));
            _hasBeenTouched = true;
        }

        public void Insert(int index, T item)
        {
            _states.Insert(index, (IState<T>) Factory.Invoke(_stateFactory, new[] { (object) item }));
            _hasBeenTouched = true;
        }

        public void RemoveAt(int index)
        {
            _states.RemoveAt(index);
            _hasBeenTouched = true;
        }

        private TX At<TX>(LambdaExpression ex) where TX : IState
        {
            var index = ResolveIndex(ex);
            if (index < 0 || index >= _states.Count)
                throw new IndexOutOfRangeException($"There's no item in container with index {index}");

            return (TX) _states[index];
        }

        private T[] CreateValue()
        {
            var value = new List<T>();

            foreach (var state in _states)
                value.Add(state.Value);

            return value.ToArray();
        }

        private int ResolveIndex(LambdaExpression ex)
        {
            if (ex.Body is BinaryExpression body && body.NodeType == ExpressionType.ArrayIndex)
            {
                if (body.Right is ConstantExpression constant && constant.Value?.GetType() == typeof(int))
                    return (int) constant.Value;

                if (body.Right is MemberExpression member && member.Expression is ConstantExpression)
                {
                    var value = Expression.Lambda(body.Right).Compile().DynamicInvoke();
                    if (value is int intValue)
                        return intValue;
                }
            }

            throw new ArgumentException($"{ex} is not a valid array index expression");
        }
    }
}