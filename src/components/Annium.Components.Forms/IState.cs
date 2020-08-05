namespace Annium.Components.Forms
{
    public interface IState<T>
    {
        T Value { get; }
        bool HasChanged { get; }
        bool HasBeenTouched { get; }
        void Set(T value);
        void Reset();
    }
}