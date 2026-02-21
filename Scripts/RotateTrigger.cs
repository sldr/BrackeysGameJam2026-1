using Godot;
using System;

[Tool]
public partial class RotateTrigger : Area2D
{

    private float _Height = 700.0f;
    //private float _Radius = 10.0f;
    private bool _NeedsApplyHR = false;

    [Export]
    public float Radius = 10.0f;
    [Export(PropertyHint.Range, "0,999")]
    public float Height
    {
        get => _Height;
        set
        {
            if (!Mathf.IsEqualApprox(_Height, value)) {
                _Height = value;
                // IMPORTANT: guard against scene not being ready yet
                if (!IsInsideTree()) {
                    this._NeedsApplyHR = true;
                    return;
                }
                ApplyHR();
            }
        }
    }

    private void ApplyHR()
    {
        CollisionShape2D collisionShape2D = this.GetNode<CollisionShape2D>("CollisionShape2D");
        if (collisionShape2D.Shape is CapsuleShape2D capsule) {
            capsule.Height = _Height;
            capsule.Radius = Radius;
        }
        this._NeedsApplyHR = false;
        this.QueueRedrawRecursive(this);
    }

    public override void _Ready()
    {
        this.BodyEntered += Area2D_BodyEntered;
        if (_NeedsApplyHR) {
            ApplyHR();
        }
    }

    public override void _Process(double delta)
    {
        if (Engine.IsEditorHint())
            QueueRedrawRecursive(this);
    }

    private void QueueRedrawRecursive(Node node)
    {
        if (node is CanvasItem canvasItem)
            canvasItem.QueueRedraw();
        foreach (Node child in node.GetChildren())
            QueueRedrawRecursive(child);
    }

    private void Area2D_BodyEntered(Node2D body)
    {
        if (body is CharacterBody2D player) {
            if (player.IsInGroup("Player")) {
                GD.Print("Player triggered rotate");
                Game game = GetTree().CurrentScene as Game;
                if (game != null) {
                    GD.Print("Calling TriggerRotateStart");
                    game.TriggerRotateStart();
                    this.BodyEntered -= Area2D_BodyEntered;
                    this.QueueFree(); // Can only pass thru once
                }
            }
        }
    }

}
