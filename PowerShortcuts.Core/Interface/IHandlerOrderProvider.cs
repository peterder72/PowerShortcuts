namespace PowerShortcuts.Core.Interface;

internal interface IHandlerOrderProvider
{
    int GetOrder(IPowerShortcutHandler handler);
}