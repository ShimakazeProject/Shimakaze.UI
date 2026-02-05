using System.Drawing;
using System.Numerics;

using Shimakaze.UI.Rendering.Extensions;

namespace Shimakaze.UI.Rendering;

public sealed class RotatedRenderer : Renderer, IDisposable
{
    public Renderer Renderer { get; }
    public PointF Origin { get; }
    public float Radians { get; }
    private Vector2 _origin;

    public RotatedRenderer(Renderer renderer, PointF origin, float radians)
        : base(renderer.Surface)
    {
        Renderer = renderer;
        Origin = origin;
        Radians = radians;
        _origin = Origin.ToVector2();

        Canvas.Save();
        Canvas.Translate(Origin.ToSkia());
        Canvas.RotateRadians(Radians);
    }

    /// <summary>
    /// 将角度转换为弧度：弧度 = 角度 × π / 180
    /// </summary>
    /// <param name="degrees"></param>
    /// <returns></returns>
    public static float GetRadians(float degrees)
        => degrees * (float)Math.PI / 180f;

    /// <summary>
    /// 获取真实画布坐标
    /// </summary>
    /// <remarks>
    /// 绕指定旋转中心旋转坐标点
    /// </remarks>
    /// <param name="point">原始坐标</param>
    /// <returns>旋转后的坐标</returns>
    public override Vector2 FixPosition(Vector2 point)
    {
        point = Renderer.FixPosition(point);
        point -= _origin;

        // 旋转公式：
        // x' = x * cos(θ) - y * sin(θ)
        // y' = x * sin(θ) + y * cos(θ)
        float cosTheta = MathF.Cos(-Radians);
        float sinTheta = MathF.Sin(-Radians);

        point = new(
            point.X * cosTheta - point.Y * sinTheta,
            point.X * sinTheta + point.Y * cosTheta);

        point += _origin;

        return point;
    }

    public override void Dispose()
    {
        Canvas.RotateRadians(-Radians);
        Canvas.Translate(-Origin.X, -Origin.Y);
    }
}