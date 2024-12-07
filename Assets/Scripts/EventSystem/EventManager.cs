using System;
using System.Collections.Generic;
using System.Linq;

public static class EventManager
{
    private static readonly Dictionary<(Enum eventName, string signature), Delegate> eventTable = new();

    #region Helper

    // Lấy chữ ký của delegate dựa vào kiểu tham số và kiểu trả về
    private static string GetMethodSignature(Delegate del)
    {
        var parameters = del.Method.GetParameters()
            .Select(p => p.ParameterType.Name)
            .ToArray();
        var returnType = del.Method.ReturnType.Name;

        return string.Join(",", parameters) + "->" + returnType;
    }

    // Tạo chữ ký từ tham số khi gọi Raise
    private static string GetSignatureFromParameters(Type[] parameterTypes, Type returnType)
    {
        return string.Join(",", parameterTypes.Select(t => t.Name)) + "->" + returnType.Name;
    }

    #endregion

    #region Subscribe

    public static void Subscribe(Enum eventName, Delegate listener)
    {
        var signature = GetMethodSignature(listener);

        if (!eventTable.ContainsKey((eventName, signature)))
            eventTable[(eventName, signature)] = null;

        eventTable[(eventName, signature)] = Delegate.Combine(eventTable[(eventName, signature)], listener);
    }

    #endregion

    #region Unsubscribe

    public static void Unsubscribe(Enum eventName, Delegate listener)
    {
        var signature = GetMethodSignature(listener);

        if (eventTable.TryGetValue((eventName, signature), out var del))
        {
            eventTable[(eventName, signature)] = Delegate.Remove(del, listener);

            if (eventTable[(eventName, signature)] == null)
                eventTable.Remove((eventName, signature));
        }
    }

    #endregion

    #region Raise
    public static U Raise<U>(Enum eventName, params object[] parameters)
    {
        return (U) Raise(eventName, parameters);
    }

    public static object Raise(Enum eventName, params object[] parameters)
    {
        var parameterTypes = parameters.Select(p => p.GetType()).ToArray();
        var signature = GetSignatureFromParameters(parameterTypes, typeof(object)); // Kiểu trả về mặc định là object

        foreach (var kvp in eventTable)
        {
            if (kvp.Key.eventName.Equals(eventName) && kvp.Key.signature.StartsWith(string.Join(",", parameterTypes.Select(p => p.Name))))
            {
                try
                {
                    return kvp.Value?.DynamicInvoke(parameters);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Error invoking event '{eventName}': {ex.Message}", ex);
                }
            }
        }

        throw new KeyNotFoundException($"Event '{eventName}' with signature '{signature}' not found.");
    }

    #endregion
}