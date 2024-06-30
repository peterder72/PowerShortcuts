using PowerShortcuts.Core.Handlers;
using PowerShortcuts.Core.Interface;

namespace PowerShortcuts.Core;

internal sealed class HardcodedDefaultHandlerOrderProvider: IHandlerOrderProvider
{
    public int GetOrder(IPowerShortcutHandler handler)
    {
        if (handler.Id == DefaultHandlersGuids.PinOperation)
        {
            return 0;
        } else if (handler.Id == DefaultHandlersGuids.VirtualDesktopSwitch)
        {
            return 1;
        } else
        {
            return 2;
        }
    }
}