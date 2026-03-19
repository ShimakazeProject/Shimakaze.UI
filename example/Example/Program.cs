using System.Drawing;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Shimakaze.UI;
using Shimakaze.UI.Core;
using Shimakaze.UI.Rendering;
using Shimakaze.UI.Rendering.Extensions;

var builder = Host.CreateShimakazeUIApplicationBuilder(args);
builder.Services.UseGlfw();
builder.Services.UseOpenGL();
builder.Services.AddWindow<MainWindow>();

var app = builder.Build();

await app.RunAsync();

sealed class MainWindow : Window
{
    public MainWindow()
    {
        ColorPanel element = new();
        Content = element;
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        IPlatformWindowWrap wrap = this;
        PlatformWindow platformWindow = wrap.PlatformWindow;
        Console.WriteLine(platformWindow.Native.Native?.Kind);
    }
}

sealed class ColorPanel : UIElement
{
    private int _h;
    protected override void OnRender(Renderer renderer, double deltaTime)
    {
        renderer.Clear(Color.FromHsv(_h, 100, 100));
        _h++;
        if (_h > 360)
            _h = 0;
    }
}