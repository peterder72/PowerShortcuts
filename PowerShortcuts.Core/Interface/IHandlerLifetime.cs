namespace PowerShortcuts.Core.Interface;

internal interface IHandlerLifetime: IDisposable
{
    IPowerShortcutHandler[] Handlers { get; }
}