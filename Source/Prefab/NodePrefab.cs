using System;
using System.IO;
using Godot;
using NokLib;

namespace NokLib_Godot;
public class NodePrefab<T> : IDisposable where T : Node
{
    private PackedScene? prefab;
    private string prefabPath;
    private bool disposedValue;

    public NodePrefab(string path) {
        prefabPath = path;
        prefabPath = prefabPath.PrependIfMissing("res://");
        if (!ResourceLoader.Exists(prefabPath))
            throw new FileNotFoundException($"Prefab does not exist ({prefabPath})");
    }

    public void Load() {
        if (prefab == null)
            prefab = GD.Load<PackedScene>(prefabPath);
        if (prefab == null)
            throw new Exception($"Scene file failed to load ({prefabPath})");
    }

    public T Instantiate() {
        if (prefab == null)
            throw new Exception("Scene file not loaded");
        var instance = prefab.Instantiate<T>();
        return instance;
    }

    protected virtual void Dispose(bool disposing) {
        if (!disposedValue) {
            if (disposing) {
                // TODO: dispose managed state (managed objects)
            }

            prefabPath = null;
            prefab?.Dispose();
            disposedValue = true;
        }
    }

    // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    ~NodePrefab() {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    public void Dispose() {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
