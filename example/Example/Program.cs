using System.Collections.Immutable;
using System.Diagnostics;
using System.Formats.Asn1;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Shimakaze.UI.Core;
using Shimakaze.UI.Input;
using Shimakaze.UI.Rendering;

using SkiaSharp;

var builder = Host.CreateShimakazeUIApplicationBuilder(args);
builder.Services.UseGlfw();
builder.Services.UseOpenGL();
builder.Services.AddWindow<MainWindow>();

var app = builder.Build();

await app.RunAsync();

sealed class MainWindow(IRenderer renderer, ILogger<MainWindow> logger) : Window
{
    private int _h;

    protected override void OnRender(double time)
    {
        var surface = renderer.GetSurface(this);

        surface.Canvas.Clear(SKColor.FromHsv(_h, 100, 100));
        surface.Flush();
        _h++;
        if (_h > 360)
            _h = 0;
    }
}