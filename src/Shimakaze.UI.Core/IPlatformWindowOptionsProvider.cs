using Silk.NET.Windowing;


namespace Shimakaze.UI.Core;

public interface PlatformWindowOptionsProvider
{
    WindowOptions CreateOptions();
}