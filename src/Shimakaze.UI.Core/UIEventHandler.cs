namespace Shimakaze.UI;

public delegate void UIEventHandler<TSender>(TSender sender, EventArgs eventArgs);
public delegate void UIEventHandler<TSender, TEventArgs>(TSender sender, TEventArgs eventArgs)
    where TEventArgs : EventArgs;