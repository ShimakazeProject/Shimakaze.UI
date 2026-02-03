using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateShimakazeUIApplicationBuilder(args);
builder.Services.UseGlfw();
builder.Services.UseOpenGL();
builder.Services.AddWindow<MainWindow>();

var app = builder.Build();

await app.RunAsync();
