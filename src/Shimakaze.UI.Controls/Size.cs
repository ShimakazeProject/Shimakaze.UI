using System.Numerics;

using Silk.NET.Maths;

using SkiaSharp;

namespace Shimakaze.UI;

public readonly record struct Size(double Width, double Height)
{
    public Size(double uniform)
        : this(uniform, uniform)
    { }

    public static implicit operator Size(SKSize size) => new(size.Width, size.Height);
    public static implicit operator Size(System.Drawing.Size size) => new(size.Width, size.Height);
    public static implicit operator Size(Vector2D<float> vector) => new(vector.X, vector.Y);
    public static implicit operator Size(Vector2D<double> vector) => new(vector.X, vector.Y);
    public static implicit operator Size(Vector2 vector) => new(vector.X, vector.Y);

    public static implicit operator SKSize(Size size) => new((float)size.Width, (float)size.Height);
    public static implicit operator System.Drawing.Size(Size size) => new((int)size.Width, (int)size.Height);
    public static implicit operator Vector2D<float>(Size size) => new((float)size.Width, (float)size.Height);
    public static implicit operator Vector2D<double>(Size size) => new(size.Width, size.Height);
    public static implicit operator Vector2(Size size) => new((float)size.Width, (float)size.Height);
}