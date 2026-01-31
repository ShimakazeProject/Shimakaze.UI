using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

using SkiaSharp;

namespace Shimakaze.UI.Rendering.OpenGL;

internal sealed class OpenGLRendererContext
{
    public GL? GL { get; set; }
    public GRContext? GRContext { get; set; }
    public GRGlFramebufferInfo? GRGlFramebufferInfo { get; set; }
    public GRBackendRenderTarget? GRBackendRenderTarget { get; set; }
    public SKSurface? Surface { get; set; }
    public Vector2D<int> Size { get; set; }

    [MemberNotNullWhen(
        false,
        nameof(GL),
        nameof(GRContext),
        nameof(GRGlFramebufferInfo),
        nameof(GRBackendRenderTarget),
        nameof(Surface))]
    public bool ShouldRecreate(IWindow window) => window.Size != Size;

    [MemberNotNull(
        nameof(GL),
        nameof(GRContext),
        nameof(GRGlFramebufferInfo),
        nameof(GRBackendRenderTarget),
        nameof(Surface))]
    public void Create(IWindow window, ref GRContext? globalContext)
    {
        Size = window.Size;

        GL ??= window.CreateOpenGL();
        Debug.Assert(GL is not null);

        if (GRContext is null)
        {
            globalContext ??= GRContext.CreateGl();
            globalContext ??= GRContext.CreateGl(GRGlInterface.Create());
            GRContext = globalContext;
            GRContext ??= GRContext.CreateGl(
                GRGlInterface.CreateOpenGl(name => GL.Context.GetProcAddress(name)));
        }
        Debug.Assert(GRContext is not null);

        // 获取 GL 的 framebuffer 信息
        GRGlFramebufferInfo ??= new(
            0,
            SKColorType.Rgba8888.ToGlSizedFormat());

        // 创建渲染目标描述
        GRBackendRenderTarget = new(
            Size.X,
            Size.Y,
            0,
            8,
            GRGlFramebufferInfo.Value
        );

        // 创建 Skia 画布
        Surface = SKSurface.Create(
            GRContext,
            GRBackendRenderTarget,
            GRSurfaceOrigin.BottomLeft,
            SKColorType.Rgba8888);
        Debug.Assert(Surface is not null);
    }
}