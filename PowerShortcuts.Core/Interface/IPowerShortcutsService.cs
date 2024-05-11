namespace PowerShortcuts.Core.Interface;

public interface IPowerShortcutsService: IDisposable
{
    public void Initialize();
    public void Terminate();
}