using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Shimakaze.UI.Core.Dispatchers;

using Silk.NET.Windowing;

namespace Shimakaze.UI.Core;

public sealed class Application : IHost
{
    private bool _disposedValue;
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    private readonly IWindowOptionsProvider _windowOptionsProvider;
    private readonly IWindowProvider _windowProvider;

    public static Application Instance { get; private set; } = default!;

    public WindowManager WindowManager { get; }
    public Dispatcher Dispatcher { get; }
    public IServiceProvider Services { get; }
    public ILogger Logger { get; }

    public Application(
        IServiceProvider serviceProvider,
        Dispatcher dispatcher,
        WindowManager windowManager,
        ILogger<Application> logger,
        IWindowOptionsProvider windowOptionsProvider,
        IWindowProvider windowProvider)
    {
        if (Instance is not null)
            throw new InvalidOperationException("Application is already initialized.");

        Instance = this;

        WindowManager = windowManager;
        Dispatcher = dispatcher;
        Services = serviceProvider;
        Logger = logger;
        _windowOptionsProvider = windowOptionsProvider;
        _windowProvider = windowProvider;
    }


    internal IWindow CreateNativeWindow()
        => _windowProvider.CreateWindow(
            _windowOptionsProvider.CreateOptions());


    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.Register(_cancellationTokenSource.Cancel);
        Dispatcher.Start(_cancellationTokenSource.Token);
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        await _cancellationTokenSource.CancelAsync();

        await Task.WhenAny(
            Task.Run(
                () =>
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        if (Dispatcher.Wait(TimeSpan.FromSeconds(1)))
                            break;
                    }
                },
                cancellationToken),
            Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken)
        );
    }

    private void Dispose(bool disposing)
    {
        if (_disposedValue)
            return;

        if (disposing)
        {
            // TODO: 释放托管状态(托管对象)
        }

        // TODO: 释放未托管的资源(未托管的对象)并重写终结器
        // TODO: 将大型字段设置为 null
        _disposedValue = true;
    }

    // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
    // ~Application()
    // {
    //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}