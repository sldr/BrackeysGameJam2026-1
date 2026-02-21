using Godot;
using System;

public partial class EndTrigger : Node2D
{

    public void _on_area_2d_body_entered(Node2D body)
    {
        if (body is CharacterBody2D player && player.IsInGroup("Player")) {
            GD.Print("Player triggered end game");
            GetTree().ChangeSceneToFile("res://EndGame.tscn");
        }
    }

}
