using Godot;
using System;

public partial class Hud : CanvasLayer
{
    [Export]
    public NodePath HealthProgressBarNodePath;

    public ProgressBar GetHealthProgressBar()
    {
        return this.GetNode<ProgressBar>(this.HealthProgressBarNodePath);
    }
}
