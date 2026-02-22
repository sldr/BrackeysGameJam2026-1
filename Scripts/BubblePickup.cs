using Godot;
using System;

public partial class BubblePickup : Node2D
{

    [Export]
    public GameStats.PickupTypes PickupType;

	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_area_2d_body_entered(PhysicsBody2D body)
	{
		if (body.IsInGroup("Player")) {
            Game game = GetTree().CurrentScene as Game;
            if (game != null) {
                game.ChangePlayerHealthFull(PickupType);
            }
			this.GetNode<Sprite2D>("Sprite2D").Visible = false;
            this.GetNode<GpuParticles2D>("GPUParticles2D").Emitting = true;
        }
    }

	private void _on_gpu_particles_2d_finished()
	{
		this.QueueFree();
	}
}
