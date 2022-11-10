using System;
using Godot;

namespace NokLib_Godot;
public partial class FollowTarget2D : Node2D
{
    [Export] private NodePath TargetReference;
    [Export] public bool Smoothing;
    [Export] public float SmoothingSpeed;

    public Node2D Target;

    public override void _Ready() {
        SmoothingSpeed = Mathf.Clamp(SmoothingSpeed, 0, 1);
        Target = (Node2D)GetNode(TargetReference);
        if (Target == null)
            throw new NullReferenceException(nameof(TargetReference));
    }

    public override void _Process(double delta) {
        var targetPos = Target.Position;
        if (Smoothing) {
            targetPos = Position.Lerp(targetPos, SmoothingSpeed);
        }
        Position = targetPos;
    }
}
