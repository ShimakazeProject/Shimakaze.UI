using Microsoft.Extensions.DependencyInjection;

using Shimakaze.Foundation.Runtime;
using Shimakaze.Foundation.Windowing;
using Shimakaze.Foundation.Windowing.GLFW;
using Shimakaze.Foundation.Windowing.Rendering;
using Shimakaze.Foundation.Windowing.Rendering.OpenGL;
using Shimakaze.Foundation.Windowing.Rendering.Vulkan;
using Shimakaze.Foundation.Windowing.SDL;

var builder = ApplicationBuilder.Create();

builder.Services.AddPlatformWindowFactory<GlfwPlatformWindowFactory>();
//builder.Services.AddPlatformWindowFactory<SdlPlatformWindowFactory>();

builder.Services.AddPlatformWindowRendererProvider<OpenGLPlatformWindowRendererProvider>();
//builder.Services.AddPlatformWindowRendererProvider<VulkanPlatformWindowRendererProvider>();

builder.Services.AddSingleton<PlatformWindow, MainWindow>();

Application app = builder.Build();

await app.RunAsync();