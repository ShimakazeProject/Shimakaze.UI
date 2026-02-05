using System.Drawing;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Shimakaze.UI.Core;
using Shimakaze.UI.Rendering;
using Shimakaze.UI.Rendering.Extensions;

var builder = Host.CreateShimakazeUIApplicationBuilder(args);
builder.Services.UseGlfw();
builder.Services.UseOpenGL();
builder.Services.AddWindow<MainWindow>();

var app = builder.Build();

await app.RunAsync();

sealed class MainWindow(IRendererProvider rendererProvider) : Window
{
    private int _h;

    protected override void OnRender(double time)
    {
        using var renderer = rendererProvider.GetRenderer(this);

        renderer.Clear(Color.FromHsv(_h, 100, 100));
        _h++;
        if (_h > 360)
            _h = 0;
    }
}