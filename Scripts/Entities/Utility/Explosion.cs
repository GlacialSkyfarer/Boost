using Godot;
using System;

public partial class Explosion : GpuParticles3D
{

	[Export]
	public float time = 1;
	[Export]
	public CurveTexture scaleCurve;
	[Export]
	public GradientTexture1D colorGradient;

	SphereMesh mesh;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		Destroy();

	}

	async void Destroy() {

		await ToSignal(GetTree().CreateTimer(0.01f), "timeout");

		ParticleProcessMaterial pMat = ProcessMaterial.Duplicate() as ParticleProcessMaterial;
		pMat.ScaleCurve = scaleCurve;
		pMat.ColorRamp = colorGradient;

		ProcessMaterial = pMat;

		Lifetime = time;

		Emitting = true;

		Tween tw = GetTree().CreateTween();
		tw.TweenProperty(this, "transparency", 1, time);

		await ToSignal(GetTree().CreateTimer(time), "timeout");
		QueueFree();

	}

}
