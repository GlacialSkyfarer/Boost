using Godot;
using System;
public partial class LivingActor : Actor {

    public float health = 5;
    [Export] public float maxHealth = 5;
    public bool isDead = false;

    public override void _func_godot_build_complete(Godot.Collections.Dictionary<string, Variant> properties) {

        base._func_godot_build_complete(properties);
        health = maxHealth;

    }

    public virtual void OnHit(float damage, Vector3 hitPoint, Vector3 hitNormal, Node3D source) 
    {

        health -= damage;
        if (health <= 0 && !isDead) 
        {
            OnDeath();
        }

    }

    public virtual void OnDeath() 
    {

        isDead = true;

        //This is where Death code would go.
        //IF I HAD SOME.

    }


}