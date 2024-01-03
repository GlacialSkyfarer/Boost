using Godot;
using System;

public partial class Gun : Node3D
{

	[Export]
	public NodePath animatorPath;
	public AnimationPlayer animator;
	[Export]
	public NodePath heatMeterPath;
	public TextureProgressBar heatMeter;
	[Export]
	public NodePath swapAnimatorPath;
	public AnimationPlayer swapAnimator;

	[Export]
	public float fireCooldown = 0.6f;
	[Export]
	public float altFireCooldown = 6f;

	public float fCool = 0f;
	public float altFCool = 0f;

	[Export]
	public float maxHeat = 15;
	public float heat = 0;

	public bool overheated = false;

	public float cooldownTimer = 0;

	public bool current = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		current = false;
		swapAnimator = GetNode<AnimationPlayer>(swapAnimatorPath);
		animator = GetNode<AnimationPlayer>(animatorPath);
		heatMeter = GetNode<TextureProgressBar>(heatMeterPath);
		heatMeter.MaxValue = maxHeat;
		heatMeter.Value = 0;

	}

	public virtual void SwapIn() {

		swapAnimator.Play("SwapIn");
		swapAnimator.Seek(0);

	}

    public override void _Process(double delta)
    {

		if (current) 
		{

			Visible = true;
			heatMeter.Visible = true;

		} else 
		{

			Visible = false;
			heatMeter.Visible = false;

		}

		heatMeter.Value = Mathf.Lerp(Mathf.Max(0.1f, heatMeter.Value), heat, 0.1f);
		fCool = Math.Max(0, fCool - (float)delta);
		altFCool = Math.Max(0, altFCool - (float)delta);

		if (fCool > 0) {

			cooldownTimer = 1f;

		}

		cooldownTimer -= (float)delta;
			

		if (cooldownTimer <= 0) 
		{

			heat = Math.Max(0, heat - (float)delta);

		}

		if (heat <= 0) overheated = false;

    }

    public virtual void Fire(Player player) {

		altFCool = fireCooldown;
		fCool = fireCooldown;
		animator.Seek(0);
		animator.Play("Fire");

	}

	public virtual void AltFire(Player player) {

		altFCool = altFireCooldown;
		fCool = fireCooldown;
		//Nothing in the basic class.

	}

}
