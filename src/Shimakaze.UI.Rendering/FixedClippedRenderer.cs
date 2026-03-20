using System.Drawing;
using System.Numerics;

namespace Shimakaze.UI.Rendering;

public sealed class FixedClippedRenderer(Renderer renderer, RectangleF clip) : ClippedRenderer(renderer, clip), IDisposable
{
    private readonly RectangleF _clip = clip;

    public override Vector2 FixPosition(Vector2 point)
        => base.FixPosition(point) + _clip.Location.ToVector2();
}