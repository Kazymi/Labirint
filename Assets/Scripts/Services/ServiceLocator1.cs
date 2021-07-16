using System;
using System.Collections.Generic;

public static class ServiceLocator
{
    private static Dictionary<Type, object> services = new Dictionary<Type, object>();

    public static void Subscribe<T>(object service)
    {
        services.Add(typeof(T), service);
    }

    public static void Unsubscribe<T>()
    {
        services.Remove(typeof(T));
    }

    public static T GetService<T>()
    {
        try
        {
            return (T) services[typeof(T)];
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}