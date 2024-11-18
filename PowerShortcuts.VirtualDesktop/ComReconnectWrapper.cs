using System.Runtime.InteropServices;

namespace PowerShortcuts.VirtualDesktop;

internal sealed class ComReconnectWrapper<T>(Func<T> instanceFactory, Action onReconnect) where T : class
{
    private const int ComConnectionFailedCode = -2147023174;

    private T m_Instance = instanceFactory.Invoke();

    public void Execute(Action<T> action, bool retry = true)
    {
        try
        {
            action(m_Instance);
        }
        catch (COMException e) when (e.ErrorCode == ComConnectionFailedCode)
        {
            if (!retry) throw CreateAggregateException(e);
            
            OnReconnect();
            Execute(action, false);
        }
    }

    public TRet Execute<TRet>(Func<T, TRet> action, bool retry = true)
    {
        try
        {
            return action(m_Instance);
        }
        catch (COMException e) when (e.ErrorCode == ComConnectionFailedCode)
        {
            if (!retry) throw CreateAggregateException(e);

            OnReconnect();
            return Execute(action, false);
        }
    }

    private void OnReconnect()
    {
        onReconnect.Invoke();
        m_Instance = instanceFactory.Invoke();
    }

    private static AggregateException CreateAggregateException(COMException e) =>
        new("Failed to reconnect to COM instance after multiple attempts", e);
}