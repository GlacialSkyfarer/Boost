using Godot;
using System;

public partial class DamageIndicator : Label3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CallDeferred("Trigger");
	}

	async void Trigger() {

		Tween tw = GetTree().CreateTween();
		tw.SetTrans(Tween.TransitionType.Sine).TweenProperty(this, "position", GlobalPosition + Vector3.Up, 1f);
		tw.Parallel().TweenProperty(this, "transparency", 1f, 1f);

		await ToSignal(tw, "finished");
		QueueFree();

	}

}
