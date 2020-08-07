namespace Annium.Components.Forms
{
    public interface IState<T> : IState
    {
        T Value { get; }
        void Set(T value);
    }

    public interface IState
    {
        bool HasChanged { get; }
        bool HasBeenTouched { get; }
        void Reset();
        bool HasOnlyStatuses(params Status[] statuses);
        bool HasAllStatuses(params Status[] statuses);
        bool HasAnyStatus(params Status[] statuses);
    }
}