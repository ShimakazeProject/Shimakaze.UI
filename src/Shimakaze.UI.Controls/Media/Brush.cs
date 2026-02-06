using System.Drawing;

using Shimakaze.UI.Rendering;

namespace Shimakaze.UI.Media;

public abstract class Brush
{
    protected internal abstract void OnRender(Renderer renderer, params IEnumerable<RectangleF> bounds);
}