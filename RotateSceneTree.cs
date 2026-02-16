using Godot;
using System;

public partial class RotateSceneTree : Node2D
{
    private Node2D PlayerNode;
    private Node2D childRotateNode2D;

    [Export]
    bool Rotating = false;

    public void SetPlayer(Node2D PlayerNode)
    {
        this.PlayerNode = PlayerNode;
    }

    public void _on_game_node_2d_rotate(bool left)
    {
        int a = 0;
    }

    public override void _Process(double delta)
    {
        if (PlayerNode != null) {
            //Vector2 NewGlobalPosition = PlayerNode.GlobalPosition;
            //this.GlobalPosition = NewGlobalPosition;
            //NewGlobalPosition.X = -NewGlobalPosition.X;
            ////NewGlobalPosition.Y = -NewGlobalPosition.Y;
            //Vector2 LocalPosition = childRotateNode2D.Position;
            //this.childRotateNode2D.GlobalPosition = NewGlobalPosition;
        }
    }

    public override void _Ready()
    {
        base._Ready();
        this.childRotateNode2D =  this.GetChild<Node2D>(0);
    }
}
