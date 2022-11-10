using Godot;

namespace NokLib_Godot;
public class Node2DPrefab<T> : NodePrefab<T> where T : Node2D
{
    public Node2DPrefab(string path) : base(path) { }

    public T Instantiate(Vector2 position) {
        var instance = Instantiate();
        instance.Position = position;
        return instance;
    }

    public T Instantiate(Vector2 position, float rotation) {
        var instance = Instantiate(position);
        instance.Rotation = rotation;
        return instance;
    }

    public T Instantiate(Vector2 position, float rotation, Vector2 scale) {
        var instance = Instantiate(position, rotation);
        instance.Scale = scale;
        return instance;
    }
}
