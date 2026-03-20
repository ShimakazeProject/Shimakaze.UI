using Silk.NET.Windowing;

namespace Shimakaze.UI.Core;

public interface IPlatformWindowProvider
{
    IWindow CreateWindow(WindowOptions options);
}