using Godot;
using System;

public partial class Giblet : RigidBody3D
{
	
	[Export]
	public Godot.Collections.Array<NodePath> meshPaths;
	public Godot.Collections.Array<MeshInstance3D> meshes = new Godot.Collections.Array<MeshInstance3D>();
	[Export]
	public float fadeTime = 10f;

    public override void _Ready()
    {
		foreach (NodePath path in meshPaths) {
			meshes.Add(GetNode<MeshInstance3D>(path));
		}
		Fade();
        base._Ready();
    }

	async void Fade() {

		await ToSignal(GetTree().CreateTimer(fadeTime - 1), "timeout");
		Tween tw = GetTree().CreateTween();
		foreach (MeshInstance3D mesh in meshes) {
			
			tw.Parallel().TweenProperty(mesh, "transparency", 1f, 1);

		}
		await ToSignal(GetTree().CreateTimer(1), "timeout");
		QueueFree();

	}

}
