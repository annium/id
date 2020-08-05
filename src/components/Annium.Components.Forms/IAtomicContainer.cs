using System;

namespace Annium.Components.Forms
{
    public interface IAtomicContainer<T> : IState<T>
        where T : IEquatable<T>
    {
        // Status Status { get; }
    }
}