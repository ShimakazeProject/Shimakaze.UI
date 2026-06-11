using Silk.NET.Vulkan;

namespace Shimakaze.UI.Rendering.Vulkan;

internal static class ResultExtensions
{
    public static void EnsureSuccessed(this Result result)
    {
        if (result is not Result.Success)
            throw new InvalidProgramException();
    }
    public static void EnsureSuccessed(this Result result, string message)
    {
        if (result is not Result.Success)
            throw new InvalidProgramException(message);
    }
}