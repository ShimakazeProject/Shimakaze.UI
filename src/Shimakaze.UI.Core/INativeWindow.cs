using Silk.NET.Windowing;


namespace Shimakaze.UI.Core;

public interface INativeWindow : IDisposable
{
    IWindow Native { get; }
}