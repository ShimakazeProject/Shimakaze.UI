using Silk.NET.Windowing;


namespace Shimakaze.UI.Core;

public interface IWindowProvider
{
    IWindow CreateWindow(WindowOptions options);
}