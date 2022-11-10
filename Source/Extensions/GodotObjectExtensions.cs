namespace NokLib_Godot;
public static class GodotObjectExtensions
{
    public static bool IsNull(this Godot.Object obj) => obj == null;
    public static bool IsNotNull(this Godot.Object obj) => !obj.IsNull();
}