using Godot;
using System;

public partial class FlashOverlay : ColorRect
{
	
	public void Flash(Color from, Color to, float time = 1) {

		Color = from;

		Tween tw = GetTree().CreateTween();

		tw.TweenProperty(this, "color", to, time);

	}

}
