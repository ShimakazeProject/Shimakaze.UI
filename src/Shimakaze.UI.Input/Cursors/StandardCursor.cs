using Silk.NET.Input;

namespace Shimakaze.UI.Input.Cursors;

internal sealed class StandardCursor(Silk.NET.Input.StandardCursor? standardCursor) : Cursor()
{
    internal override Task Apply(ICursor cursor, CancellationToken cancellationToken)
    {
        if (standardCursor.HasValue)
        {
            cursor.CursorMode = CursorMode.Normal;
            cursor.Type = CursorType.Standard;
            cursor.StandardCursor = standardCursor.Value;
        }
        else
        {
            cursor.CursorMode = CursorMode.Hidden;
        }
        return Task.CompletedTask;
    }
}