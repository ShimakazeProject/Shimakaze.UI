namespace Shimakaze.UI.Core.Dispatchers;

public interface IDispatcherTask
{
    DispatcherPriority Priority { get; }
    CancellationToken CancellationToken { get; }
    internal void Invoke();
    IDispatcherTaskAwaiter GetAwaiter();
}

public interface IDispatcherTask<out TResult> : IDispatcherTask
{
    new IDispatcherTaskAwaiter<TResult> GetAwaiter();
    IDispatcherTaskAwaiter IDispatcherTask.GetAwaiter() => GetAwaiter();
}