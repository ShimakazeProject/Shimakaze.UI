using Silk.NET.Core;
using Silk.NET.Maths;

namespace Shimakaze.UI.Input.Cursors;

public record class JiffiesCursorFrame(Vector2D<int> Hotspot, RawImage Image, int Jiffies) : CursorFrame(Hotspot, Image);