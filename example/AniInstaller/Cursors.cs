using System.Diagnostics;

using SkiaSharp;

sealed record class Cursors(string Name)
{
    public IReadOnlyList<SKBitmap>? Arrow { get; set; }
    public IReadOnlyList<SKBitmap>? Help { get; set; }
    public IReadOnlyList<SKBitmap>? AppStarting { get; set; }
    public IReadOnlyList<SKBitmap>? Wait { get; set; }
    public IReadOnlyList<SKBitmap>? Crosshair { get; set; }
    public IReadOnlyList<SKBitmap>? IBeam { get; set; }
    public IReadOnlyList<SKBitmap>? NWPen { get; set; }
    public IReadOnlyList<SKBitmap>? No { get; set; }
    public IReadOnlyList<SKBitmap>? SizeNS { get; set; }
    public IReadOnlyList<SKBitmap>? SizeWE { get; set; }
    public IReadOnlyList<SKBitmap>? SizeNWSE { get; set; }
    public IReadOnlyList<SKBitmap>? SizeNESW { get; set; }
    public IReadOnlyList<SKBitmap>? SizeAll { get; set; }
    public IReadOnlyList<SKBitmap>? UpArrow { get; set; }
    public IReadOnlyList<SKBitmap>? Hand { get; set; }

    public CursorsFrame GetFrame(int frame)
    {
        Debug.Assert(Arrow is not null);
        Debug.Assert(Help is not null);
        Debug.Assert(AppStarting is not null);
        Debug.Assert(Wait is not null);
        Debug.Assert(Crosshair is not null);
        Debug.Assert(IBeam is not null);
        Debug.Assert(NWPen is not null);
        Debug.Assert(No is not null);
        Debug.Assert(SizeNS is not null);
        Debug.Assert(SizeWE is not null);
        Debug.Assert(SizeNWSE is not null);
        Debug.Assert(SizeNESW is not null);
        Debug.Assert(SizeAll is not null);
        Debug.Assert(UpArrow is not null);
        Debug.Assert(Hand is not null);
        return new()
        {
            Arrow = Arrow[frame % Arrow.Count],
            Help = Help[frame % Help.Count],
            AppStarting = AppStarting[frame % AppStarting.Count],
            Wait = Wait[frame % Wait.Count],
            Crosshair = Crosshair[frame % Crosshair.Count],
            IBeam = IBeam[frame % IBeam.Count],
            NWPen = NWPen[frame % NWPen.Count],
            No = No[frame % No.Count],
            SizeNS = SizeNS[frame % SizeNS.Count],
            SizeWE = SizeWE[frame % SizeWE.Count],
            SizeNWSE = SizeNWSE[frame % SizeNWSE.Count],
            SizeNESW = SizeNESW[frame % SizeNESW.Count],
            SizeAll = SizeAll[frame % SizeAll.Count],
            UpArrow = UpArrow[frame % UpArrow.Count],
            Hand = Hand[frame % Hand.Count],
        };
    }
}
