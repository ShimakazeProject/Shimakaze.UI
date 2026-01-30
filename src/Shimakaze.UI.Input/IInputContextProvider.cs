using Silk.NET.Input;
using Silk.NET.Windowing;


namespace Shimakaze.UI.Input;

public interface IInputContextProvider
{
    IInputContext CreateInputContext(IWindow window);
}