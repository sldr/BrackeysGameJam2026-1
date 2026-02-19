using Godot;
using System;
using System.Runtime.InteropServices.JavaScript;
using static Pivot;

public partial class Game : Node2D
{

    public enum Biom
    {
        Bottom = 0,
        Right = 1,
        Top = 2,
        Left = 3,
        None = 4
    }

    private Pivot childRotateSceneTree;
    private CharacterBody2D childPlayer;
    private Parallax2D botBiomParallax;
    private Parallax2D rgtBiomParallax;
    private Parallax2D topBiomParallax;
    private Parallax2D lftBiomParallax;
    private Biom currentBiom = Biom.Bottom;
    private Hud hud;
    private int playerhealth;
    private const int PlayerHealthInit = 500;

    [Export]
    public int PlayerHealthMax = PlayerHealthInit;

    [Export]
    public int PlayerHealthStart = PlayerHealthInit;

    [Export]
    public int HazardCollLay2 = 10;

    [Export]
    public int HazardCollLay3 = 20;

    [Export]
    public int HazardCollLay4 = 50;

    [Export]
    public int EnemyHitCollLay5 = 100;


    [Signal]
    public delegate void RotateStartEventHandler(bool left = false);

    private void TriggerRotateStart(bool left = false)
    {
        EmitSignal(SignalName.RotateStart, left);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        // TriggerRotate(true);
        if (Input.IsActionJustPressed("pivotright")) {
            GD.Print("Player Position: ", childPlayer.Position);
            GD.Print("Pivot Right pressed this frame");
            TriggerRotateStart();
        }
        if (Input.IsActionJustPressed("pivotleft")) {
            GD.Print("Player Position: ", childPlayer.Position);
            GD.Print("Pivot Left pressed this frame");
            TriggerRotateStart(true);
        }
    }

    public override void _Ready()
    {
        base._Ready();
        this.childRotateSceneTree = GetChild<Pivot>(0);
        this.childPlayer = GetChild<CharacterBody2D>(1);
        this.botBiomParallax = GetNode<Parallax2D>("ParallaxienNodes/BottomBiomParallax");
        this.rgtBiomParallax = GetNode<Parallax2D>("ParallaxienNodes/RightBiomParallax");
        this.topBiomParallax = GetNode<Parallax2D>("ParallaxienNodes/TopBiomParallax");
        this.lftBiomParallax = GetNode<Parallax2D>("ParallaxienNodes/LeftBiomParallax");
        this.hud = GetNode<Hud>("HUD");
        this.childRotateSceneTree.SetPlayer(this.childPlayer);
        enableBiom(Biom.Bottom);
        this.childRotateSceneTree.RotateFinished += ChildRotateSceneTree_RotateFinished;
        this.playerhealth = PlayerHealthStart;
        this.hud.UpdateHealthPercent(playerhealth * 100 / PlayerHealthMax);
    }

    private void ChildRotateSceneTree_RotateFinished(bool left)
    {
        if (left) {
            this.currentBiom -= 1;
        } else {
            this.currentBiom += 1;
        }
        enableBiom(this.currentBiom);
    }

    public void AddRotateFinishedHandler(RotateFinishedEventHandler anotherRotateFinishedHandler)
    {
        if (anotherRotateFinishedHandler == null) {
            this.childRotateSceneTree.AddRotateFinishedHandler(anotherRotateFinishedHandler);
        }
    }

    public void enableBiom(Biom biom)
    {
        switch (biom) {
            case Biom.Bottom:
                GD.Print("Bottom Biom Enabled");
                this.rgtBiomParallax.Visible = false;
                this.topBiomParallax.Visible = false;
                this.lftBiomParallax.Visible = false;
                this.botBiomParallax.Visible = true;
                this.botBiomParallax.ScrollOffset = this.childRotateSceneTree.GetTopLeftGlobalPosition();
                break;
            case Biom.Right:
                GD.Print("Right Biom Enabled");
                this.botBiomParallax.Visible = false;
                this.topBiomParallax.Visible = false;
                this.lftBiomParallax.Visible = false;
                this.rgtBiomParallax.Visible = true;
                this.rgtBiomParallax.ScrollOffset = this.childRotateSceneTree.GetTopLeftGlobalPosition();
                break;
            case Biom.Top:
                GD.Print("Top Biom Enabled");
                this.botBiomParallax.Visible = false;
                this.rgtBiomParallax.Visible = false;
                this.lftBiomParallax.Visible = false;
                this.topBiomParallax.Visible = true;
                break;
            case Biom.Left:
                GD.Print("Left Biom Enabled");
                this.botBiomParallax.Visible = false;
                this.rgtBiomParallax.Visible = false;
                this.topBiomParallax.Visible = false;
                this.lftBiomParallax.Visible = true;
                break;
            case Biom.None:
            default:
                GD.Print("Bioms Disabled");
                this.botBiomParallax.Visible = false;
                this.rgtBiomParallax.Visible = false;
                this.topBiomParallax.Visible = false;
                this.lftBiomParallax.Visible = false;
                break;
        }
    }

    public void ChangePlayerHealth(int change)
    {
        if (change == 0) {
            return;
        }
        this.playerhealth += change;
        if (playerhealth > PlayerHealthMax) {
            this.playerhealth = PlayerHealthMax;
        }
        if (playerhealth < 0) {
            this.playerhealth = 0;
        }
        this.hud.UpdateHealthPercent(playerhealth * 100 / PlayerHealthMax);
    }

    public void HazardHit(int hazardLevel)
    {
        switch (hazardLevel) {
            case 1:
                ChangePlayerHealth(-this.HazardCollLay2);
                break;
            case 2:
                ChangePlayerHealth(-this.HazardCollLay3);
                break;
            case 3:
                ChangePlayerHealth(-this.HazardCollLay4);
                break;
            case 0:
            default:
                return;
        }
    }

}
