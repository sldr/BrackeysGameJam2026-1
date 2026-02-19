using Godot;
using System;

public partial class Hud : CanvasLayer
{

    private ProgressBar healthProgressBar;

    [Export]
    public NodePath HealthProgressBarNodePath;

    public ProgressBar GetHealthProgressBar()
    {
        return this.GetNode<ProgressBar>(this.HealthProgressBarNodePath);
    }

    public void UpdateHealthPercent(int healthPercent)
    {
        if (healthPercent < 0) {
            healthPercent = 0;
        }
        if (healthPercent > 100) {
            healthPercent = 100;
        }
        this.healthProgressBar.Value = healthPercent;
    }

    public override void _Ready()
    {
        base._Ready();
        this.healthProgressBar = this.GetNode<ProgressBar>(this.HealthProgressBarNodePath);
    }
}
