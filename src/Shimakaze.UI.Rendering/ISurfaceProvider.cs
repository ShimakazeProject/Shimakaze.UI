using System.Diagnostics.CodeAnalysis;

using SkiaSharp;

namespace Shimakaze.UI.Rendering;

public interface ISurfaceProvider
{
    SKSurface? Surface { get; }

    [MemberNotNullWhen(false, nameof(Surface))]
    bool IsInvalid();

    [MemberNotNull(nameof(Surface))]
    void EnsureCreated();
}