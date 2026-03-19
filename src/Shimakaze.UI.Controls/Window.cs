
using System.Drawing;

using Shimakaze.UI.Core;

namespace Shimakaze.UI;

public class Window : ContentElement, IPlatformWindowWrap, IDisposable
{
    private readonly PlatformWindow _window = new();
    private bool _disposedValue;

    public Window()
    {
        _window.Initialize += OnInitialize;
        _window.Update += OnUpdate;
        _window.Render += OnRender;
    }

    private void OnRender(PlatformWindow sender, UpdateEventArgs eventArgs)
    {
        using var renderer = Application.GetRenderer(_window);
        OnRender(renderer, eventArgs.DeltaTime);
    }

    private void OnUpdate(PlatformWindow sender, UpdateEventArgs eventArgs)
    {
        if (Visibility is not Visibility.Collapsed)
        {
            // 1. 如果是根节点 (没有父容器分配空间)，给一个默认大屏幕尺寸进行测量
            // 如果有父节点，理论上 Measure 应该在父节点的 ArrangeCore 里被调用过。
            // 但为了“能跑就行”，如果 IsMeasureValid 为 false，我们强制测一次。
            if (!IsMeasureValid)
            {
                Measure(new(Width, Height));
            }

            // 2. 如果排列失效，强制排列
            // 同样，根节点需要兜底一个矩形
            if (!IsArrangeValid)
            {
                // 如果是根节点，通常填满屏幕或根据 DesiredSize 决定
                // 这里简单处理：如果还没位置，就给个默认位置
                Arrange(new RectangleF(0, 0, DesiredSize.Width, DesiredSize.Height));
            }
        }

        OnUpdate( eventArgs.DeltaTime);
    }

    protected virtual void OnInitialize(PlatformWindow sender, EventArgs eventArgs)
    {
        Width = _window.Size.Width;
        Height = _window.Size.Height;
        OnInitialize();
    }

    PlatformWindow IPlatformWindowWrap.PlatformWindow => _window;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _window.Dispose();
            }

            _disposedValue = true;
        }
    }

    // ~Window()
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