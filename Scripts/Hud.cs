using Godot;
using System;

public partial class Hud : CanvasLayer
{
    [Export]
    public NodePath ProgressBarNodePath;

    public ProgressBar GetProgressBar()
    {
        return this.GetNode<ProgressBar>(this.ProgressBarNodePath);
    }
}
