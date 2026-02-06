using System.Drawing;

using Shimakaze.UI.Rendering;
using Shimakaze.UI.Rendering.Extensions;

using SkiaSharp;

namespace Shimakaze.UI.Media;

public sealed class SolidColorBrush(Color color) : Brush
{
    public Color Color { get; } = color;
    private readonly SKPaint _paint = new()
    {
        Color = color.ToSkia()
    };

    protected internal override void OnRender(Renderer renderer, params IEnumerable<RectangleF> bounds)
    {
        foreach (var bound in bounds)
            renderer.Canvas.DrawRect(bound.ToSkia(), _paint);
    }
}
