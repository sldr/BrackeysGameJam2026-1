using Godot;
using System;

public partial class RotateTrigger : StaticBody2D
{

    [Export]
    public float Height = 64.0f;
    [Export]
    public float Radius = 10.0f;

    public override void _Ready()
    {
        CollisionShape2D collisionShape2D = this.GetChild<CollisionShape2D>(0);
        this.
        if (collisionShape2D.Shape is CapsuleShape2D capsule) {
            capsule.Height = Height;
            capsule.Radius = Radius;
        }
    }

    public override void _Process(double delta)
    {

    }

}
