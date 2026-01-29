namespace Shimakaze.UI.Core.Dispatchers;

public enum DispatcherTaskStatus
{
    WaitingToRun,
    Running,
    Completed,
    Canceled,
    Faulted,
}