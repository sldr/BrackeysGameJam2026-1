using Godot;
using System;

public partial class EndGame : CanvasLayer
{
    [Export]
    public NodePath KillsValueLabelNodePath;
    [Export]
    public NodePath HealthRestoresValueLabelNodePath;
    [Export]
    public NodePath TimeValueLabelNodePath;
    [Export]
    public NodePath WonLabelNodePath;
    [Export]
    public NodePath LostLabelNodePath;

    public void _on_button_pressed()
    {
        GD.Print("OK button pressed");
        GetTree().ChangeSceneToFile("res://Main.tscn");
    }

    public override void _Ready()
    {
        GameStats stats = GetNode<GameStats>("/root/GameStats");
        if (stats.WonGame) {
            this.GetNode<Label>(this.LostLabelNodePath).QueueFree();
        } else {
            this.GetNode<Label>(this.WonLabelNodePath).QueueFree();
        }
        this.GetNode<Label>(this.KillsValueLabelNodePath).Text = $"{stats.Kills}";
        this.GetNode<Label>(this.HealthRestoresValueLabelNodePath).Text = $"{stats.GetHealthRestores()}";
        int mins = (int)(stats.TimeSeconds / 60);
        int secs = (int)(stats.TimeSeconds % 60);
        this.GetNode<Label>(this.TimeValueLabelNodePath).Text = $"{mins:00}:{secs:00}";
        int[] pickupCounts = stats.GetPickupCounts();
        this.GetNode<Label>("AspectRatioContainer/CenterContainer/PanelContainer/MarginContainer/VBoxContainer/MarginContainer/GridContainer/BlackHolesValue").Text = $"{pickupCounts[(int)GameStats.PickupTypes.BlackHoles]}";
        this.GetNode<Label>("AspectRatioContainer/CenterContainer/PanelContainer/MarginContainer/VBoxContainer/MarginContainer/GridContainer/FireOrbsValue").Text = $"{pickupCounts[(int)GameStats.PickupTypes.FireOrbs]}";
        this.GetNode<Label>("AspectRatioContainer/CenterContainer/PanelContainer/MarginContainer/VBoxContainer/MarginContainer/GridContainer/FireRingsValue").Text = $"{pickupCounts[(int)GameStats.PickupTypes.FireRings]}";
        this.GetNode<Label>("AspectRatioContainer/CenterContainer/PanelContainer/MarginContainer/VBoxContainer/MarginContainer/GridContainer/WaterLeafsValue").Text = $"{pickupCounts[(int)GameStats.PickupTypes.WaterLeafs]}";
        this.GetNode<Label>("AspectRatioContainer/CenterContainer/PanelContainer/MarginContainer/VBoxContainer/MarginContainer/GridContainer/BubblesValue").Text = $"{pickupCounts[(int)GameStats.PickupTypes.Bubble]}";
    }
}
