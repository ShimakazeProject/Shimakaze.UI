using System.Collections.Concurrent;

using Shimakaze.UI.Core;
using Shimakaze.UI.Input.EventArgs;

using Silk.NET.Input;

namespace Shimakaze.UI.Input;

public sealed class KeyboardManager
{
    public event UIEventHandler<KeyboardManager, KeyboardKeyEventArgs>? KeyPressed;

    private readonly ConcurrentDictionary<Key, KeyboardManagerContext> _states = new();

    /// <summary>
    /// 首次按键重复延迟（秒），默认 0.5 秒
    /// </summary>
    public TimeSpan InitialRepeatDelay { get; set; } = TimeSpan.FromSeconds(0.5);

    /// <summary>
    /// 后续按键重复间隔（秒），默认 0.05 秒
    /// </summary>
    public TimeSpan RepeatInterval { get; set; } = TimeSpan.FromSeconds(0.05);

    public KeyboardManager(InputManager inputManager)
    {
        inputManager.KeyboardKeyDown += OnKeyDown;
        inputManager.KeyboardKeyUp += OnKeyUp;
        inputManager.Window.Update += (_, args) => Tick(args.DeltaTime);
    }

    private void OnKeyUp(InputManager sender, KeyboardKeyEventArgs eventArgs)
    {
        _states.Remove(eventArgs.Key, out _);
    }

    private void OnKeyDown(InputManager sender, KeyboardKeyEventArgs eventArgs)
    {
        KeyPressed?.Invoke(this, eventArgs);
        _states.AddOrUpdate(eventArgs.Key, new KeyboardManagerContext(eventArgs), (_, _) => new(eventArgs));
    }

    private void Tick(double deltaTime)
    {
        var initialRepeatDelay = InitialRepeatDelay.TotalSeconds;
        var repeatInterval = RepeatInterval.TotalSeconds;

        foreach (var context in _states.Values)
        {
            context.LastKeyDown += deltaTime;

            // 检查是否应该触发重复事件
            if (context.LastKeyDown >= initialRepeatDelay)
            {
                var elapsedTimeSinceFirstRepeat = context.LastKeyDown - initialRepeatDelay;
                var repeatCount = (int)(elapsedTimeSinceFirstRepeat / repeatInterval);

                // 如果是首次重复或经过了一个完整间隔
                var previousRepeatCount = (int)((context.LastKeyDown - deltaTime - initialRepeatDelay) / repeatInterval);
                if (repeatCount > previousRepeatCount)
                {
                    KeyPressed?.Invoke(this, context.EventArgs);
                }
            }
        }
    }
    private sealed class KeyboardManagerContext(KeyboardKeyEventArgs eventArgs)
    {
        public double LastKeyDown { get; set; }

        public KeyboardKeyEventArgs EventArgs { get; } = eventArgs;
    }
}