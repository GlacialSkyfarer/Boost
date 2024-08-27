using Godot;
using System;

public partial class TimedDestroyParticles : GpuParticles3D
{

    [Export]
    public float time = 1f;
    [Export]
    public bool active = true;

    async void Destroy() {

        await ToSignal(GetTree().CreateTimer(time), "timeout");
        QueueFree();
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (active) Destroy();
    }

}
