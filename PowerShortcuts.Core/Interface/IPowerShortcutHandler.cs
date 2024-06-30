using GlobalHotKeys;

namespace PowerShortcuts.Core.Interface;

internal interface IPowerShortcutHandler
{
    Guid Id { get; }
    bool HandleShortcut(HotKey hotKey);
    void RegisterHotkeys(IHotKeyRegistration registration);
}