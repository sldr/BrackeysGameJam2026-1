using Godot;
using System;

public partial class Pivot : Node2D
{

    private Node2D PlayerNode;
    private Node2D childRotateNode2D;
    private Tween rotTween;
    private int rot = 0;

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
        rotTween.TweenProperty(this, "rotation_degrees", rot, 1.0f);
        rotTween.Finished += RotTween_Finished;


        //Vector2 MyGlobalPosition = this.GlobalPosition;
        //Vector2 PivotSceneRootAdjust = Vector2.Zero;
        //Vector2 NewGlobalPosition = PlayerNode.GlobalPosition;
        //PivotSceneRootAdjust.X = PlayerNode.GlobalPosition.X - MyGlobalPosition.X;
        //PivotSceneRootAdjust.Y = PlayerNode.GlobalPosition.Y - MyGlobalPosition.Y;
        //this.GlobalPosition = NewGlobalPosition;
        //NewGlobalPosition = childRotateNode2D.Position + PivotSceneRootAdjust;
        //if (left) {
        //    rot -= 90;
        //} else {
        //    rot += 90;
        //}
        //this.rotTween = CreateTween();
        //rotTween.TweenProperty(this, "rotation_degrees", rot, 1.0f);
        //this.QueueRedraw();
        //rotTween.Finished += RotTween_Finished;




        //NewGlobalPosition.X = -NewGlobalPosition.X;
        ////NewGlobalPosition.Y = -NewGlobalPosition.Y;
        //Vector2 LocalPosition = childRotateNode2D.Position;
        //this.childRotateNode2D.GlobalPosition = NewGlobalPosition;

    }

    private void RotTween_Finished()
    {
        //Vector2 swapPosition = this.Position;
        //this.Position = childRotateNode2D.Position;
        //this.childRotateNode2D.Position = Vector2.Zero - swapPosition;
        //Vector2 MyGlobalPosition = this.Position;
        //Vector2 PivotSceneRootAdjust;
        //PivotSceneRootAdjust.X = -MyGlobalPosition.X;
        //PivotSceneRootAdjust.Y = -MyGlobalPosition.Y;
        //this.Position = Vector2.Zero;
        //this.childRotateNode2D.Position = PivotSceneRootAdjust;
        GD.Print("Finished rot", this.childRotateNode2D.Position);
    }
}
