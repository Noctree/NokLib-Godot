using System;
using Godot;

namespace NokLib_Godot;
internal partial class GodotProcessingHook : Node
{
    public static Action<double>? ProcessCallback;
    public static Action<double>? PhysicsProcessCallback;
    public override void _Ready() {
        GD.Print(nameof(GodotProcessingHook), " Initializing...");
        Singleton.RegisterSingleton(this);
    }

    public override void _Process(double delta) => ProcessCallback?.Invoke(delta);
    public override void _PhysicsProcess(double delta) => PhysicsProcessCallback?.Invoke(delta);
}
