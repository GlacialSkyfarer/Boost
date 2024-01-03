using Godot;
using System;

public partial class Shotgun : Gun 
{

    [Export]
    public PackedScene bulletScene;
    [Export]
    public float bulletSpeed = 10f;
    [Export]
    public float bulletSpread = 20f;
    [Export]
    public uint bulletCount = 10;

    [Export(PropertyHint.File)]
    public string fireSound;

    [Export]
    public float fireHeat = 5f;

    [Export]
    public float baseKickback = 5f;

    SoundManager soundManager;

    public override void _Ready()
    {
        base._Ready();
        soundManager = GetTree().GetFirstNodeInGroup("SoundManager") as SoundManager;
    }

    public override void Fire(Player player)
    {

        base.Fire(player);

        soundManager.PlayDirectionlessSound(new Sound(fireSound, 0, GlobalPosition));

        heat += fireHeat;
        if (heat >= maxHeat)
        {
            heat = maxHeat;
            overheated = true;
        }

        for (int i = 0; i < bulletCount; i++)
        {

            PlayerBullet p = bulletScene.Instantiate<PlayerBullet>();
            player.GetParent().AddChild(p);
            p.GlobalPosition = GlobalPosition;
            float randX = Mathf.DegToRad((float)GD.RandRange(-bulletSpread, bulletSpread));
            float randY = Mathf.DegToRad((float)GD.RandRange(-bulletSpread, bulletSpread));
            p.velocity = (-GlobalTransform.Basis.Z).Rotated(GlobalTransform.Basis.X, randX).Rotated(GlobalTransform.Basis.Y, randY) * bulletSpeed;

        }

        player.Velocity += GlobalTransform.Basis.Z * baseKickback;

    }

    public override void AltFire(Player player)
    {

        base.AltFire(player);

        animator.Play("AltFire");

    }

}