using Godot;
using System;

public partial class Projectile : Node3D
{

	public Vector3 velocity;

	[Export]
	public NodePath cast;
	public ShapeCast3D shapeCast;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		shapeCast = GetNode<ShapeCast3D>(cast);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		shapeCast.TargetPosition = velocity * (float)delta;

		if (shapeCast.IsColliding()) {

			OnCollide();

		} 

		GlobalPosition += velocity * (float)delta;

	}

	public virtual void OnCollide() {



	}

}
