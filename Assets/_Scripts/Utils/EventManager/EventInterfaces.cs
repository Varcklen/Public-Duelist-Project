namespace Project.Utils.Events.Interfaces
{
    /// <summary>
    /// Gives access to invoking a static event.
    /// </summary>
    public interface IEventInvoke
    {
        internal void Invoke();
    }

    /// <summary>
    /// Gives access to invoking a static event.
    /// </summary>
    public interface IEventInvoke<T>
    {
        internal void Invoke(T param);
    }

    /// <summary>
    /// Gives access to invoking a static event.
    /// </summary>
    public interface IFuncInvoke<T>
    {
        internal T Invoke();
    }
}
