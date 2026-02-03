using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;

using Microsoft.Win32;

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

    public string? ArrowPath
    {
        get => field;
        set
        {
            field = value;
            ArrowName = Path.GetFileName(value);
        }
    }
    public string? HelpPath
    {
        get => field;
        set
        {
            field = value;
            HelpName = Path.GetFileName(value);
        }
    }
    public string? AppStartingPath
    {
        get => field;
        set
        {
            field = value;
            AppStartingName = Path.GetFileName(value);
        }
    }
    public string? WaitPath
    {
        get => field;
        set
        {
            field = value;
            WaitName = Path.GetFileName(value);
        }
    }
    public string? CrosshairPath
    {
        get => field;
        set
        {
            field = value;
            CrosshairName = Path.GetFileName(value);
        }
    }
    public string? IBeamPath
    {
        get => field;
        set
        {
            field = value;
            IBeamName = Path.GetFileName(value);
        }
    }
    public string? NWPenPath
    {
        get => field;
        set
        {
            field = value;
            NWPenName = Path.GetFileName(value);
        }
    }
    public string? NoPath
    {
        get => field;
        set
        {
            field = value;
            NoName = Path.GetFileName(value);
        }
    }
    public string? SizeNSPath
    {
        get => field;
        set
        {
            field = value;
            SizeNSName = Path.GetFileName(value);
        }
    }
    public string? SizeWEPath
    {
        get => field;
        set
        {
            field = value;
            SizeWEName = Path.GetFileName(value);
        }
    }
    public string? SizeNWSEPath
    {
        get => field;
        set
        {
            field = value;
            SizeNWSEName = Path.GetFileName(value);
        }
    }
    public string? SizeNESWPath
    {
        get => field;
        set
        {
            field = value;
            SizeNESWName = Path.GetFileName(value);
        }
    }
    public string? SizeAllPath
    {
        get => field;
        set
        {
            field = value;
            SizeAllName = Path.GetFileName(value);
        }
    }
    public string? UpArrowPath
    {
        get => field;
        set
        {
            field = value;
            UpArrowName = Path.GetFileName(value);
        }
    }
    public string? HandPath
    {
        get => field;
        set
        {
            field = value;
            HandName = Path.GetFileName(value);
        }
    }

    public string? ArrowName { get; set; }
    public string? HelpName { get; set; }
    public string? AppStartingName { get; set; }
    public string? WaitName { get; set; }
    public string? CrosshairName { get; set; }
    public string? IBeamName { get; set; }
    public string? NWPenName { get; set; }
    public string? NoName { get; set; }
    public string? SizeNSName { get; set; }
    public string? SizeWEName { get; set; }
    public string? SizeNWSEName { get; set; }
    public string? SizeNESWName { get; set; }
    public string? SizeAllName { get; set; }
    public string? UpArrowName { get; set; }
    public string? HandName { get; set; }
    public CursorsFrame GetFrame(int frame)
    {
        AssertNull();
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

    [MemberNotNull(nameof(Arrow), nameof(ArrowPath), nameof(ArrowName))]
    [MemberNotNull(nameof(Help), nameof(HelpPath), nameof(HelpName))]
    [MemberNotNull(nameof(AppStarting), nameof(AppStartingPath), nameof(AppStartingName))]
    [MemberNotNull(nameof(Wait), nameof(WaitPath), nameof(WaitName))]
    [MemberNotNull(nameof(Crosshair), nameof(CrosshairPath), nameof(CrosshairName))]
    [MemberNotNull(nameof(IBeam), nameof(IBeamPath), nameof(IBeamName))]
    [MemberNotNull(nameof(NWPen), nameof(NWPenPath), nameof(NWPenName))]
    [MemberNotNull(nameof(No), nameof(NoPath), nameof(NoName))]
    [MemberNotNull(nameof(SizeNS), nameof(SizeNSPath), nameof(SizeNSName))]
    [MemberNotNull(nameof(SizeWE), nameof(SizeWEPath), nameof(SizeWEName))]
    [MemberNotNull(nameof(SizeNWSE), nameof(SizeNWSEPath), nameof(SizeNWSEName))]
    [MemberNotNull(nameof(SizeNESW), nameof(SizeNESWPath), nameof(SizeNESWName))]
    [MemberNotNull(nameof(SizeAll), nameof(SizeAllPath), nameof(SizeAllName))]
    [MemberNotNull(nameof(UpArrow), nameof(UpArrowPath), nameof(UpArrowName))]
    [MemberNotNull(nameof(Hand), nameof(HandPath), nameof(HandName))]
    private void AssertNull()
    {
        Debug.Assert(Arrow is not null);
        Debug.Assert(!string.IsNullOrWhiteSpace(ArrowPath));
        Debug.Assert(!string.IsNullOrWhiteSpace(ArrowName));
        Debug.Assert(Help is not null);
        Debug.Assert(!string.IsNullOrWhiteSpace(HelpPath));
        Debug.Assert(!string.IsNullOrWhiteSpace(HelpName));
        Debug.Assert(AppStarting is not null);
        Debug.Assert(!string.IsNullOrWhiteSpace(AppStartingPath));
        Debug.Assert(!string.IsNullOrWhiteSpace(AppStartingName));
        Debug.Assert(Wait is not null);
        Debug.Assert(!string.IsNullOrWhiteSpace(WaitPath));
        Debug.Assert(!string.IsNullOrWhiteSpace(WaitName));
        Debug.Assert(Crosshair is not null);
        Debug.Assert(!string.IsNullOrWhiteSpace(CrosshairPath));
        Debug.Assert(!string.IsNullOrWhiteSpace(CrosshairName));
        Debug.Assert(IBeam is not null);
        Debug.Assert(!string.IsNullOrWhiteSpace(IBeamPath));
        Debug.Assert(!string.IsNullOrWhiteSpace(IBeamName));
        Debug.Assert(NWPen is not null);
        Debug.Assert(!string.IsNullOrWhiteSpace(NWPenPath));
        Debug.Assert(!string.IsNullOrWhiteSpace(NWPenName));
        Debug.Assert(No is not null);
        Debug.Assert(!string.IsNullOrWhiteSpace(NoPath));
        Debug.Assert(!string.IsNullOrWhiteSpace(NoName));
        Debug.Assert(SizeNS is not null);
        Debug.Assert(!string.IsNullOrWhiteSpace(SizeNSPath));
        Debug.Assert(!string.IsNullOrWhiteSpace(SizeNSName));
        Debug.Assert(SizeWE is not null);
        Debug.Assert(!string.IsNullOrWhiteSpace(SizeWEPath));
        Debug.Assert(!string.IsNullOrWhiteSpace(SizeWEName));
        Debug.Assert(SizeNWSE is not null);
        Debug.Assert(!string.IsNullOrWhiteSpace(SizeNWSEPath));
        Debug.Assert(!string.IsNullOrWhiteSpace(SizeNWSEName));
        Debug.Assert(SizeNESW is not null);
        Debug.Assert(!string.IsNullOrWhiteSpace(SizeNESWPath));
        Debug.Assert(!string.IsNullOrWhiteSpace(SizeNESWName));
        Debug.Assert(SizeAll is not null);
        Debug.Assert(!string.IsNullOrWhiteSpace(SizeAllPath));
        Debug.Assert(!string.IsNullOrWhiteSpace(SizeAllName));
        Debug.Assert(UpArrow is not null);
        Debug.Assert(!string.IsNullOrWhiteSpace(UpArrowPath));
        Debug.Assert(!string.IsNullOrWhiteSpace(UpArrowName));
        Debug.Assert(Hand is not null);
        Debug.Assert(!string.IsNullOrWhiteSpace(HandPath));
        Debug.Assert(!string.IsNullOrWhiteSpace(HandName));
    }


    [SupportedOSPlatform("windows")]
    private void InstallWindows()
    {
        AssertNull();

        var target = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Cursors", Name);
        Directory.CreateDirectory(target);
        File.Copy(WaitPath, Path.Combine(target, WaitName), true);
        File.Copy(UpArrowPath, Path.Combine(target, UpArrowName), true);
        File.Copy(SizeWEPath, Path.Combine(target, SizeWEName), true);
        File.Copy(SizeNWSEPath, Path.Combine(target, SizeNWSEName), true);
        File.Copy(SizeNSPath, Path.Combine(target, SizeNSName), true);
        File.Copy(SizeNESWPath, Path.Combine(target, SizeNESWName), true);
        File.Copy(SizeAllPath, Path.Combine(target, SizeAllName), true);
        File.Copy(NWPenPath, Path.Combine(target, NWPenName), true);
        File.Copy(NoPath, Path.Combine(target, NoName), true);
        File.Copy(IBeamPath, Path.Combine(target, IBeamName), true);
        File.Copy(HelpPath, Path.Combine(target, HelpName), true);
        File.Copy(HandPath, Path.Combine(target, HandName), true);
        File.Copy(CrosshairPath, Path.Combine(target, CrosshairName), true);
        File.Copy(ArrowPath, Path.Combine(target, ArrowName), true);
        File.Copy(AppStartingPath, Path.Combine(target, AppStartingName), true);

        var value = string.Join(
            ",",
            [
                Path.Combine(target, ArrowName),
                Path.Combine(target, HelpName),
                Path.Combine(target, AppStartingName),
                Path.Combine(target, WaitName),
                Path.Combine(target, IBeamName),
                Path.Combine(target, CrosshairName),
                Path.Combine(target, NWPenName),
                Path.Combine(target, NoName),
                Path.Combine(target, SizeNSName),
                Path.Combine(target, SizeWEName),
                Path.Combine(target, SizeNWSEName),
                Path.Combine(target, SizeNESWName),
                Path.Combine(target, SizeAllName),
                Path.Combine(target, UpArrowName),
                Path.Combine(target, HandName),
            ]);

        using var basekey = Registry.CurrentUser.CreateSubKey(@"Control Panel\Cursors", true);

        basekey.SetValue(null, Name);
        basekey.SetValue("Arrow", Path.Combine(target, ArrowName));
        basekey.SetValue("Help", Path.Combine(target, HelpName));
        basekey.SetValue("AppStarting", Path.Combine(target, AppStartingName));
        basekey.SetValue("Wait", Path.Combine(target, WaitName));
        basekey.SetValue("Crosshair", Path.Combine(target, CrosshairName));
        basekey.SetValue("IBeam", Path.Combine(target, IBeamName));
        basekey.SetValue("NWPen", Path.Combine(target, NWPenName));
        basekey.SetValue("No", Path.Combine(target, NoName));
        basekey.SetValue("SizeNS", Path.Combine(target, SizeNSName));
        basekey.SetValue("SizeWE", Path.Combine(target, SizeWEName));
        basekey.SetValue("SizeNWSE", Path.Combine(target, SizeNWSEName));
        basekey.SetValue("SizeNESW", Path.Combine(target, SizeNESWName));
        basekey.SetValue("SizeAll", Path.Combine(target, SizeAllName));
        basekey.SetValue("UpArrow", Path.Combine(target, UpArrowName));
        basekey.SetValue("Hand", Path.Combine(target, HandName));

        using var schemes = basekey.CreateSubKey("Schemes", true);
        schemes.SetValue(Name, value);

        var proc = new Process();
        proc.StartInfo.FileName = "rundll32.exe";
        proc.StartInfo.Arguments = "shell32.dll,Control_RunDLL main.cpl @0";
        proc.Start();
    }

    public IEnumerable<IEnumerable<SKBitmap>> GetAllFrames()
    {
        if (Arrow is not null)
            yield return Arrow;
        if (Help is not null)
            yield return Help;
        if (AppStarting is not null)
            yield return AppStarting;
        if (Wait is not null)
            yield return Wait;
        if (Crosshair is not null)
            yield return Crosshair;
        if (IBeam is not null)
            yield return IBeam;
        if (NWPen is not null)
            yield return NWPen;
        if (No is not null)
            yield return No;
        if (SizeNS is not null)
            yield return SizeNS;
        if (SizeWE is not null)
            yield return SizeWE;
        if (SizeNWSE is not null)
            yield return SizeNWSE;
        if (SizeNESW is not null)
            yield return SizeNESW;
        if (SizeAll is not null)
            yield return SizeAll;
        if (UpArrow is not null)
            yield return UpArrow;
        if (Hand is not null)
            yield return Hand;
    }
}
