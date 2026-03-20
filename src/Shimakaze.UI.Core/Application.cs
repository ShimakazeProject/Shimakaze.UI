using Microsoft.Extensions.Hosting;

using Shimakaze.UI.Core;
using Shimakaze.UI.Core.Threading;

using Silk.NET.Windowing;

namespace Shimakaze.UI;

public sealed class Application : IHost
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly IPlatformWindowOptionsProvider _windowOptionsProvider;
    private readonly IPlatformWindowProvider _windowProvider;
    private readonly IHostApplicationLifetime _lifetime;
    private readonly Dispatcher _dispatcher;
    private readonly WindowManager _windowManager;

    public Application(
        IServiceProvider serviceProvider,
        IPlatformWindowOptionsProvider windowOptionsProvider,
        IPlatformWindowProvider windowProvider,
        IHostApplicationLifetime lifetime,
        Dispatcher dispatcher,
        WindowManager windowManager)
    {
        if (Instance is not null)
            throw new InvalidOperationException("Application is already initialized.");

        Instance = this;
        _windowOptionsProvider = windowOptionsProvider;
        _windowProvider = windowProvider;
        _lifetime = lifetime;
        _dispatcher = dispatcher;
        _windowManager = windowManager;
        Services = serviceProvider;
    }

    public static Application Instance { get; private set; } = default!;
    public IServiceProvider Services { get; }

    internal IWindow CreateNativeWindow()
        => _windowProvider.CreateWindow(
            _windowOptionsProvider.CreateOptions());

    private void MainLoop()
    {
        // 初始化 Windowing 运行时（如 GLFW）
        _windowManager.Initialize();

        while (!_lifetime.ApplicationStopping.IsCancellationRequested)
        {
            if (_dispatcher.Dequeue(out var task))
                task.Invoke();

            // 如果没有窗口且允许自动退出，则退出
            if (_windowManager.IsEmpty)
                break;

            // 2. 处理窗口事件和渲染
            _windowManager.Update();

            Thread.Yield();
        }

        Shutdown();
    }

    public void Shutdown() => _lifetime.StopApplication();

    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.Register(_cancellationTokenSource.Cancel);
        _dispatcher.Start(MainLoop);
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
                        if (_dispatcher.Wait(TimeSpan.FromSeconds(1)))
                            break;
                    }
                },
                cancellationToken),
            Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken)
        );
    }

    public void Dispose()
    {
    }
}