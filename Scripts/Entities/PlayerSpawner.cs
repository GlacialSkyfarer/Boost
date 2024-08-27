using Godot;
using System;

public partial class PlayerSpawner : Node3D
{

	[Export] public PackedScene playerScene;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		CallDeferred("Setup");

	}

	public void Setup() {

		Player p = playerScene.Instantiate<Player>();
		GetParent().AddChild(p);
		p.GlobalPosition = GlobalPosition;
		QueueFree();

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
