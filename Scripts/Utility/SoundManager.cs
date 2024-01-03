using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Sound 
{

    public string path;
    public float volume;
    public Vector3 position;

    public Sound(string path, float volume, Vector3 position) {
        this.path = path;
        this.position = position;
    }

}

public partial class SoundManager : Node3D
{

    [Export]
    public int playerCount = 32;
    [Export]
    public int directionlessPlayerCount = 6;
    [Export]
    public string bus = "master";

    List<AudioStreamPlayer3D> available = new List<AudioStreamPlayer3D>();
    List<Sound> queue = new List<Sound>();
    List<AudioStreamPlayer> dAvailable = new List<AudioStreamPlayer>();
    List<Sound> dQueue = new List<Sound>();

    AudioStreamPlayer mainMusic;
    AudioStreamPlayer fadeMusic;

    public override void _Ready() {

        for (int i = 0; i < playerCount; i++) {

            AudioStreamPlayer3D p = new AudioStreamPlayer3D();
            AddChild(p);
            available.Add(p);
            p.Bus = bus;
            p.Finished += () => available.Add(p);  

        }
        for (int i = 0; i < directionlessPlayerCount; i++) {

            AudioStreamPlayer p = new AudioStreamPlayer();
            AddChild(p);
            dAvailable.Add(p);
            p.Bus = bus;
            p.Finished += () => dAvailable.Add(p);  

        }

        mainMusic = new AudioStreamPlayer();
        AddChild(mainMusic);
        mainMusic.Bus = bus;
        fadeMusic = new AudioStreamPlayer();
        AddChild(fadeMusic);
        fadeMusic.Bus = bus;

    }

    public void PlaySound(Sound s) {

        queue.Add(s);

    }

    Tween tw;

    public async void PlayMusic(Sound s) {

        if (mainMusic.Stream == null || s.path == "Silence" || !mainMusic.Stream.Equals(GD.Load<AudioStream>(s.path))) {

            if (tw != null) tw.Kill();
            fadeMusic.Stream = mainMusic.Stream;
            fadeMusic.VolumeDb = mainMusic.VolumeDb;
            fadeMusic.Play(mainMusic.GetPlaybackPosition());
            tw = GetTree().CreateTween();
            mainMusic.VolumeDb = -40;
            tw.TweenProperty(fadeMusic, "volume_db", -40, 5);
            tw.Parallel().TweenProperty(mainMusic, "volume_db", s.volume, 5);
            if (s.path != "Silence") {

                mainMusic.Stream = GD.Load<AudioStream>(s.path);
                mainMusic.Play(0);

            } else {

                mainMusic.Stream = null;
                mainMusic.Stop();

            }

            await ToSignal(GetTree().CreateTimer(10), "timeout");
            fadeMusic.Stop();

        }

    }

    public void PlayDirectionlessSound(Sound s) {

        dQueue.Add(s);

    }

    public override void _Process(double delta)
    {
        
        if (queue.Count > 0 && available.Count > 0) {

            available[0].VolumeDb = queue[0].volume;
            available[0].GlobalPosition = queue[0].position;
            available[0].Stream = GD.Load<AudioStream>(queue[0].path);
            queue.RemoveAt(0);
            available[0].Play(0);
            available.RemoveAt(0);

        }

        if (dQueue.Count > 0 && dAvailable.Count > 0) {

            dAvailable[0].VolumeDb = dQueue[0].volume;
            dAvailable[0].Stream = GD.Load<AudioStream>(dQueue[0].path);
            dQueue.RemoveAt(0);
            dAvailable[0].Play(0);
            dAvailable.RemoveAt(0);

        }

    }

}
