using Godot;
using System;
using static System.Net.Mime.MediaTypeNames;

public partial class SpinSlash : Node2D
{
	private int slash_count = 0;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	private void _on_slash_area_body_entered(Node2D body)
	{
		if (body is PhysicsBody2D enemy)
		{
			if (enemy.IsInGroup("enemies"))
			{
				enemy.QueueFree();
                Game game = GetTree().CurrentScene as Game;
                if (game != null) {
                    game.AddKill();
                }

            }
        }
	}
	
}
