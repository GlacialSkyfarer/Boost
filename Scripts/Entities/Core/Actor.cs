using Godot;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

public partial class Actor : CharacterBody3D
{

	public float gravity = -ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public virtual void _func_godot_apply_properties(Godot.Collections.Dictionary<string, Variant> properties) {

        foreach (KeyValuePair<string, Variant> property in properties) {

            if (!this.Get(property.Key).Equals(null)) {

                this.Set(property.Key, property.Value);

            }

        }

    }

    public virtual void _func_godot_build_complete(Godot.Collections.Dictionary<string, Variant> properties) {

        foreach (KeyValuePair<string, Variant> property in properties) {

            if (!this.Get(property.Key).Equals(null)) {

                this.Set(property.Key, property.Value);

            }

        }

    }
    
}
