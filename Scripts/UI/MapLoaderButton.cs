using Godot;
using System;

public partial class MapLoaderButton : Button
{

	[Export]
	public string sceneName;

	public override void _Ready() {

		Text = sceneName;

	}

	public void ButtonPressed() 
	{

		GetTree().ChangeSceneToFile("res://Scenes/Maps/" + sceneName);

	}
}
