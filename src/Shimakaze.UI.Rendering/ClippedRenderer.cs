using System.Drawing;

using Shimakaze.UI.Rendering.Extensions;

namespace Shimakaze.UI.Rendering;

public class ClippedRenderer : Renderer, IDisposable
{
    public ClippedRenderer(Renderer renderer, RectangleF clip)
        : base(renderer.Surface)
    {
        Canvas.Save();
        Canvas.ClipRect(renderer.FixPosition(clip).ToSkia());
    }

    public override void Dispose()
    {
        Canvas.Restore();
    }
}