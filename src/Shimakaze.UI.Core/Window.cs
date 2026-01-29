using Silk.NET.Windowing;


namespace Shimakaze.UI.Core;

public class Window
{
    internal IWindow Native { get; set; } = null!;


    public Window()
    {
        Application.Instance.WindowManager.ApplyWindow(this);
    }

}