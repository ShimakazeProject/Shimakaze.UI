namespace Shimakaze.UI;

public interface IContentProvider
{
    void GetContent(string path, out byte[]? content, out string? contentType);
}