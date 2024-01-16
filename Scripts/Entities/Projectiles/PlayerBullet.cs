using Godot;
using System;

public partial class PlayerBullet : Projectile
{

	[Export]
	public PackedScene hitParticles;
	[Export]
	public float damage = 0.05f;

    public override void OnCollide()
    {
		
        base.OnCollide();

		if ((shapeCast.GetCollider(0) as CollisionObject3D).GetCollisionLayerValue(3))
		{

			Actor a = shapeCast.GetCollider(0) as Actor;

			a.OnHit(damage, shapeCast.GetCollisionPoint(0), shapeCast.GetCollisionNormal(0), this);

			QueueFree();

		}
		if ((shapeCast.GetCollider(0) as CollisionObject3D).GetCollisionLayerValue(1))
		{

			Vector3 point = shapeCast.GetCollisionPoint(0);
			GpuParticles3D particles = hitParticles.Instantiate<GpuParticles3D>();
			GetParent().AddChild(particles);
			particles.GlobalPosition = point;
			particles.Emitting = true;
			particles.LookAt(particles.GlobalPosition - velocity);

			QueueFree();

		}

	}

}
