namespace PowerShortcuts.WinService.Singleton;

internal static class SingletonOperations
{
   public static SingletonLock SingleInstanceLock(string name)
   {
      return new SingletonLock(name);
   } 
}