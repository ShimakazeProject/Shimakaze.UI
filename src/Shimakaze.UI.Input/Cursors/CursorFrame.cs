using Silk.NET.Core;
using Silk.NET.Maths;

namespace Shimakaze.UI.Input.Cursors;

public record class CursorFrame(Vector2D<int> Hotspot, RawImage Image);