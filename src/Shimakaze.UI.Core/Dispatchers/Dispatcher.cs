using System.Diagnostics;
using System.Threading.Channels;

namespace Shimakaze.UI.Core.Dispatchers;

public sealed class Dispatcher
{
    private readonly Channel<IDispatcherTask> _tasks = Channel.CreateUnbounded<IDispatcherTask>();


    private readonly Thread _thread;

    /// <summary>
    /// 初始化调度器，创建并配置UI线程
    /// </summary>
    public Dispatcher()
    {
        _thread = new(UIThreadRun)
        {
            Name = "UI Thread",
            IsBackground = false,
        };

        if (OperatingSystem.IsWindows())
            _thread.SetApartmentState(ApartmentState.STA);

    }

    private void UIThreadRun(object? state)
    {
        CancellationToken cancellationToken = state is null
            ? CancellationToken.None
            : (CancellationToken)state;

        DispatcherSynchronizationContext context = new(this);
        SynchronizationContext.SetSynchronizationContext(context);
        while (!cancellationToken.IsCancellationRequested)
        {
            // TODO: 处理事件循环
        }
    }

    internal void Start(CancellationToken cancellationToken) => _thread.Start(cancellationToken);

    internal bool Wait(TimeSpan timeout) => _thread.Join(timeout);

    /// <summary>
    /// 检查当前线程是否为UI线程
    /// </summary>
    /// <returns>如果当前线程是UI线程返回true，否则返回false</returns>
    internal bool CheckAccess() => _thread == Thread.CurrentThread;

    /// <summary>
    /// 将操作加入调度队列
    /// </summary>
    /// <param name="priority">调度优先级</param>
    /// <param name="handler">要执行的操作</param>
    private void Enqueue(IDispatcherTask task)
    {
        var result = _tasks.Writer.TryWrite(task);
        Debug.Assert(result);
    }

    /// <summary>
    /// 在 UI 线程上执行操作
    /// </summary>
    /// <param name="priority">调度优先级</param>
    /// <param name="action">要执行的操作</param>
    /// <param name="cancellationToken">取消令牌</param>
    public IDispatcherTask InvokeAsync(DispatcherPriority priority, Action action, CancellationToken cancellationToken = default)
        => InvokeAsync(priority, _ => action(), cancellationToken);

    /// <summary>
    /// 在 UI 线程上执行操作
    /// </summary>
    /// <param name="priority">调度优先级</param>
    /// <param name="action">要执行的操作</param>
    /// <param name="cancellationToken">取消令牌</param>
    public IDispatcherTask InvokeAsync(DispatcherPriority priority, Action<CancellationToken> action, CancellationToken cancellationToken = default)
    {
        DispatcherTask task = new(priority, action, cancellationToken);
        Enqueue(task);
        return task;
    }

    /// <summary>
    /// 在 UI 线程上执行操作
    /// </summary>
    /// <param name="priority">调度优先级</param>
    /// <param name="action">要执行的操作</param>
    /// <param name="cancellationToken">取消令牌</param>
    public IDispatcherTask<TResult> InvokeAsync<TResult>(DispatcherPriority priority, Func<TResult> action, CancellationToken cancellationToken = default)
        => InvokeAsync(priority, _ => action(), cancellationToken);

    /// <summary>
    /// 在 UI 线程上执行操作
    /// </summary>
    /// <param name="priority">调度优先级</param>
    /// <param name="action">要执行的操作</param>
    /// <param name="cancellationToken">取消令牌</param>
    public IDispatcherTask<TResult> InvokeAsync<TResult>(DispatcherPriority priority, Func<CancellationToken, TResult> action, CancellationToken cancellationToken = default)
    {
        DispatcherTask<TResult> task = new(priority, action, cancellationToken);
        Enqueue(task);
        return task;
    }
}