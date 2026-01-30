using Silk.NET.Input;

namespace Shimakaze.UI.Input;

internal static class IInputContextExtensions
{
    extension(IInputContext context)
    {
        public IEnumerable<IInputDevice> Devices => context.GetDevices();
        private IEnumerable<IInputDevice> GetDevices()
        {
            foreach (var device in context.Gamepads)
                yield return device;
            foreach (var device in context.Joysticks)
                yield return device;
            foreach (var device in context.Keyboards)
                yield return device;
            foreach (var device in context.Mice)
                yield return device;
            foreach (var device in context.OtherDevices)
                yield return device;
        }
    }
}