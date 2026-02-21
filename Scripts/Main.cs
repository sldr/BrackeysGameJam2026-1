using Godot;
using System;

public partial class Main : Node2D
{
    public void _on_play_pressed()
    {
        GD.Print ("Play button pressed");
        GameStats stats = GetNode<GameStats>("/root/GameStats");
        stats.WonGame = false;
        stats.Kills = 0;
        stats.TimeSeconds = 0;
        GetTree().ChangeSceneToFile("res://Game.tscn");
    }

    public void _on_options_pressed()
    {
        GD.Print ("Options button pressed");
        GetTree().ChangeSceneToFile("res://Options.tscn");
    }

    public void _on_quit_pressed()
    {
        GD.Print ("Quit button pressed");
        GetTree().Quit();
    }
}
