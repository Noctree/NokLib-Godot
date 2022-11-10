using System;
using Godot;

namespace NokLib_Godot;

[Serializable]
public struct MovementInputMap2D
{
    public static readonly MovementInputMap2D DefaultWSAD = new MovementInputMap2D(Key.W, Key.S, Key.A, Key.D);
    public static readonly MovementInputMap2D DefaultArrowKeys = new MovementInputMap2D(Key.Up, Key.Down, Key.Left, Key.Right);
    public Key Up;
    public Key Down;
    public Key Left;
    public Key Right;

    public MovementInputMap2D(Key up, Key down, Key left, Key right) {
        Up = up;
        Down = down;
        Left = left;
        Right = right;
    }
}
