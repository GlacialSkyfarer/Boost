using Godot;
using System;

public partial class Line3D : Node3D
{

	[Export]
	public Godot.Collections.Array<NodePath> linePaths;

	public Godot.Collections.Array<MeshInstance3D> lines;

	[Export]
	public Color color;
	[Export]
	public Vector3 target;

	[Export]
	public bool fade = true;
	[Export]
	public float fadeTime = 0.2f;

	float size = 1;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		Setup();

	}

	async void Setup() {

		await ToSignal(GetTree().CreateTimer(0.01f), "timeout");

		LookAt(target);
		lines = new Godot.Collections.Array<MeshInstance3D>();
		foreach (NodePath path in linePaths)
		{
			lines.Add(GetNode<MeshInstance3D>(path));
		}

		foreach (MeshInstance3D line in lines)
		{

			StandardMaterial3D mat = line.Mesh.SurfaceGetMaterial(0).Duplicate() as StandardMaterial3D;
			mat.AlbedoColor = color;
			mat.Emission = color;

			line.Mesh.SurfaceSetMaterial(0, mat);

			QuadMesh m = line.Mesh as QuadMesh;
			m.Size = new Vector2(0.1f, GlobalPosition.DistanceTo(target));
			m.CenterOffset = new Vector3(0, GlobalPosition.DistanceTo(target) / 2, 0);

		}

		await ToSignal(GetTree().CreateTimer(fadeTime), "timeout");
		QueueFree();

	}

    public override void _Process(double delta)
    {
        
		size = Mathf.MoveToward(size, 0, (float)delta / fadeTime);

		foreach (MeshInstance3D line in lines)
		{
			
			QuadMesh m = line.Mesh as QuadMesh;
			m.Size = new(size * 0.1f, m.Size.Y);

		}

    }

}
