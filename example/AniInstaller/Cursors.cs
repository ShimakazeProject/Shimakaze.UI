using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using System.Text;

using Shimakaze.UI.Core;
using Shimakaze.UI.Core.Dispatchers;

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
    public CursorsFrame GetFrame(int frame) => new()
    {
        Arrow = Arrow?[frame % Arrow.Count],
        Help = Help?[frame % Help.Count],
        AppStarting = AppStarting?[frame % AppStarting.Count],
        Wait = Wait?[frame % Wait.Count],
        Crosshair = Crosshair?[frame % Crosshair.Count],
        IBeam = IBeam?[frame % IBeam.Count],
        NWPen = NWPen?[frame % NWPen.Count],
        No = No?[frame % No.Count],
        SizeNS = SizeNS?[frame % SizeNS.Count],
        SizeWE = SizeWE?[frame % SizeWE.Count],
        SizeNWSE = SizeNWSE?[frame % SizeNWSE.Count],
        SizeNESW = SizeNESW?[frame % SizeNESW.Count],
        SizeAll = SizeAll?[frame % SizeAll.Count],
        UpArrow = UpArrow?[frame % UpArrow.Count],
        Hand = Hand?[frame % Hand.Count],
    };

    [SupportedOSPlatform("windows")]
    private void InstallWindows()
    {
        var target = Path.Combine(Path.GetTempPath(), "Shimakaze.AnimatedCursor.Installer");
        if (Directory.Exists(target))
            Directory.Delete(target, true);
        Directory.CreateDirectory(target);
        var infPath = Path.Combine(target, "install.inf");

        using StringWriter sourceDisksFiles = new();
        using StringWriter applyCursorScheme = new();
        using StringWriter registerCursorScheme = new();
        using StringWriter cursors = new();


        if (!string.IsNullOrWhiteSpace(ArrowName) && File.Exists(ArrowPath))
        {
            File.Copy(ArrowPath, Path.Combine(target, ArrowName), true);
            sourceDisksFiles.WriteLine($"{ArrowName} = 1");
            cursors.WriteLine(ArrowName);
            applyCursorScheme.WriteLine($"HKCU, \"Control Panel\\Cursors\", \"Arrow\", 0x00020000, \"%SystemRoot%\\cursors\\{Name}\\{ArrowName}\"");
            registerCursorScheme.Write($"\"%SystemRoot%\\cursors\\{Name}\\{ArrowName}\", ");
        }
        if (!string.IsNullOrWhiteSpace(HelpName) && File.Exists(HelpPath))
        {
            File.Copy(HelpPath, Path.Combine(target, HelpName), true);
            sourceDisksFiles.WriteLine($"{HelpName} = 1");
            cursors.WriteLine(HelpName);
            applyCursorScheme.WriteLine($"HKCU, \"Control Panel\\Cursors\", \"IBeam\", 0x00020000, \"%SystemRoot%\\cursors\\{Name}\\{IBeamName}\"");
            registerCursorScheme.Write($"\"%SystemRoot%\\cursors\\{Name}\\{HelpName}\", ");
        }
        if (!string.IsNullOrWhiteSpace(AppStartingName) && File.Exists(AppStartingPath))
        {
            File.Copy(AppStartingPath, Path.Combine(target, AppStartingName), true);
            sourceDisksFiles.WriteLine($"{AppStartingName} = 1");
            cursors.WriteLine(AppStartingName);
            applyCursorScheme.WriteLine($"HKCU, \"Control Panel\\Cursors\", \"Wait\", 0x00020000, \"%SystemRoot%\\cursors\\{Name}\\{WaitName}\"");
            registerCursorScheme.Write($"\"%SystemRoot%\\cursors\\{Name}\\{AppStartingName}\", ");
        }
        if (!string.IsNullOrWhiteSpace(WaitName) && File.Exists(WaitPath))
        {
            File.Copy(WaitPath, Path.Combine(target, WaitName), true);
            sourceDisksFiles.WriteLine($"{WaitName} = 1");
            cursors.WriteLine(WaitName);
            applyCursorScheme.WriteLine($"HKCU, \"Control Panel\\Cursors\", \"Crosshair\", 0x00020000, \"%SystemRoot%\\cursors\\{Name}\\{CrosshairName}\"");
            registerCursorScheme.Write($"\"%SystemRoot%\\cursors\\{Name}\\{WaitName}\", ");
        }
        if (!string.IsNullOrWhiteSpace(CrosshairName) && File.Exists(CrosshairPath))
        {
            File.Copy(CrosshairPath, Path.Combine(target, CrosshairName), true);
            sourceDisksFiles.WriteLine($"{CrosshairName} = 1");
            cursors.WriteLine(CrosshairName);
            applyCursorScheme.WriteLine($"HKCU, \"Control Panel\\Cursors\", \"UpArrow\", 0x00020000, \"%SystemRoot%\\cursors\\{Name}\\{UpArrowName}\"");
            registerCursorScheme.Write($"\"%SystemRoot%\\cursors\\{Name}\\{CrosshairName}\", ");
        }
        if (!string.IsNullOrWhiteSpace(IBeamName) && File.Exists(IBeamPath))
        {
            File.Copy(IBeamPath, Path.Combine(target, IBeamName), true);
            sourceDisksFiles.WriteLine($"{IBeamName} = 1");
            cursors.WriteLine(IBeamName);
            applyCursorScheme.WriteLine($"HKCU, \"Control Panel\\Cursors\", \"SizeNS\", 0x00020000, \"%SystemRoot%\\cursors\\{Name}\\{SizeNSName}\"");
            registerCursorScheme.Write($"\"%SystemRoot%\\cursors\\{Name}\\{IBeamName}\", ");
        }
        if (!string.IsNullOrWhiteSpace(NWPenName) && File.Exists(NWPenPath))
        {
            File.Copy(NWPenPath, Path.Combine(target, NWPenName), true);
            sourceDisksFiles.WriteLine($"{NWPenName} = 1");
            cursors.WriteLine(NWPenName);
            applyCursorScheme.WriteLine($"HKCU, \"Control Panel\\Cursors\", \"SizeWE\", 0x00020000, \"%SystemRoot%\\cursors\\{Name}\\{SizeWEName}\"");
            registerCursorScheme.Write($"\"%SystemRoot%\\cursors\\{Name}\\{NWPenName}\", ");
        }
        if (!string.IsNullOrWhiteSpace(NoName) && File.Exists(NoPath))
        {
            File.Copy(NoPath, Path.Combine(target, NoName), true);
            sourceDisksFiles.WriteLine($"{NoName} = 1");
            cursors.WriteLine(NoName);
            applyCursorScheme.WriteLine($"HKCU, \"Control Panel\\Cursors\", \"SizeNWSE\", 0x00020000, \"%SystemRoot%\\cursors\\{Name}\\{SizeNWSEName}\"");
            registerCursorScheme.Write($"\"%SystemRoot%\\cursors\\{Name}\\{NoName}\", ");
        }
        if (!string.IsNullOrWhiteSpace(SizeNSName) && File.Exists(SizeNSPath))
        {
            File.Copy(SizeNSPath, Path.Combine(target, SizeNSName), true);
            sourceDisksFiles.WriteLine($"{SizeNSName} = 1");
            cursors.WriteLine(SizeNSName);
            applyCursorScheme.WriteLine($"HKCU, \"Control Panel\\Cursors\", \"SizeNESW\", 0x00020000, \"%SystemRoot%\\cursors\\{Name}\\{SizeNESWName}\"");
            registerCursorScheme.Write($"\"%SystemRoot%\\cursors\\{Name}\\{SizeNSName}\", ");
        }
        if (!string.IsNullOrWhiteSpace(SizeWEName) && File.Exists(SizeWEPath))
        {
            File.Copy(SizeWEPath, Path.Combine(target, SizeWEName), true);
            sourceDisksFiles.WriteLine($"{SizeWEName} = 1");
            cursors.WriteLine(SizeWEName);
            applyCursorScheme.WriteLine($"HKCU, \"Control Panel\\Cursors\", \"SizeAll\", 0x00020000, \"%SystemRoot%\\cursors\\{Name}\\{SizeAllName}\"");
            registerCursorScheme.Write($"\"%SystemRoot%\\cursors\\{Name}\\{SizeWEName}\", ");
        }
        if (!string.IsNullOrWhiteSpace(SizeNWSEName) && File.Exists(SizeNWSEPath))
        {
            File.Copy(SizeNWSEPath, Path.Combine(target, SizeNWSEName), true);
            sourceDisksFiles.WriteLine($"{SizeNWSEName} = 1");
            cursors.WriteLine(SizeNWSEName);
            applyCursorScheme.WriteLine($"HKCU, \"Control Panel\\Cursors\", \"No\", 0x00020000, \"%SystemRoot%\\cursors\\{Name}\\{NoName}\"");
            registerCursorScheme.Write($"\"%SystemRoot%\\cursors\\{Name}\\{SizeNWSEName}\", ");
        }
        if (!string.IsNullOrWhiteSpace(SizeNESWName) && File.Exists(SizeNESWPath))
        {
            File.Copy(SizeNESWPath, Path.Combine(target, SizeNESWName), true);
            sourceDisksFiles.WriteLine($"{SizeNESWName} = 1");
            cursors.WriteLine(SizeNESWName);
            applyCursorScheme.WriteLine($"HKCU, \"Control Panel\\Cursors\", \"Hand\", 0x00020000, \"%SystemRoot%\\cursors\\{Name}\\{HandName}\"");
            registerCursorScheme.Write($"\"%SystemRoot%\\cursors\\{Name}\\{SizeNESWName}\", ");
        }
        if (!string.IsNullOrWhiteSpace(SizeAllName) && File.Exists(SizeAllPath))
        {
            File.Copy(SizeAllPath, Path.Combine(target, SizeAllName), true);
            sourceDisksFiles.WriteLine($"{SizeAllName} = 1");
            cursors.WriteLine(SizeAllName);
            applyCursorScheme.WriteLine($"HKCU, \"Control Panel\\Cursors\", \"AppStarting\", 0x00020000, \"%SystemRoot%\\cursors\\{Name}\\{AppStartingName}\"");
            registerCursorScheme.Write($"\"%SystemRoot%\\cursors\\{Name}\\{SizeAllName}\", ");
        }
        if (!string.IsNullOrWhiteSpace(UpArrowName) && File.Exists(UpArrowPath))
        {
            File.Copy(UpArrowPath, Path.Combine(target, UpArrowName), true);
            sourceDisksFiles.WriteLine($"{UpArrowName} = 1");
            cursors.WriteLine(UpArrowName);
            applyCursorScheme.WriteLine($"HKCU, \"Control Panel\\Cursors\", \"Help\", 0x00020000, \"%SystemRoot%\\cursors\\{Name}\\{HelpName}\"");
            registerCursorScheme.Write($"\"%SystemRoot%\\cursors\\{Name}\\{UpArrowName}\", ");
        }
        if (!string.IsNullOrWhiteSpace(HandName) && File.Exists(HandPath))
        {
            File.Copy(HandPath, Path.Combine(target, HandName), true);
            sourceDisksFiles.WriteLine($"{HandName} = 1");
            cursors.WriteLine(HandName);
            applyCursorScheme.WriteLine($"HKCU, \"Control Panel\\Cursors\", \"NWPen\", 0x00020000, \"%SystemRoot%\\cursors\\{Name}\\{NWPenName}\"");
            registerCursorScheme.Write($"\"%SystemRoot%\\cursors\\{Name}\\{HandName}\", ");
        }

        // 必须以 UTF16-LE 保存此文件
        using (var fs = File.Create(infPath))
        using (StreamWriter writer = new(fs, Encoding.Unicode))
        {
            writer.WriteLine($$"""
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

            [DestinationDirs]
            Cursors = 10,"Cursors\{{Name}}"

            [SourceDisksFiles]
            {{sourceDisksFiles}}

            [ApplyCursorScheme]
            HKCU, "Control Panel\Cursors", "Scheme Source",0x10001, 2
            {{applyCursorScheme}}

            [RegisterCursorScheme]
            HKLM, "SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Cursors\Schemes", "{{Name}}", 0x00020000, "{{registerCursorScheme}},"
        
            [Cursor]
            {{cursors}}
            
            [Strings]
            ThemeName="{{Name}}"
            """);
            writer.Flush();
        }

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

    public IEnumerable<SKBitmap> GetAllBitmaps() => GetAllFrames().SelectMany(static i => i);

    private IEnumerable<IEnumerable<SKBitmap>> GetAllFrames()
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