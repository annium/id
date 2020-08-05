namespace Annium.Components.Forms
{
    public interface IArrayContainer<T> : IState<T[]>
    {
        IState<T> At(int index);
        IArrayContainer<T> Add(T item);
        IArrayContainer<T> Insert(int index, T item);
        IArrayContainer<T> Delete(int index);
    }
}