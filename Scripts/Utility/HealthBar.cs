using Godot;
using System;

public partial class HealthBar : TextureProgressBar
{

	[Export]
	public NodePath actorPath;
	public Actor actor;

	[Export]
	public Gradient colorCurve;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		actor = GetNode<Actor>(actorPath);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		Value = Mathf.Lerp(Value, actor.health, 0.04f);
		MaxValue = actor.maxHealth;
		TintProgress = colorCurve.Sample((float)(Value / MaxValue));

	}
}
