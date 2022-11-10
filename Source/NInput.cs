using Godot;

namespace NokLib_Godot;
public static class NInput
{
    public static Vector2 Get2DInputAxis(MovementInputMap2D inputMap) {
        Vector2 axis = Vector2.Zero;
        if (Input.IsPhysicalKeyPressed(inputMap.Up))
            axis.y -= 1;
        if (Input.IsPhysicalKeyPressed(inputMap.Down))
            axis.y += 1;
        if (Input.IsPhysicalKeyPressed(inputMap.Left))
            axis.x -= 1;
        if (Input.IsPhysicalKeyPressed(inputMap.Right))
            axis.x += 1;
        return axis.Normalized();
    }
}
