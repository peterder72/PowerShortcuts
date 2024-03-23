// See https://aka.ms/new-console-template for more information

using GlobalHotKeys;
using GlobalHotKeys.Native.Types;

var builder = WebApplication.CreateSlimBuilder(args);
var app = builder.Build();

var mgr = new VirtualDesktopManager();

void HotKeyPressed(HotKey hotKey)
{
    var number = hotKey.Key switch
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

    var numDesktops = mgr.GetDesktopCount();

    if (number > numDesktops - 1) return;

    mgr.GoToDesktopNumber(number);
}

using var hotKeyManager = new HotKeyManager();
using var subscription = hotKeyManager.HotKeyPressed.Subscribe(HotKeyPressed);

using (hotKeyManager.Register(VirtualKeyCode.KEY_1, Modifiers.Alt))
using (hotKeyManager.Register(VirtualKeyCode.KEY_2, Modifiers.Alt))
using (hotKeyManager.Register(VirtualKeyCode.KEY_3, Modifiers.Alt))
using (hotKeyManager.Register(VirtualKeyCode.KEY_4, Modifiers.Alt))
using (hotKeyManager.Register(VirtualKeyCode.KEY_5, Modifiers.Alt))
using (hotKeyManager.Register(VirtualKeyCode.KEY_6, Modifiers.Alt))
using (hotKeyManager.Register(VirtualKeyCode.KEY_7, Modifiers.Alt))
using (hotKeyManager.Register(VirtualKeyCode.KEY_8, Modifiers.Alt))
using (hotKeyManager.Register(VirtualKeyCode.KEY_9, Modifiers.Alt))
using (hotKeyManager.Register(VirtualKeyCode.KEY_0, Modifiers.Alt))
{
    app.Run();
}