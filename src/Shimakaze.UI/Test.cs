namespace Shimakaze.UI;

internal static class Test
{

    [STAThread]
    private static void Main()
    {
        Console.WriteLine("Hello, World!");
        Kernel.Run("Shimakaze.UI", new ContentProvider());
    }

    private sealed class ContentProvider : IContentProvider
    {
        public void GetContent(string path, out byte[]? content, out string? contentType)
        {
            contentType = null;
            content = null;
            if (path == "/favicon.ico")
                return;
            Console.WriteLine(path);
            contentType = "text/html";
            content = """
                <!DOCTYPE html>
                <html lang="en">
                <head>
                  <meta charset="UTF-8">
                  <meta name="viewport" content="width=device-width, initial-scale=1.0">
                  <title>Document</title>
                </head>
                <body>
                  <h1>.Net ❤ Rust</h1>
                </body>
                </html>
                """u8.ToArray();
        }
    }

}