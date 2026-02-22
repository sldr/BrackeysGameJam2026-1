using Godot;
using System;

public partial class Music : AudioStreamPlayer
{

	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		Game game = GetTree().CurrentScene as Game;
        if (game != null) {
            game.BiomChanged += biome;
        }
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Game game = GetTree().CurrentScene as Game;
        if (game != null) {
            game.BiomChanged += biome;

        }
	}

	public void biome(int biomeChanged)
	{
		Game game = GetTree().CurrentScene as Game;
		switch (Game.intToBiom(biomeChanged)) {
			case Game.Biom.Bottom:
				Set("parameters/switch_to_clip", "Moonloop Final");
				break;
			case Game.Biom.Right:
				Set("parameters/switch_to_clip", "Fireloop Final");
				break;
			case Game.Biom.Top:
				Set("parameters/switch_to_clip", "Waterloop Final");
				break;
			case Game.Biom.Left:
				Set("parameters/switch_to_clip", "Forestloop Final");
				break;
		}
		
	}
}
