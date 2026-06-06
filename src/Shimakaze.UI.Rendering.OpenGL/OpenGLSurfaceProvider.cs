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
    private GRContext? _gRContext;
    private GRGlFramebufferInfo? _gRGlFramebufferInfo;
    private GRBackendRenderTarget? _gRBackendRenderTarget;
    private Vector2D<int> _framebufferSize;

    private SKSurface? _surface;

    public SKSurface Begin()
    {
        if (_surface is not null && window.FramebufferSize == _framebufferSize)
            return _surface;

        _framebufferSize = window.FramebufferSize;

        _gl ??= window.CreateOpenGL();
        Debug.Assert(_gl is not null);

        if (_gRContext is not { IsAbandoned: false })
        {
            if (s_globalContext is not { IsAbandoned: false })
                s_globalContext = GRContext.CreateGl();
            if (s_globalContext is not { IsAbandoned: false })
                s_globalContext = GRContext.CreateGl(GRGlInterface.Create());

            _gRContext = s_globalContext;
            if (_gRContext is not { IsAbandoned: false })
                _gRContext = GRContext.CreateGl(GRGlInterface.CreateOpenGl(name => _gl.Context.GetProcAddress(name)));
        }
        Debug.Assert(_gRContext is not null);

        // 获取 GL 的 framebuffer 信息
        _gRGlFramebufferInfo ??= new(
            0,
            SKColorType.Rgba8888.ToGlSizedFormat());

        // 创建渲染目标描述
        _gRBackendRenderTarget = new(
            _framebufferSize.X,
            _framebufferSize.Y,
            0,
            8,
            _gRGlFramebufferInfo.Value
        );

        // 创建 Skia 画布
        _surface = SKSurface.Create(
            _gRContext,
            _gRBackendRenderTarget,
            GRSurfaceOrigin.BottomLeft,
            SKColorType.Rgba8888);
        Debug.Assert(_surface is not null);

        return _surface;
    }

    public void End()
    {
        if (!window.ShouldSwapAutomatically)
            window.SwapBuffers();
    }
}