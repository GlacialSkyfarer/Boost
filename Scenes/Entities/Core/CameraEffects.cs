using Godot;
using System;

public partial class CameraEffects : Camera3D
{

	[ExportGroup("References")]
	[Export]
	public NodePath animationTreePath;
	public AnimationTree animationTree;

	[ExportGroup("Properties")]

	[Export]
	public bool strafeAnimation = true;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		animationTree = GetNode<AnimationTree>(animationTreePath);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
