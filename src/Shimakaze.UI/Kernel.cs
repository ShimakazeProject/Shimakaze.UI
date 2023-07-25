using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Shimakaze.UI;

public static partial class Kernel
{
    private static IContentProvider? s_contentProvider;

    [LibraryImport("shimakaze_ui", EntryPoint = "run", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    private static unsafe partial void Run(
        string title,
        delegate* unmanaged[Cdecl]<
            byte*,
            nuint,
            ResponseData
        > get_data);

    private static unsafe nint AllocHGlobal(byte[] data)
    {
        nint ptr = Marshal.AllocHGlobal(data.Length);
        fixed (void* p = data)
            Buffer.MemoryCopy(p, (void*)ptr, data.Length, data.Length);
        return ptr;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static unsafe ResponseData Callback(byte* chars, nuint size)
    {
        _ = s_contentProvider
            ?? throw new NullReferenceException("s_contentProvider is null");
        s_contentProvider.GetContent(
            Encoding.UTF8.GetString(chars, (int)size),
            out byte[]? content,
            out string? contentType
        );

        if (content is null || contentType is null)
        {
            return new()
            {
                Data = 0,
                Size = 0,
                ContentType = 0,
                ContentTypeSize = 0,
            };
        }

        nint pContent = AllocHGlobal(content);
        nint pContentType = AllocHGlobal(Encoding.ASCII.GetBytes(contentType));

        return new()
        {
            Data = pContent,
            Size = (nuint)content.Length,
            ContentType = pContentType,
            ContentTypeSize = (nuint)contentType.Length,
        };
    }

    public static void Run(string title, IContentProvider contentProvider)
    {
        s_contentProvider = contentProvider;
        unsafe
        {
            Run(title, &Callback);
        }
    }
    }
