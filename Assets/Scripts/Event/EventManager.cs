using System;
using System.Collections.Generic;

public static class EventManager<T> where T : Enum
{
    private static Dictionary<T, Delegate> actionTable = new Dictionary<T, Delegate>();
    private static Dictionary<T, Delegate> funcTable = new Dictionary<T, Delegate>();

    #region Subscribe

    // Đăng ký Action
    public static void SubscribeAction(T eventName, Delegate listener)
    {
        if (!actionTable.ContainsKey(eventName))
            actionTable[eventName] = null;

        actionTable[eventName] = Delegate.Combine(actionTable[eventName], listener);
    }

    // Đăng ký Func
    public static void SubscribeFunc(T eventName, Delegate listener)
    {
        if (!funcTable.ContainsKey(eventName))
            funcTable[eventName] = null;

        funcTable[eventName] = Delegate.Combine(funcTable[eventName], listener);
    }

    #endregion

    #region Unsubscribe

    // Hủy đăng ký Action
    public static void UnsubscribeAction(T eventName, Delegate listener)
    {
        if (actionTable.ContainsKey(eventName))
        {
            actionTable[eventName] = Delegate.Remove(actionTable[eventName], listener);

            if (actionTable[eventName] == null)
                actionTable.Remove(eventName);
        }
    }

    // Hủy đăng ký Func
    public static void UnsubscribeFunc(T eventName, Delegate listener)
    {
        if (funcTable.ContainsKey(eventName))
        {
            funcTable[eventName] = Delegate.Remove(funcTable[eventName], listener);

            if (funcTable[eventName] == null)
                funcTable.Remove(eventName);
        }
    }

    #endregion

    #region Raise

    // Gọi Action
    public static void RaiseAction(T eventName, params object[] parameters)
    {
        if (actionTable.TryGetValue(eventName, out var del))
        {
            del?.DynamicInvoke(parameters);
        }
        else
        {
            throw new KeyNotFoundException($"Action event {eventName} not found.");
        }
    }

    // Gọi Func
    public static object RaiseFunc(T eventName, params object[] parameters)
    {
        if (funcTable.TryGetValue(eventName, out var del))
        {
            return del?.DynamicInvoke(parameters);
        }
        else
        {
            throw new KeyNotFoundException($"Func event {eventName} not found.");
        }
    }

    #endregion
}

public enum GameEvent
{
    OnScoreIncrease,
    GetScoreIncrease
}

public enum UIEvent
{
        
}
