// See https://aka.ms/new-console-template for more information

using GlobalHotKeys;
using GlobalHotKeys.Native.Types;
using PowerShortcuts.Host;
using PowerShortcuts.VirtualDesktop;

const Modifiers switchDesktopModifier = Modifiers.Alt;
const Modifiers moveWindowModifier = Modifiers.Alt | Modifiers.Shift;

VirtualKeyCode[] numberKeyCodes = [
    VirtualKeyCode.KEY_1,
    VirtualKeyCode.KEY_2,
    VirtualKeyCode.KEY_3,
    VirtualKeyCode.KEY_4,
    VirtualKeyCode.KEY_5,
    VirtualKeyCode.KEY_6,
    VirtualKeyCode.KEY_7,
    VirtualKeyCode.KEY_8,
    VirtualKeyCode.KEY_9,
    VirtualKeyCode.KEY_0
];

var builder = WebApplication.CreateSlimBuilder(args);
var app = builder.Build();

var desktopManager = new VirtualDesktopManager();
var windowManager = new WindowManager();

using var hotKeyManager = new HotKeyManager();

using var desktopSubscription = hotKeyManager.HotKeyPressed.Subscribe(DesktopHotKeyPressed);
using var registrations = new DisposableStack();

var switchDesktopRegistrations = numberKeyCodes
    .Select(key => hotKeyManager.Register(key, switchDesktopModifier))
    .ToArray();

var moveWindowRegistrations = numberKeyCodes
    .Select(key => hotKeyManager.Register(key, moveWindowModifier))
    .ToArray();

registrations.PushRange(switchDesktopRegistrations);
registrations.PushRange(moveWindowRegistrations);

app.Run();

return;

void DesktopHotKeyPressed(HotKey hotKey)
{
    var desktopNumber = hotKey.Key switch
    {
        VirtualKeyCode.KEY_1 => 0,
        VirtualKeyCode.KEY_2 => 1,
        VirtualKeyCode.KEY_3 => 2,
        VirtualKeyCode.KEY_4 => 3,
        VirtualKeyCode.KEY_5 => 4,
        VirtualKeyCode.KEY_6 => 5,
        VirtualKeyCode.KEY_7 => 6,
        VirtualKeyCode.KEY_8 => 7,
        VirtualKeyCode.KEY_9 => 8,
        VirtualKeyCode.KEY_0 => 9,
        _ => throw new ArgumentException()
    };

    var numDesktops = desktopManager.GetDesktopCount();

    if (desktopNumber > numDesktops - 1) return;

    switch (hotKey.Modifiers)
    {
        case switchDesktopModifier:
            desktopManager.GoToDesktopNumber(desktopNumber);
            break;
        case moveWindowModifier:
        {
            var windowInFocus = windowManager.GetWindowInFocus();
            var currentWindowDesktop = desktopManager.GetWindowDesktopNumber(windowInFocus);

            if (currentWindowDesktop == desktopNumber) return;

            desktopManager.MoveWindowToDesktopNumber(windowInFocus, desktopNumber);
            break;
        }
    }

}