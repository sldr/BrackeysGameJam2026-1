using Godot;
using System;

public partial class ParallaxScene : Node2D
{
    private string parentname;
    public override void _Ready()
    {
        this.parentname = this.GetParent().Name;
        this.VisibilityChanged += ParallaxScene_VisibilityChanged;
        GD.Print("[REDY] Position ", parentname, " ", this.Name, " Visable: ", this.Visible, " Pos:", this.Position, " Scroll Offset: ", this.GetChild<Parallax2D>(0).ScrollOffset);
    }

    private void rotateFinishedHandler(bool left)
    {
        GD.Print("[RFIN] Position ", parentname, " ", this.Name, " Visable: ", this.Visible, " Pos:", this.Position, " Scroll Offset: ", this.GetChild<Parallax2D>(0).ScrollOffset);
    }

    private void ParallaxScene_VisibilityChanged()
    {
        GD.Print("[VICH] Position ", parentname, " ", this.Name, " Visable: ", this.Visible, " Pos:", this.Position, " Scroll Offset: ", this.GetChild<Parallax2D>(0).ScrollOffset);
    }

    public void setRotateFinishedHandler()
    {
        Game game = GetTree().CurrentScene as Game;
        game.AddRotateFinishedHandler(rotateFinishedHandler);
    }

    public void resetParallaxScene(Vector2 pos)
    {
        // DO NOTHING
    }



}
