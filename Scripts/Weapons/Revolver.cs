using Godot;
using System;
using System.Transactions;

public partial class Revolver : Gun
{

	#region Variables

	[ExportGroup("References")]
	[Export]
	public NodePath warpAnimator;
	public AnimationTree warpAnimatorTree;
	[Export]
	public NodePath overheatAnimatorPath;
	public AnimationPlayer overheatAnimator;
	[Export]
	public NodePath rayCastPath;
	public RayCast3D rayCast;
	[Export]
	public NodePath rayOriginPath;
	public Node3D rayOrigin;

	SoundManager soundManager;

	FlashOverlay flash;

	[ExportGroup("Practical")]

	[Export]
	public PackedScene warpVoidScene;
	[Export]
	public float warpVoidVelocity = 10;
	WarpVoid currentVoid;

	[Export]
	public float fireHeat = 1f;
	[Export]
	public float altFireHeat = 7.5f;

	[ExportGroup("Fire Effects")]
	[Export]
	public PackedScene lineScene;
	[Export]
	public Color mainLineColor = Colors.Yellow;
	[Export]
	public PackedScene hitParticles;

	[ExportGroup("Sounds")]

	[Export(PropertyHint.File)]
	public string fireSound;

	[Export(PropertyHint.File)]
	public string altFireSound;

	[Export(PropertyHint.File)]
	public string overheatSound;

	#endregion

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		base._Ready();
		
		overheatAnimator = GetNode<AnimationPlayer>(overheatAnimatorPath);
		soundManager = GetTree().GetFirstNodeInGroup("SoundManager") as SoundManager;
		warpAnimatorTree = GetNode<AnimationTree>(warpAnimator);
		rayCast = GetNode<RayCast3D>(rayCastPath);
		rayOrigin = GetNode<Node3D>(rayOriginPath);
		flash = GetTree().GetFirstNodeInGroup("FlashOverlay") as FlashOverlay;

	}

    public override void _Process(double delta)
    {

        base._Process(delta);

		if (overheated) {

			heatMeter.TintProgress = Colors.Red;

			if (heat <= 1f) {

				overheatAnimator.Play("CooldownReset");

			} else {
	
				overheatAnimator.Play("Cooldown");

			}

		} else {

			heatMeter.TintProgress = Colors.White;

		}

    }

    public override void Fire(Player player) {

		base.Fire(player);

		heat += fireHeat;
		if (heat >= maxHeat)  
		{
			
			heat = maxHeat;

			overheated = true;
			soundManager.PlayDirectionlessSound(new(overheatSound, 0, GlobalPosition));

		}
		soundManager.PlayDirectionlessSound(new Sound(fireSound, 0, GlobalPosition));

		Line3D line = lineScene.Instantiate<Line3D>();

		player.GetParent().AddChild(line);

		line.GlobalPosition = rayOrigin.GlobalPosition;
		line.color = mainLineColor;

		if (rayCast.IsColliding()) {

			Node3D col = rayCast.GetCollider() as Node3D;
			
			if (col.IsInGroup("WarpVoid")) {

				player.GlobalPosition = rayCast.GetCollisionPoint();
				WarpVoid wv = col as WarpVoid;

				player.Velocity += wv.velocity;

				wv.Destroy();
				currentVoid = null;

				flash.Flash(Colors.Black, new(0,0,0,0), 0.5f);
			} else {

				line.target = rayCast.GetCollisionPoint();

				GpuParticles3D hitParticlesInstance = hitParticles.Instantiate<GpuParticles3D>();
				player.GetParent().AddChild(hitParticlesInstance);
				hitParticlesInstance.GlobalPosition = rayCast.GetCollisionPoint();
				hitParticlesInstance.LookAt(rayOrigin.GlobalPosition);
				hitParticlesInstance.Emitting = true;

			}

		} else {

			line.target = GlobalPosition + -GlobalTransform.Basis.Z * rayCast.TargetPosition.Length();

		}

	}

    public override void AltFire(Player player)
    {
        
		base.AltFire(player);

		animator.Play("WarpFire");
		soundManager.PlayDirectionlessSound(new Sound(altFireSound, 0, GlobalPosition));

		heat += altFireHeat;
		if (heat >= maxHeat)  
		{
			
			heat = maxHeat;

			overheated = true;

			soundManager.PlayDirectionlessSound(new(overheatSound, 0, GlobalPosition));

		}

		AnimationNodeStateMachinePlayback stateMachine = (AnimationNodeStateMachinePlayback)warpAnimatorTree.Get("parameters/playback");

		stateMachine.Travel("Recharge");

		WarpVoid warpVoid = warpVoidScene.Instantiate<WarpVoid>();
		player.GetParent().AddChild(warpVoid);
		warpVoid.GlobalPosition = GlobalPosition + -GlobalTransform.Basis.Z * 0.3f;

		warpVoid.velocity = -GlobalTransform.Basis.Z * warpVoidVelocity;

		if (currentVoid != null && !currentVoid.dead) currentVoid.Destroy();

		currentVoid = warpVoid;

    }

}
