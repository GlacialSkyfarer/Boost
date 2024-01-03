using Godot;
using System;

public partial class PlayerBullet : Projectile
{

	[Export]
	public PackedScene hitParticles;

    public override void OnCollide()
    {
		
        base.OnCollide();

		if ((shapeCast.GetCollider(0) as Node).IsInGroup("Enemy"))
		{



		}
		if ((shapeCast.GetCollider(0) as Node).IsInGroup("Ground"))
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
