using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

public partial class MapLoader : VBoxContainer
{

	[Export]
	public PackedScene buttonScene;

	public static List<string> LoadMapsFromDirectory(string path) {

		List<string> mapNames = new();

		using var dir = DirAccess.Open(path);

		if (dir != null)
		{

			dir.ListDirBegin();
			string fileName = dir.GetNext();
			while (fileName != "") {

				if (fileName.Contains(".tscn")) {
					
					if (fileName.Contains(".tscn.remap")) {

						fileName = fileName.Replace(".remap", "");

					}

					GD.Print("Found scene: " + fileName);
					mapNames.Add(fileName);
					
				} else {
					
					GD.PushWarning("File is not a scene. Skipping.");
					
				}
				
				fileName = dir.GetNext();

			}
			dir.ListDirEnd();

		}
		return mapNames;

	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		foreach (string name in LoadMapsFromDirectory("res://Scenes/Maps")) {

			MapLoaderButton b = buttonScene.Instantiate<MapLoaderButton>();
			b.sceneName = name;
			AddChild(b);

		}

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
