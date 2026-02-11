using Silk.NET.Windowing;


namespace Shimakaze.UI.Core;

public interface PlatformWindowProvider
{
    IWindow CreateWindow(WindowOptions options);
}