using System.Drawing;
using System.Runtime.CompilerServices;

using Silk.NET.Maths;
using Silk.NET.Vulkan;
using Silk.NET.Windowing;

using SkiaSharp;

namespace Shimakaze.UI.Rendering.Vulkan;

internal sealed partial class VulkanSurfaceProvider : ISurfaceProvider
{
    private static VulkanApplication? s_vulkanApplication;
    private readonly IWindow _window;
    private readonly VulkanWindow _vulkanWindow;
    private ulong _frames;
    private Vector2D<int> _size;

    private bool _disposedValue;

    public VulkanSurfaceProvider(IWindow window)
    {
        if (window.VkSurface is null)
            throw new NotSupportedException("window.VkSurface is null");

        _window = window;

        s_vulkanApplication ??= new(window.VkSurface);
        _vulkanWindow = new(_window, s_vulkanApplication);

        _size = _window.FramebufferSize;
        _vulkanWindow.CreateSwapChain();
    }

    public SKSurface Begin()
    {
        if (_size != _window.FramebufferSize)
        {
            _vulkanWindow.RecreateSwapChain();
            _size = _window.FramebufferSize;
        }

        return _vulkanWindow.GetCurrent(_frames++);
    }

    public void End()
    {
        //throw new NotImplementedException();
    }

    private void Dispose(bool disposing)
    {
        if (_disposedValue)
        {
            return;
        }
        if (disposing)
        {
            // TODO: 释放托管状态(托管对象)
        }

        // TODO: 释放未托管的资源(未托管的对象)并重写终结器
        // TODO: 将大型字段设置为 null
        _disposedValue = true;
    }

    // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
    ~VulkanSurfaceProvider()
    {
        // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

}
