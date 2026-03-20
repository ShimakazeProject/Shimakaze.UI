using System.Drawing;
using System.Numerics;

using Shimakaze.UI.Rendering.Extensions;

namespace Shimakaze.UI.Rendering;

public sealed class ClippedRenderer : Renderer, IDisposable
{
    private readonly RectangleF _clip;

    public ClippedRenderer(Renderer renderer, RectangleF clip)
        : base(renderer.Surface)
    {
        _clip = clip;
        Canvas.Save();
        Canvas.ClipRect(renderer.FixPosition(clip).ToSkia());
    }

    public override Vector2 FixPosition(Vector2 point)
        => base.FixPosition(point) + _clip.Location.ToVector2();

    public override void Dispose()
    {
        Canvas.Restore();
    }
}