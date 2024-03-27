using Godot;
using System;

public partial class CameraEffects : Camera3D
{

    Vector3 basePosition;

    public override void _Ready()
    {
        
        basePosition = Position;

    }

}
