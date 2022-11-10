using System;
using System.Collections.Generic;
using Godot;
using NokLib;

namespace NokLib_Godot;
public static class Singleton
{
    private const string PREFIX = "[Singleton]: ";
    private const string REGISTER_MSG = PREFIX + "Registered {0} ({1})";
    private const string UNREGISTER_MSG = PREFIX + "Unregistered {0} ({1})";
    private const string UNREGISTER_ALL_MSG = PREFIX + "UNREGISTERING ALL INSTANCES";
    private static Dictionary<Type, object> singletons = new();
    private static Dictionary<Type, Delegate> singletonInstanceAwaiters = new();

#if DEBUG
    public static bool DebugLogging { get; set; } = true;
#else
    public static bool DebugLogging { get; set; } = false;
#endif

    private static Type GetID<T>(T? _ = null) where T : class => typeof(T);

    public static void RegisterSingleton<T>(T instance) where T : class {
        var id = GetID(instance);
        LogMessage(REGISTER_MSG, instance);
        if (singletons.ContainsKey(id))
            throw new DuplicateObjectInCollectionException($"A singleton instance of {typeof(T).Name} is already registered");
        if (instance == null)
            throw new NullReferenceException(nameof(instance));
        singletons.Add(id, instance);
        if (singletonInstanceAwaiters.TryGetValue(id, out var callback))
            ((Action<T>)callback)?.Invoke(instance);
    }

    public static void UnregisterSingleton<T>(T _) where T : class => UnregisterSingleton<T>();

    public static T? UnregisterSingleton<T>() where T : class {
        var id = GetID<T>();
        T? instance = null;
        LogMessage(UNREGISTER_MSG, instance);
        if (singletons.TryGetValue(id, out var instanceRaw)) {
            instance = (T)instanceRaw;
            singletons.Remove(id);
        }
        return instance;
    }

    public static bool IsInstanceRegistered<T>() where T : class => singletons.ContainsKey(GetID<T>());

    public static T? GetInstance<T>() where T : class {
        var guid = GetID<T>();
        if (singletons.TryGetValue(guid, out var instance))
            return (T)instance;
        else
            return null;
    }

    public static bool WaitForSingletonRegistration<T>(Action<T> callback) where T : class {
        var id = GetID<T>();
        if (singletons.TryGetValue(id, out var instance)) {
            callback((T)instance);
            return true;
        }
        else {
            if (singletonInstanceAwaiters.TryGetValue(id, out var delegates))
                singletonInstanceAwaiters[id] = (Action<T>)delegates + callback;
            else
                singletonInstanceAwaiters.Add(id, callback);
            return false;
        }
    }

    public static void UnregisterAllInstances() {
        LogMessage(UNREGISTER_ALL_MSG);
        singletonInstanceAwaiters.Clear();
        singletons.Clear();
    }

    private static void LogMessage<T>(string message, T? _ = null) where T : class {
        if (DebugLogging)
            LogMessage(string.Format(message, typeof(T).Name, GetID(_)));
    }

    private static void LogMessage(string message) {
        if (DebugLogging)
            GD.Print(message);
    }
}
