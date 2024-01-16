using Godot;
using System;
using System.ComponentModel;

public partial class Dummy : Actor
{

	[Export]
	public NodePath hitParticlesPath;
	public GpuParticles3D hitParticles;
	public Vector3 lastHitDirection;
	public Vector3 lastHitPoint;
	[Export]
	public PackedScene gibletScene;
	[Export]
	public float gibletVelocity = 4f;
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		hitParticles = GetNode<GpuParticles3D>(hitParticlesPath);
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		Velocity -= Vector3.Down * gravity * (float)delta;
		MoveAndSlide();

	}

	public override void OnHit(float damage, Vector3 hitPoint, Vector3 hitNormal, Node3D source)
	{

		lastHitDirection = hitNormal;
		lastHitPoint = hitPoint;
		base.OnHit(damage, hitPoint, hitNormal, source);
		GpuParticles3D particles = (GpuParticles3D)hitParticles.Duplicate();
		GetParent().AddChild(particles);
		particles.LookAt(hitPoint - hitNormal);
		particles.GlobalPosition = hitPoint;
		particles.Emitting = true;
		particles.Set("active", true);
		
	}

    public override void OnDeath()
    {

		base.OnDeath();
		Giblet g = gibletScene.Instantiate<Giblet>();
		GetParent().AddChild(g);
		g.GlobalPosition = GlobalPosition;
		g.GlobalRotation = GlobalRotation;
		g.ApplyImpulse(-lastHitDirection * gibletVelocity, lastHitPoint);

        QueueFree();
    }

}
