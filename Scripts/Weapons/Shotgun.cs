using Godot;
using System;

public partial class Shotgun : Gun 
{

	[ExportGroup("References")]
	
	[Export]
	public NodePath chargeAnimatorPath;
	public AnimationTree chargeAnimator;
	public AnimationNodeStateMachinePlayback chargeSM;

	[ExportGroup("Practical")]

	public int chargeLevel = 1;

	[Export]
	public PackedScene bulletScene;
	[Export]
	public float bulletSpeed = 10f;
	[Export]
	public float bulletSpread = 20f;
	[Export]
	public uint bulletCount = 10;

	[Export(PropertyHint.File)]
	public string fireSound;
	[Export(PropertyHint.File)]
	public string overheatSound;

	[Export]
	public float fireHeat = 5f;

	[Export]
	public float baseKickback = 5f;
	[Export]
	public float secondLevelKickback = 10f;
	[Export]
	public float thirdLevelKickback = 15f;

	SoundManager soundManager;

	public override void _Ready()
	{
		base._Ready();
		soundManager = GetTree().GetFirstNodeInGroup("SoundManager") as SoundManager;
		chargeAnimator = GetNode<AnimationTree>(chargeAnimatorPath);
		chargeSM = (AnimationNodeStateMachinePlayback)chargeAnimator.Get("parameters/playback");
	}

	public override void Fire(Player player)
	{

		base.Fire(player);

		soundManager.PlayDirectionlessSound(new Sound(fireSound, 0, GlobalPosition));

		heat += fireHeat * chargeLevel;
		if (heat >= maxHeat)
		{
			heat = maxHeat;
			overheated = true;
			soundManager.PlayDirectionlessSound(new Sound(overheatSound, 0, GlobalPosition));
		}

		for (int i = 0; i < (bulletCount + (chargeLevel - 1) * (bulletCount / 2)); i++)
		{

			PlayerBullet p = bulletScene.Instantiate<PlayerBullet>();
			player.GetParent().AddChild(p);
			p.GlobalPosition = GlobalPosition;
			float currentSpread = bulletSpread / (1 + ((chargeLevel - 1) / 2));
			float randX = Mathf.DegToRad((float)GD.RandRange(-currentSpread, currentSpread));
			float randY = Mathf.DegToRad((float)GD.RandRange(-currentSpread, currentSpread));
			p.velocity = (-GlobalTransform.Basis.Z).Rotated(GlobalTransform.Basis.X, randX).Rotated(GlobalTransform.Basis.Y, randY) * bulletSpeed * (1 + ((chargeLevel - 1) / 2));

		}

		float kickback = 0;

			switch (chargeLevel)
			{

				case 1:

					kickback = baseKickback;
					break;

				case 2:

					kickback = secondLevelKickback;
					break;

				case 3:

					kickback = thirdLevelKickback;
					break;

			}

		player.Velocity += GlobalTransform.Basis.Z * kickback;

		chargeLevel = 1;

		chargeSM.Travel("Transition1");

	}

	public override void AltFire(Player player)
	{

		if (chargeLevel < 3) 
		{

			base.AltFire(player);

			animator.Play("AltFire");
			animator.Seek(0);

			chargeLevel++;

			switch (chargeLevel) 
			{

				case 2:

					chargeSM.Travel("Transition2");

					break;

				case 3:

					chargeSM.Travel("Transition3");

					break;

			}

		}

	}

	public override void SwapIn()
	{
		base.SwapIn();
		chargeSM.Travel("Idle1");
		chargeLevel = 1;
	}

    public override void _Process(double delta)
    {
        base._Process(delta);

		if (overheated) {

			heatMeter.TintProgress = Colors.Red;

		} else {

			heatMeter.TintProgress = Colors.White;

		}
    }

}
