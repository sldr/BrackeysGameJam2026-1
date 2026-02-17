using Godot;
using System;
using static Pivot;

public partial class Game : Node2D
{
    private Pivot childRotateSceneTree;
    private CharacterBody2D childPlayer;

    [Signal]
    public delegate void RotateEventHandler(bool left = false);

    private void TriggerRotate(bool left = false)
    {
        EmitSignal(SignalName.Rotate, left);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        // TriggerRotate(true);
        if (Input.IsActionJustPressed("pivotright")) {
            GD.Print("Player Position: ", childPlayer.Position);
            GD.Print("Pivot Right pressed this frame");
            TriggerRotate();
        }
        if (Input.IsActionJustPressed("pivotleft")) {
            GD.Print("Player Position: ", childPlayer.Position);
            GD.Print("Pivot Left pressed this frame");
            TriggerRotate(true);
        }
    }

    public override void _Ready()
    {
        base._Ready();
        this.childRotateSceneTree = GetChild<Pivot>(0);
        this.childPlayer = GetChild<CharacterBody2D>(1);
        this.childRotateSceneTree.SetPlayer(this.childPlayer);
    }

    public void AddRotateFinishedHandler(RotateFinishedEventHandler anotherRotateFinishedHandler)
    {
        if (anotherRotateFinishedHandler == null) {
            this.childRotateSceneTree.AddRotateFinishedHandler(anotherRotateFinishedHandler);
        }
    }
}
