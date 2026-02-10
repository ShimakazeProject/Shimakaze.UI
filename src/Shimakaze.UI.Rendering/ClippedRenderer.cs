using System.Drawing;

using Shimakaze.UI.Rendering.Extensions;

namespace Shimakaze.UI.Rendering;

public sealed class ClippedRenderer : Renderer, IDisposable
{
    public ClippedRenderer(Renderer renderer, RectangleF clip)
        : base(renderer.Surface)
    {
        Canvas.Save();
        Canvas.ClipRect(FixPosition(clip).ToSkia());
    }

    public override void Dispose()
    {
        Canvas.Restore();
    }
}