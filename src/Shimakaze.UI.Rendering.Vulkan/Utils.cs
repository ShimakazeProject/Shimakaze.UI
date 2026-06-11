using System.Diagnostics;
using System.Runtime.CompilerServices;

using Silk.NET.Core;
using Silk.NET.Core.Native;
using Silk.NET.Vulkan;

namespace Shimakaze.UI.Rendering.Vulkan;

internal static class DebugExtensions
{
    extension(Debug)
    {
        [Conditional("DEBUG")]
        public static unsafe void Print(ReadOnlySpan<LayerProperties> layers)
        {
            Debug.WriteLine("available layers: ");
            Debug.Indent();
            foreach (var layer in layers)
            {
                var name = SilkMarshal.PtrToString((nint)layer.LayerName, NativeStringEncoding.UTF8);
                var spec = (Version32)layer.SpecVersion;
                var impl = (Version32)layer.ImplementationVersion;
                Debug.WriteLine($"{name}({spec.Major}.{spec.Minor}.{spec.Patch}:{impl.Major}.{impl.Minor}.{impl.Patch})");
            }
            Debug.Unindent();
        }

        [Conditional("DEBUG")]
        public static unsafe void Print(ReadOnlySpan<ExtensionProperties> extensions)
        {
            Debug.WriteLine("available extensions: ");
            Debug.Indent();
            foreach (var extension in extensions)
            {
                var name = SilkMarshal.PtrToString((nint)extension.ExtensionName, NativeStringEncoding.UTF8);
                var spec = (Version32)extension.SpecVersion;
                Debug.WriteLine($"{name}({spec.Major}.{spec.Minor}.{spec.Patch})");
            }
            Debug.Unindent();
        }

    }
}

internal static class Utils
{
    public delegate void RefToDo<T>(ref T reference, ref uint count) where T : unmanaged;

    public static unsafe T[] TwoStep<T>(RefToDo<T> action)
        where T : unmanaged
    {
        uint count = 0;
        action(ref Unsafe.AsRef<T>(null), ref count);
        T[] arr = GC.AllocateUninitializedArray<T>((int)count);
        fixed (T* ptr = arr)
            action(ref new Span<T>(ptr, arr.Length).GetPinnableReference(), ref count);

        return arr;
    }
}
