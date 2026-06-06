namespace Shimakaze.UI.Rendering;

public sealed class BaseRenderer(ISurfaceProvider surfaceProvider) : Renderer(surfaceProvider.Begin())
{
    public override void Dispose()
    {
        Canvas.Flush();
        Surface.Flush();
        surfaceProvider.End();
    }
}