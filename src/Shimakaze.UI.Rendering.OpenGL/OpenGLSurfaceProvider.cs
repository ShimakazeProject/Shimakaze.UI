using System.Diagnostics;

using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

using SkiaSharp;

namespace Shimakaze.UI.Rendering.OpenGL;

internal sealed class OpenGLSurfaceProvider(IWindow window) : ISurfaceProvider
{
    private static GRContext? s_globalContext;
    private GL? _gl;
    private GRContext? _grContext;
    private GRGlFramebufferInfo? _grGlFramebufferInfo;
    private GRBackendRenderTarget? _grBackendRenderTarget;
    private Vector2D<int> _framebufferSize;

    private SKSurface? _surface;
    private bool _disposedValue;

    public SKSurface Begin()
    {
        if (_surface is not null && window.FramebufferSize == _framebufferSize)
            return _surface;

        _framebufferSize = window.FramebufferSize;

        _gl ??= window.CreateOpenGL();
        Debug.Assert(_gl is not null);

        if (_grContext is not { IsAbandoned: false })
        {
            if (s_globalContext is not { IsAbandoned: false })
                s_globalContext = GRContext.CreateGl();
            if (s_globalContext is not { IsAbandoned: false })
                s_globalContext = GRContext.CreateGl(GRGlInterface.Create());

            _grContext = s_globalContext;
            if (_grContext is not { IsAbandoned: false })
                _grContext = GRContext.CreateGl(GRGlInterface.CreateOpenGl(name => _gl.Context.GetProcAddress(name)));
        }
        Debug.Assert(_grContext is not null);

        // 获取 GL 的 framebuffer 信息
        _grGlFramebufferInfo ??= new(
            0,
            SKColorType.Rgba8888.ToGlSizedFormat());

        // 创建渲染目标描述
        _grBackendRenderTarget = new(
            _framebufferSize.X,
            _framebufferSize.Y,
            0,
            8,
            _grGlFramebufferInfo.Value
        );

        // 创建 Skia 画布
        _surface = SKSurface.Create(
            _grContext,
            _grBackendRenderTarget,
            GRSurfaceOrigin.BottomLeft,
            SKColorType.Rgba8888);
        Debug.Assert(_surface is not null);

        return _surface;
    }

    public void End()
    {
        _grContext?.Submit();
        if (!window.ShouldSwapAutomatically)
            window.SwapBuffers();
    }

    private void Dispose(bool disposing)
    {
        if (_disposedValue)
            return;

        if (disposing)
        {
            _surface?.Dispose();
            _grBackendRenderTarget?.Dispose();
            _grContext?.Dispose();
            _gl?.Dispose();
        }

        _grGlFramebufferInfo = null;
        _disposedValue = true;
    }

    ~OpenGLSurfaceProvider()
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