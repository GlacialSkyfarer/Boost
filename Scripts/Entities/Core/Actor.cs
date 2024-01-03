using Godot;
using System;
using System.Linq.Expressions;

public partial class Actor : CharacterBody3D
{
	
	public float health = 100;
    public float maxHealth = 100;

	public float gravity = -ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public virtual void OnHit(float damage, Vector3 hitPoint, Node3D source) 
    {

        health -= damage;
        if (health <= 0) 
        {
            OnDeath();
        }

    }

    public virtual void OnDeath() 
    {

        //This is where Death code would go.
        //IF I HAD SOME.

    }
    
}
