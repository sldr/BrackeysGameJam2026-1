using Godot;
using System;

public partial class Pivot : Node2D
{

    private Node2D PlayerNode;
    private Node2D childRotateNode2D;
    private Tween rotTween = null;
    private int rot = 0;
    private float _SideLength = 14000f;
    private bool _NeedsApplySideLength = true;
    private float _PivotTime = 1f;
    private bool rotatedLeft = false;

    [Export(PropertyHint.Range, "0,999999")]
    public float SideLength
    {
        get => _SideLength;
        set
        {
            if (!Mathf.IsEqualApprox(_SideLength, value)) {
                _SideLength = value;
                // IMPORTANT: guard against scene not being ready yet
                if (!IsInsideTree()) {
                    this._NeedsApplySideLength = true;
                    return;
                }
                ApplySideLength();
            }
        }
    }

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
    public delegate void RotateFinishedEventHandler(bool left);

    private void TriggerRotateFinished()
    {
        EmitSignal(SignalName.RotateFinished, this.rotatedLeft);
    }


    private void ApplySideLength()
    {
        Line2D lines = this.GetNode<Line2D>("SceneRootNode2D/WallsLine2D");
        Vector2[] points = lines.Points;
        points[1] = new Vector2(this._SideLength, 0);
        points[2] = new Vector2(this._SideLength, this._SideLength);
        points[3] = new Vector2(0, this._SideLength);
        lines.Points = points;
        lines.GetChild<StaticBody2D>(0).Position = new Vector2(0, this._SideLength);
        lines.GetChild<StaticBody2D>(1).Position = new Vector2(0, 0);
        lines.GetChild<StaticBody2D>(2).Position = new Vector2(this._SideLength, 0);
        lines.GetChild<StaticBody2D>(3).Position = new Vector2(0, 0);
        this.QueueRedraw();
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
        if (_NeedsApplySideLength) {
            ApplySideLength();
        }
    }

    public void _on_game_node_2d_rotate_start(bool left)
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
        this.rotatedLeft = left;
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

    public Vector2 GetTopLeftGlobalPosition()
    {
        GD.Print("GetTopLeftGlobalPosition: ", this.childRotateNode2D.GlobalPosition);
        return this.childRotateNode2D.GlobalPosition;
    }
}
