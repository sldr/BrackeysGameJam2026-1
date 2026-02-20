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
    private const float PlayerStaminaInit = 100f;
    private bool inHazardCoolDown = false;
    private Tween hazardTween = null;
    private int killCount = 0;
    private float stamina=0f;

    [Export]
    public int PlayerHealthMax = PlayerHealthInit;

    [Export]
    public int PlayerHealthStart = PlayerHealthInit / 2;

    [Export]
    public float PlayerStaminaMax = PlayerStaminaInit;

    [Export]
    public float PlayerStaminaStart = PlayerStaminaInit;

    [Export]
    public int HazardCollLay2 = 10;

    [Export]
    public int HazardCollLay3 = 20;

    [Export]
    public int HazardCollLay4 = 50;

    [Export]
    public int EnemyHitCollLay5 = 100;

    [Export]
    public float HazardCoolDownBlink1 = 0.3f;

    [Export]
    public float HazardCoolDownBlink2 = 0.5f;


    [Signal]
    public delegate void RotateStartEventHandler(bool left = false);
    [Signal]
    public delegate void KillCountChangedEventHandler(int newCount);
    [Signal]
    public delegate void StaminaChangedEventHandler(float newStamina, float newStaminaPercentOfMax);

    private void TriggerRotateStart(bool left = false)
    {
        EmitSignal(SignalName.RotateStart, left);
    }

    public void AddKill()
    {
        killCount++;
        EmitSignal(SignalName.KillCountChanged, killCount);
    }

    public bool TryChangeStamina(float staminaChange)
    {
        if (Mathf.Abs(staminaChange) < Mathf.Epsilon) {
            return false; // Stamina Will Not Be Changed because staminaChange ~= 0.0
        }
        float newStamina = stamina + staminaChange;
        if (newStamina < 0.0f) {
            return false; // Stamina Will Not Be Changed because the staminaChange value would cause stamina to go below 0.0
        }
        if (newStamina > this.PlayerStaminaMax) {
            if (NearlyEqual(stamina, this.PlayerStaminaMax)) {
                return false; // Stamina Will Not Be Changed because stamina is already ~= PlayerStaminaMax
            }
            stamina = this.PlayerStaminaMax;
        } else {
            stamina = newStamina;
        }
        EmitSignal(SignalName.StaminaChanged, stamina, stamina * 100 / this.PlayerStaminaMax);
        return true;
    }

    public (bool success, float decreasedAmount) TryDecreaseAnyStaminaUpTo(float staminaDecreaseMax)
    {
        if (Mathf.Abs(staminaDecreaseMax) < Mathf.Epsilon) {
            return (false, 0.0f); // Stamina Will Not Be Changed because staminaDecreaseMax ~= 0.0
        }
        if (Mathf.Abs(stamina) < Mathf.Epsilon || stamina < 0.0f) {
            return (false, 0.0f); // Stamina Will Not Be Changed because stamina ~= 0.0 or less than 0.0 so none left
        }
        if (stamina > staminaDecreaseMax) {
            stamina -= staminaDecreaseMax;
            EmitSignal(SignalName.StaminaChanged, stamina, stamina * 100 / this.PlayerStaminaMax);
            return (true, staminaDecreaseMax);
        }
        float staminaDecrease = stamina;
        stamina = 0.0f;
        EmitSignal(SignalName.StaminaChanged, stamina, 0.0f);
        return (true, staminaDecrease);
    }

    public void ResetStamina()
    {
        stamina = this.PlayerStaminaMax;
        EmitSignal(SignalName.StaminaChanged, stamina, 100.0f);
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
        this.childRotateSceneTree = GetChild<Pivot>(1);
        this.childPlayer = GetChild<CharacterBody2D>(2);
        this.botBiomParallax = GetNode<Node2D>("ParallaxienNodes/BottomBiomParallax").GetNode<Parallax2D>("Parallax2D"); 
        this.rgtBiomParallax = GetNode<Parallax2D>("ParallaxienNodes/RightBiomParallax");
        this.topBiomParallax = GetNode<Parallax2D>("ParallaxienNodes/TopBiomParallax");
        this.lftBiomParallax = GetNode<Parallax2D>("ParallaxienNodes/LeftBiomParallax");
        this.hud = GetNode<Hud>("HUD");
        this.childRotateSceneTree.SetPlayer(this.childPlayer);
        enableBiom(Biom.Bottom);
        this.childRotateSceneTree.RotateFinished += ChildRotateSceneTree_RotateFinished;
        this.playerhealth = PlayerHealthStart;
        this.stamina = PlayerStaminaStart;
        EmitSignal(SignalName.StaminaChanged, stamina);
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

    public void ChangePlayerHealthFull()
    {
        this.playerhealth = PlayerHealthMax;
        this.hud.UpdateHealthPercent(playerhealth * 100 / PlayerHealthMax);
    }

    public void HazardHit(int hazardLevel)
    {
        if (inHazardCoolDown) {
            return;
        }
        inHazardCoolDown = true;
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
        hazardTween = CreateTween();
        hazardTween.SetLoops(1);
        hazardTween.TweenProperty(childPlayer, "modulate:a", 0f, this.HazardCoolDownBlink1);
        hazardTween.TweenProperty(childPlayer, "modulate:a", 1f, this.HazardCoolDownBlink1);
        hazardTween.TweenProperty(childPlayer, "modulate:a", 0f, this.HazardCoolDownBlink2);
        hazardTween.TweenProperty(childPlayer, "modulate:a", 1f, this.HazardCoolDownBlink2);
        hazardTween.Finished += HazardTween_Finished;
    }

    private void HazardTween_Finished()
    {
        hazardTween = null;
        inHazardCoolDown = false;
    }

    private bool NearlyEqual(float a, float b, float tolerance = Mathf.Epsilon)
    {
        return Mathf.Abs(a - b) < tolerance;
    }


}
