namespace Shimakaze.UI.Core.Threading;

public enum DispatcherTaskStatus
{
    WaitingToRun,
    Running,
    Completed,
    Canceled,
    Faulted,
}