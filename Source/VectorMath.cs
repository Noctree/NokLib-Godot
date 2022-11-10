using Godot;

namespace NokLib_Godot;
public static class VectorMath
{
    public static Vector2 AngleToDirection(float angle) {
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }
}
