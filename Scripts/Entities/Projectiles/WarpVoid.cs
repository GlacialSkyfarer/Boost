using Godot;
using System;

public partial class WarpVoid : Projectile
{

	[Export]
	public NodePath collider;
	public CollisionShape3D colliderShape;

	[Export(PropertyHint.File)]
	public string burstSound;

	[Export]
	public PackedScene explosionScene;
	[Export]
	public CurveTexture explosionCurve;
	[Export]
	public float explosionDuration = 1;
	[Export]
	public GradientTexture1D explosionColorGradient;

	public bool dead = false;

	SoundManager soundManager;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		soundManager = GetNode<SoundManager>("/root/SoundManager");
		base._Ready();
		colliderShape = GetNode<CollisionShape3D>(collider);
		Timeout();

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		base._Process(delta);

	}

	public override void OnCollide()
	{
		
		if ((shapeCast.GetCollider(0) as CollisionObject3D).GetCollisionLayerValue(1)) {

			Destroy();

		}

	}

	public async void Destroy() {

		dead = true;

		Explosion exp = explosionScene.Instantiate<Explosion>();
		GetParent().AddChild(exp);
		exp.GlobalPosition = GlobalPosition;
		exp.scaleCurve = explosionCurve;
		exp.time = explosionDuration;
		exp.colorGradient = explosionColorGradient;

		soundManager.PlaySound(new(burstSound, 0, GlobalPosition));

		colliderShape.Disabled = true;
		shapeCast.Enabled = false;
		Tween tw = GetTree().CreateTween();
		tw.TweenProperty(this, "scale", Vector3.One * 0.001f, 0.2);
		await ToSignal(GetTree().CreateTimer(0.3), "timeout");
		QueueFree();

	}

	public async void Timeout() {

		await ToSignal(GetTree().CreateTimer(0.01), "timeout");
		Explosion exp = explosionScene.Instantiate<Explosion>();
		GetParent().AddChild(exp);
		exp.GlobalPosition = GlobalPosition;
		exp.scaleCurve = explosionCurve;
		exp.time = explosionDuration;
		exp.colorGradient = explosionColorGradient;

		await ToSignal(GetTree().CreateTimer(10), "timeout");
		Destroy();

	}

}
