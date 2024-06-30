using GlobalHotKeys.Native.Types;

namespace PowerShortcuts.Core.Interface;

internal interface IHotKeyRegistration
{
    void RegisterHotKey(VirtualKeyCode key, Modifiers modifiers);
}