using System.Numerics;

using Silk.NET.Maths;

using SkiaSharp;

namespace Shimakaze.UI;

/// <summary>
/// 表示边距或内边距的四个方向的厚度。
/// </summary>
/// <remarks>
/// 初始化 <see cref="Thickness"/> 结构的新实例。
/// </remarks>
public readonly record struct Thickness(float Left, float Top, float Right, float Bottom) : IEquatable<Thickness>
{
    /// <summary>
    /// 初始化 <see cref="Thickness"/> 结构的新实例，四个方向使用相同的值。
    /// </summary>
    public Thickness(float uniformLength)
        : this(uniformLength, uniformLength, uniformLength, uniformLength)
    {
    }

    /// <summary>
    /// 初始化 <see cref="Thickness"/> 结构的新实例，水平方向和垂直方向使用不同的值。
    /// </summary>
    public Thickness(float horizontal, float vertical)
        : this(horizontal, vertical, horizontal, vertical)
    {
    }

    /// <summary>
    /// 获取水平方向的厚度总和（Left + Right）。
    /// </summary>
    public float Horizontal => Left + Right;

    /// <summary>
    /// 获取垂直方向的厚度总和（Top + Bottom）。
    /// </summary>
    public float Vertical => Top + Bottom;

    public static implicit operator Thickness(SKRect rect) => new(rect.Left, rect.Top, rect.Right, rect.Bottom);
    public static implicit operator Thickness(Vector4D<float> vector) => new(vector.X, vector.Y, vector.Z, vector.W);
    public static implicit operator Thickness(Vector4 vector) => new(vector.X, vector.Y, vector.Z, vector.W);

    public static implicit operator SKRect(Thickness thickness) => new((float)thickness.Left, (float)thickness.Top, (float)thickness.Right, (float)thickness.Bottom);
    public static implicit operator Vector4D<float>(Thickness thickness) => new((float)thickness.Left, (float)thickness.Top, (float)thickness.Right, (float)thickness.Bottom);
    public static implicit operator Vector4(Thickness thickness) => new((float)thickness.Left, (float)thickness.Top, (float)thickness.Right, (float)thickness.Bottom);
}