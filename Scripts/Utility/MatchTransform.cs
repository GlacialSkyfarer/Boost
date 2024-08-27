using Godot;
using System;

public partial class MatchTransform : Node3D
{

	[Export]
	public NodePath targetPath;
	public Node3D target;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		target = GetNode<Node3D>(targetPath);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		GlobalPosition = target.GlobalPosition;
		GlobalRotation = target.GlobalRotation;

	}
}
