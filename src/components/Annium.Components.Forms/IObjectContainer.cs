using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Annium.Components.Forms
{
    public interface IObjectContainer<T> : IState<T>
    {
        IState<F> At<F>(Expression<Func<T, F>> ex);
        IState<F> At<F>(Expression<Func<T, F>> ex);
        IArrayContainer<F> At<F>(Expression<Func<T, IEnumerable<F>>> ex);
    }
}