using Godot;
using System;

public partial class ViewportShareWorld : SubViewport
{

	[Export]
	public NodePath targetVp;
	Viewport targetVpView;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		targetVpView = GetNode<Viewport>(targetVp);
		World3D = targetVpView.World3D;

	}

}
