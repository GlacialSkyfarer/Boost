using Godot;
using System;

public partial class MusicTrigger : Node
{

    SoundManager sm;
    [Export(PropertyHint.File)]
    public string filePath = "Silence";
    [Export]
    public float volume = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

        sm = GetTree().GetFirstNodeInGroup("SoundManager") as SoundManager;
        
    }

    public void Trigger(Node3D body) {

        sm.PlayMusic(new(filePath, volume, Vector3.Zero));

    }

}
