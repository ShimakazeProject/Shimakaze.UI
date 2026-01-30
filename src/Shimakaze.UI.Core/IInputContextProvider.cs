using Silk.NET.Input;
using Silk.NET.Windowing;


namespace Shimakaze.UI.Core;

public interface IInputContextProvider
{
    IInputContext CreateInputContext(IWindow window);
}