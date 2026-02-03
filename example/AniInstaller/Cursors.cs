using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using System.Text;

using SkiaSharp;

using Windows.Win32;
using Windows.Win32.UI.WindowsAndMessaging;

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

        // 必须以 UTF16-LE 保存此文件
        var inf = $$"""
        ; Generated by Shimakaze.AnimatedCursor.Installer
        ;     ❤ from frg2089

        [Version]
        Signature="$Windows NT$"
        Provider=%ThemeName%
        DriverVer={{DateTimeOffset.Now:yyyy/MM/dd}},1.0.0.0

        [DefaultInstall]
        CopyFiles=Cursors
        AddReg=RegisterCursorScheme, ApplyCursorScheme

        [SourceDisksNames]
        1 = %ThemeName%

        [SourceDisksFiles]
        {{ArrowName}} = 1
        {{HelpName}} = 1
        {{AppStartingName}} = 1
        {{WaitName}} = 1
        {{CrosshairName}} = 1
        {{IBeamName}} = 1
        {{NWPenName}} = 1
        {{NoName}} = 1
        {{SizeNSName}} = 1
        {{SizeWEName}} = 1
        {{SizeNWSEName}} = 1
        {{SizeNESWName}} = 1
        {{SizeAllName}} = 1
        {{UpArrowName}} = 1
        {{HandName}} = 1

        [DestinationDirs]
        Cursors = 10,"Cursors\{{Name}}"

        [Cursors]
        {{ArrowName}}
        {{HelpName}}
        {{AppStartingName}}
        {{WaitName}}
        {{CrosshairName}}
        {{IBeamName}}
        {{NWPenName}}
        {{NoName}}
        {{SizeNSName}}
        {{SizeWEName}}
        {{SizeNWSEName}}
        {{SizeNESWName}}
        {{SizeAllName}}
        {{UpArrowName}}
        {{HandName}}

        [RegisterCursorScheme]
        HKLM,"SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Cursors\Schemes","{{Name}}",0x00020000,"%SystemRoot%\cursors\{{Name}}\{{ArrowName}},%SystemRoot%\cursors\{{Name}}\{{HelpName}},%SystemRoot%\cursors\{{Name}}\{{AppStartingName}},%SystemRoot%\cursors\{{Name}}\{{WaitName}},%SystemRoot%\cursors\{{Name}}\{{CrosshairName}},%SystemRoot%\cursors\{{Name}}\{{IBeamName}},%SystemRoot%\cursors\{{Name}}\{{NWPenName}},%SystemRoot%\cursors\{{Name}}\{{NoName}},%SystemRoot%\cursors\{{Name}}\{{SizeNSName}},%SystemRoot%\cursors\{{Name}}\{{SizeWEName}},%SystemRoot%\cursors\{{Name}}\{{SizeNWSEName}},%SystemRoot%\cursors\{{Name}}\{{SizeNESWName}},%SystemRoot%\cursors\{{Name}}\{{SizeAllName}},%SystemRoot%\cursors\{{Name}}\{{UpArrowName}},%SystemRoot%\cursors\{{Name}}\{{HandName}},,"

        [ApplyCursorScheme]
        HKCU,"Control Panel\Cursors","Arrow",0x00020000,"%SystemRoot%\cursors\{{Name}}\{{ArrowName}}"
        HKCU,"Control Panel\Cursors","IBeam",0x00020000,"%SystemRoot%\cursors\{{Name}}\{{IBeamName}}"
        HKCU,"Control Panel\Cursors","Wait",0x00020000,"%SystemRoot%\cursors\{{Name}}\{{WaitName}}"
        HKCU,"Control Panel\Cursors","Crosshair",0x00020000,"%SystemRoot%\cursors\{{Name}}\{{CrosshairName}}"
        HKCU,"Control Panel\Cursors","UpArrow",0x00020000,"%SystemRoot%\cursors\{{Name}}\{{UpArrowName}}"
        HKCU,"Control Panel\Cursors","SizeNS",0x00020000,"%SystemRoot%\cursors\{{Name}}\{{SizeNSName}}"
        HKCU,"Control Panel\Cursors","SizeWE",0x00020000,"%SystemRoot%\cursors\{{Name}}\{{SizeWEName}}"
        HKCU,"Control Panel\Cursors","SizeNWSE",0x00020000,"%SystemRoot%\cursors\{{Name}}\{{SizeNWSEName}}"
        HKCU,"Control Panel\Cursors","SizeNESW",0x00020000,"%SystemRoot%\cursors\{{Name}}\{{SizeNESWName}}"
        HKCU,"Control Panel\Cursors","SizeAll",0x00020000,"%SystemRoot%\cursors\{{Name}}\{{SizeAllName}}"
        HKCU,"Control Panel\Cursors","No",0x00020000,"%SystemRoot%\cursors\{{Name}}\{{NoName}}"
        HKCU,"Control Panel\Cursors","Hand",0x00020000,"%SystemRoot%\cursors\{{Name}}\{{HandName}}"
        HKCU,"Control Panel\Cursors","AppStarting",0x00020000,"%SystemRoot%\cursors\{{Name}}\{{AppStartingName}}"
        HKCU,"Control Panel\Cursors","Help",0x00020000,"%SystemRoot%\cursors\{{Name}}\{{HelpName}}"
        HKCU,"Control Panel\Cursors","NWPen",0x00020000,"%SystemRoot%\cursors\{{Name}}\{{NWPenName}}"
        HKCU,"Control Panel\Cursors","Scheme Source",0x10001,2

        [Strings]
        ThemeName="{{Name}}"
        """;

        var target = Path.Combine(Path.GetTempPath(), "Shimakaze.AnimatedCursor.Installer");
        if (Directory.Exists(target))
            Directory.Delete(target, true);
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

        var infPath = Path.Combine(target, "install.inf");
        File.WriteAllText(infPath, inf, Encoding.Unicode);

        var proc = new Process()
        {
            StartInfo =
            {
                FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "InfDefaultInstall.exe") ,
                Arguments = infPath,
                UseShellExecute = true,
            }
        };
        proc.Start();
        proc.WaitForExit();

        if (OperatingSystem.IsWindowsVersionAtLeast(5))
            PInvoke.SystemParametersInfo(SYSTEM_PARAMETERS_INFO_ACTION.SPI_SETCURSORS, 0, SYSTEM_PARAMETERS_INFO_UPDATE_FLAGS.SPIF_UPDATEINIFILE | SYSTEM_PARAMETERS_INFO_UPDATE_FLAGS.SPIF_SENDCHANGE);

        Directory.Delete(target, true);
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

    public void Install()
    {
        if (OperatingSystem.IsWindows())
        {
            InstallWindows();
        }
    }
}