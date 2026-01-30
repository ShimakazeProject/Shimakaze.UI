
using System.Collections.Immutable;

namespace Shimakaze.UI.Core;

public sealed class FileDropEventArgs(string[] filePaths) : EventArgs
{
    public ImmutableArray<string> FilePaths { get; } = [.. filePaths];
}