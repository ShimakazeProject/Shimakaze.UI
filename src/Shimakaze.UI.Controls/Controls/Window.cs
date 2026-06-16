
using System.Drawing;

using Shimakaze.Foundation.Rendering.Extensions;
using Shimakaze.Foundation.Windowing;
using Shimakaze.Foundation.Windowing.Events;
using Shimakaze.Foundation.Windowing.Rendering;

namespace Shimakaze.UI.Controls;

public class Window : ContentElement,  IDisposable
{
    public readonly PlatformWindow PlatformWindow = new();
    private readonly PlatformWindowRendererProvider _rendererProvider;
    private bool _disposedValue;

    public Window(PlatformWindowRendererProvider rendererProvider)
    {
        _rendererProvider = rendererProvider;
        PlatformWindow.Initialize += OnInitialize;
        PlatformWindow.Update += OnUpdate;
        PlatformWindow.Render += OnRender;
        PlatformWindow.Resize += OnResize;
        PlatformWindow.MouseClick += Input_MouseClick;
    }

    private void Input_MouseClick(PlatformWindow sender, MouseClickEventArgs eventArgs)
    {
        if (Content is null)
            return;

        var element = Content.HitTestElement(eventArgs.Position.ToDrawingPoint());
        element?.OnClick();
    }

    private void OnResize(PlatformWindow sender, WindowResizeEventArgs eventArgs)
    {
        Width = eventArgs.NewSize.X;
        Height = eventArgs.NewSize.Y;
        InvalidateMeasure();
    }

    private void OnRender(PlatformWindow sender, UpdateEventArgs eventArgs)
    {
        using var renderer = _rendererProvider.GetRenderer(PlatformWindow);
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

        OnUpdate(eventArgs.DeltaTime);
    }

    protected virtual void OnInitialize(PlatformWindow sender, EventArgs eventArgs)
    {
        Width = PlatformWindow.Size.Width;
        Height = PlatformWindow.Size.Height;
        OnInitialize();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                PlatformWindow.Dispose();
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