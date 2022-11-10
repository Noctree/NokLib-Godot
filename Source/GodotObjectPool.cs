using System;
using Godot;
using NokLib;

namespace NokLib_Godot;
public class GodotObjectPool<T> : ObjectPool<T> where T : notnull, Godot.Object
{
    private Action<T>? onGet;
    private Action<T>? onRelease;

    public event Action? Disposing;
    public GodotObjectPool(Func<T> createFunc, Action<T>? onGet = null, Action<T>? onRelease = null, int capacity = DEFAULT_MAX_CAPACITY)
        : base(OnDisposeObject, createFunc, null, null, capacity, DEFAULT_COLLECTION_CHECK) {
        this.onGet = onGet;
        this.onRelease = onRelease;
        base.onGetFunc = OnGetObject;
        base.onReleaseFunc = OnReleaseObject;
    }

    private void OnGetObject(T obj) {
        onGet?.Invoke(obj);
        if (obj is Node node)
            node.SetProcess(true);
    }

    private void OnReleaseObject(T obj) {
        onRelease?.Invoke(obj);
        if (obj is Node node) {
            node.GetParent().CallDeferred("remove_child", (obj as Node));
            node.SetProcess(false);
        }
    }

    private static void OnDisposeObject(T obj) {
        if (obj is Node node)
            node.QueueFree();
        else
            obj.Dispose();
    }

    protected override void Dispose(bool disposing) {
        Disposing?.Invoke();
        base.Dispose(disposing);
    }
}
