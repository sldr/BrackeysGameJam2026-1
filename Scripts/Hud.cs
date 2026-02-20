using Godot;
using System;

public partial class Hud : CanvasLayer
{

    private ProgressBar healthProgressBar;
    private ProgressBar staminaProgressBar;
    private Label killLabel;
    private int PlayerStaminaMax;


    [Export]
    public NodePath HealthProgressBarNodePath;
    [Export]
    public NodePath KillCountLabelNodePath;
    [Export]
    public NodePath StaminaPercentNodePath;

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
        this.staminaProgressBar = this.GetNode<ProgressBar>(this.StaminaPercentNodePath);
        this.killLabel = this.GetNode<Label>(this.KillCountLabelNodePath);
        Game game = GetTree().CurrentScene as Game;
        if (game != null) {
            game.KillCountChanged += Game_KillCountChanged;
            game.StaminaChanged += Game_StaminaChanged;
            this.PlayerStaminaMax = game.PlayerStaminaMax;
        }
    }

    private void Game_StaminaChanged(int newStamina)
    {
        staminaProgressBar.Value = newStamina * 100 / this.PlayerStaminaMax;
    }

    private void Game_KillCountChanged(int newCount)
    {
        killLabel.Text = $"{newCount} KILLS";
    }
}
