using Godot;
using System;
using System.Runtime.InteropServices.JavaScript;

public partial class HatGuyEnemy : StaticBody2D
{
    [Export]
    public int Damage = 50;

    public void _on_area_2d_body_entered(Node2D body)
    {
        if (body.IsInGroup("Player")) {
            Game game = GetTree().CurrentScene as Game;
            if (game != null) {
                game.ChangePlayerHealth(-Damage);
            }
        }
    }
}
