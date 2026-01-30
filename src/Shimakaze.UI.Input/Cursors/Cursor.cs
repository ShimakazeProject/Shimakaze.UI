using Silk.NET.Input;

namespace Shimakaze.UI.Input.Cursors;

public abstract class Cursor
{
    internal Cursor()
    {
    }

    internal abstract Task Apply(ICursor cursor, CancellationToken cancellationToken);

    public static Cursor Custom(params IEnumerable<CursorFrame> frames) => new CustomCursor(frames);
    public static Cursor None => field ??= new StandardCursor(null);
    public static Cursor Default => field ??= new StandardCursor(Silk.NET.Input.StandardCursor.Default);
    public static Cursor Arrow => field ??= new StandardCursor(Silk.NET.Input.StandardCursor.Arrow);
    public static Cursor IBeam => field ??= new StandardCursor(Silk.NET.Input.StandardCursor.IBeam);
    public static Cursor Crosshair => field ??= new StandardCursor(Silk.NET.Input.StandardCursor.Crosshair);
    public static Cursor Hand => field ??= new StandardCursor(Silk.NET.Input.StandardCursor.Hand);
    public static Cursor HResize => field ??= new StandardCursor(Silk.NET.Input.StandardCursor.HResize);
    public static Cursor VResize => field ??= new StandardCursor(Silk.NET.Input.StandardCursor.VResize);
    public static Cursor NwseResize => field ??= new StandardCursor(Silk.NET.Input.StandardCursor.NwseResize);
    public static Cursor NeswResize => field ??= new StandardCursor(Silk.NET.Input.StandardCursor.NeswResize);
    public static Cursor ResizeAll => field ??= new StandardCursor(Silk.NET.Input.StandardCursor.ResizeAll);
    public static Cursor NotAllowed => field ??= new StandardCursor(Silk.NET.Input.StandardCursor.NotAllowed);
    public static Cursor Wait => field ??= new StandardCursor(Silk.NET.Input.StandardCursor.Wait);
    public static Cursor WaitArrow => field ??= new StandardCursor(Silk.NET.Input.StandardCursor.WaitArrow);
}