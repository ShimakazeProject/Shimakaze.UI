using System.Runtime.CompilerServices;

namespace Shimakaze.UI.Core.Dispatchers;

public interface IDispatcherTaskAwaiter : ICriticalNotifyCompletion
{
    bool IsCompleted { get; }
    void GetResult();
}

public interface IDispatcherTaskAwaiter<out TResult> : IDispatcherTaskAwaiter
{
    new TResult GetResult();
    void IDispatcherTaskAwaiter.GetResult() => GetResult();
}