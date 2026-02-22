using Godot;
using System;

public partial class EndTrigger : Node2D
{
    private Game game;

    public void _on_area_2d_body_entered(Node2D body)
    {
        if (game == null) {
            game = GetTree().CurrentScene as Game;
        }
        if (body is CharacterBody2D player && player.IsInGroup("Player") && game.GetCurrentBiom() == Game.Biom.Left) {
            GD.Print("Player triggered end game WIN");
            game.TriggerGameEnded();
        }
    }

}
