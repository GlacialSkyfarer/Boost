using Godot;
using System;

public partial class HealthBar : TextureProgressBar
{

	[Export]
	public NodePath actorPath;
	public LivingActor actor;
	[Export]
	public NodePath underlayPath;
	public TextureProgressBar underlay;

	[Export]
	public float mainLerp = 0.25f;
	[Export]
	public float underlayLerp = 0.04f;

	[Export]
	public Gradient colorCurve;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		underlay = GetNode<TextureProgressBar>(underlayPath);

		actor = GetNode<LivingActor>(actorPath);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		Value = Mathf.Lerp(Value, actor.health, mainLerp);
		MaxValue = actor.maxHealth;
		TintProgress = colorCurve.Sample((float)(Value / MaxValue));
		underlay.Value = Mathf.Lerp(underlay.Value, actor.health, underlayLerp);
		underlay.MaxValue = actor.maxHealth;

	}
}
