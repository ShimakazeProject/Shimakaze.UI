using System.Drawing;

using Microsoft.Extensions.DependencyInjection;

using Shimakaze.Foundation.Rendering;
using Shimakaze.Foundation.Rendering.Extensions;
using Shimakaze.Foundation.Runtime;
using Shimakaze.Foundation.Windowing;
using Shimakaze.Foundation.Windowing.GLFW;
using Shimakaze.Foundation.Windowing.Rendering;
using Shimakaze.Foundation.Windowing.Rendering.OpenGL;
using Shimakaze.Foundation.Windowing.Rendering.Vulkan;
using Shimakaze.Foundation.Windowing.SDL;
using Shimakaze.UI;
using Shimakaze.UI.Controls;
using Shimakaze.UI.Fonts;
using Shimakaze.UI.Media;

using SkiaSharp;

var builder = ApplicationBuilder.Create();

builder.Services.AddPlatformWindowFactory<GlfwPlatformWindowFactory>();
//builder.Services.AddPlatformWindowFactory<SdlPlatformWindowFactory>();

builder.Services.AddPlatformWindowRendererProvider<OpenGLPlatformWindowRendererProvider>();
//builder.Services.AddPlatformWindowRendererProvider<VulkanPlatformWindowRendererProvider>();

builder.Services.AddSingleton<MainWindow>();
builder.Services.AddSingleton<PlatformWindow>(provider => provider.GetRequiredService<MainWindow>().PlatformWindow);

Application app = builder.Build();

await app.RunAsync();

sealed class MainWindow : Window
{
    public MainWindow(PlatformWindowRendererProvider rendererProvider) : base(rendererProvider)
    {
        using var fs = File.OpenRead(@"D:\Shimakaze\Desktop\85402558_p18_ex.png");
        var image = SKImage.FromEncodedData(fs);
        Content = new ChildrenElement()
        {
            Children =
            {
                new ColorPanel(),
                new FPSCounter(),
                new Image()
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    ImageSource = new SkiaImageSource(image),
                    Width = 1052 / 3,
                    Height = 1494 / 3,
                },
                new TextBlock()
                {
                    Text = "Hello",
                    Font = Font.FromFamilyName("Microsoft YaHei UI", 16),
                    Margin = new(24,16)
                },
            },
        };
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        Console.WriteLine(PlatformWindow.DangerousGetNative().Native?.Kind);
    }
}

sealed class FPSCounter : UIElement
{
    private int _frameCount = 0;
    private double _timeAccumulator = 0f;
    private double _currentFps = 0f;

    protected override void OnRender(Renderer renderer, double deltaTime)
    {
        _frameCount++;
        _timeAccumulator += deltaTime;

        // 当累计时间达到 1s (1秒) 时，计算一次 FPS
        if (_timeAccumulator >= 1.0f)
        {
            _currentFps = (_frameCount * 1.0f) / _timeAccumulator;

            // 重置计数器和时间累加器
            _frameCount = 0;
            _timeAccumulator = 0f;
        }

        renderer.DrawText(_currentFps.ToString(), RenderRect);

        base.OnRender(renderer, deltaTime);
    }
}

sealed class ColorPanel : UIElement
{
    private double _time;
    protected override void OnRender(Renderer renderer, double deltaTime)
    {
        renderer.Clear(Color.FromHsv((int)double.Floor(_time / 6 * 360), 100, 100));
        _time += deltaTime;
        if (_time > 6)
            _time -= 6;
    }
}