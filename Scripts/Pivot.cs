using Godot;
using System;

public partial class Pivot : Node2D
{

    private Node2D PlayerNode;
    private Node2D childRotateNode2D;
    private Tween rotTween = null;
    private int rot = 0;


    private float _PivotTime = 1f;

    [Export(PropertyHint.Range, "0,3")]
    public float PivotTime
    {
        get => _PivotTime;
        set
        {
            if (!Mathf.IsEqualApprox(_PivotTime, value)) {
                _PivotTime = value;
            }
        }
    }

    [Signal]
    public delegate void RotateFinishedEventHandler();

    private void TriggerRotateFinished()
    {
        EmitSignal(SignalName.RotateFinished);
    }

    public void SetPlayer(Node2D PlayerNode)
    {
        this.PlayerNode = PlayerNode;
    }

    public override void _Draw()
    {
        
    }

    public override void _Ready()
    {
        base._Ready();
        this.childRotateNode2D = this.GetChild<Node2D>(0);
    }

    public void _on_game_node_2d_rotate(bool left)
    {
        if (this.rotTween != null) {
            GD.Print("No rotate during rotate");
            return;
        }
        GD.Print("Player Position: ", PlayerNode.Position);
        Vector2 adjustPosition = this.PlayerNode.GlobalPosition - this.GlobalPosition;
        this.GlobalPosition = this.GlobalPosition + adjustPosition;
        this.childRotateNode2D.GlobalPosition = this.childRotateNode2D.GlobalPosition - adjustPosition;
        if (left) {
            rot -= 90;
        } else {
            rot += 90;
        }
        // this.RotationDegrees = rot; // Snap to rotation
        this.rotTween = CreateTween();
        rotTween.TweenProperty(this, "rotation_degrees", rot, _PivotTime);
        rotTween.Finished += RotTween_Finished;
    }

    private void RotTween_Finished()
    {
        GD.Print("Finished rot", this.childRotateNode2D.Position);
        this.rotTween = null;
        TriggerRotateFinished();
    }

    public void AddRotateFinishedHandler(RotateFinishedEventHandler anotherRotateFinishedHandler)
    {
        this.RotateFinished += anotherRotateFinishedHandler;
    }
}
