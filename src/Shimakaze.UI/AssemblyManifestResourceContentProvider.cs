using System.Reflection;

using MimeDetective;
using MimeDetective.Definitions;

namespace Shimakaze.UI;

public sealed class AssemblyManifestResourceContentProvider : IContentProvider
{
    private readonly Assembly _assembly;
    private readonly IDictionary<string, string> _name;
    private static readonly ContentInspector Inspector = new ContentInspectorBuilder() {
        Definitions = Default.All()
    }.Build();


    public AssemblyManifestResourceContentProvider(Assembly assembly)
    {
        _assembly = assembly;
        _name = _assembly.GetManifestResourceNames()
            .ToDictionary(x => $"/{x.Replace('\\', '/').TrimStart('/')}");
    }

    public void GetContent(string path, out byte[]? content, out string? contentType)
    {
        if (_name.TryGetValue(path, out var name))
        {
            using var stream = _assembly.GetManifestResourceStream(name);
            if (stream is not null)
            {
                content = new byte[stream.Length];
                stream.Read(content, 0, content.Length);
                var result = Inspector.Inspect(content);
                contentType = result.ByMimeType().FirstOrDefault()?.MimeType;
            }

        }
        throw new NotImplementedException();
    }
}