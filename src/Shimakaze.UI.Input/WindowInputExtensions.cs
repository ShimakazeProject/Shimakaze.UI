using Shimakaze.UI.Core;

namespace Shimakaze.UI.Input;

public static class WindowExtensions
{
    extension(Window window)
    {
        public InputManager Input => InputManager.Get(window);
    }

}