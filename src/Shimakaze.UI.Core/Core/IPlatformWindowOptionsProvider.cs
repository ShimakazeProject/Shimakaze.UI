using Silk.NET.Windowing;

namespace Shimakaze.UI.Core;

public interface IPlatformWindowOptionsProvider
{
    WindowOptions CreateOptions();
}