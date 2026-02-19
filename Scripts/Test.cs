using Godot;

/// <summary>
/// Main player controller that extends StateManager.
/// Handles physics constants (editor tunable) and state transition logic.
/// </summary>
public partial class Test : StateManager
{
    // Physics Constants (Editor Tunable)
    [Export] public float Gravity = 1200f;
    [Export] public float RunSpeed = 200f;
    [Export] public float JumpVelocity = -400f;
    [Export] public float DashSpeed = 300f;
    [Export] public float DashDuration = 0.15f;
    [Export] public float GlideHorizontalSpeed = 150f;
    [Export] public float GlideDriftSmoothness = 0.15f;
    [Export] public float GlideAscendRate = -200f;
    [Export] public float GlideDescendRate = 100f;
    [Export] public float FlapStaminaMax = 100f;
    [Export] public float FlapStaminaDrain = 20f;
    [Export] public float FlapStaminaRegen = 15f;
    [Export] public float TakeoffChargeCapTime = 1f;
    [Export] public float TakeoffLockDuration = 0.2f;
    [Export] public float CrouchSpeedMultiplier = 0.5f;
    [Export] public float TakeoffMaxHeight = 300f;
    [Export] public float TakeoffAnimationSpeed = 1.0f;


    protected override void InitializeManagers()
    {
        base.InitializeManagers();
        // Set editor-tunable values to movement manager
        MovementManager.Gravity = Gravity;
        MovementManager.RunSpeed = RunSpeed;
        MovementManager.JumpVelocity = JumpVelocity;
        MovementManager.DashSpeed = DashSpeed;
        MovementManager.DashDuration = DashDuration;
        MovementManager.GlideHorizontalSpeed = GlideHorizontalSpeed;
        MovementManager.GlideDriftSmoothness = GlideDriftSmoothness;
        MovementManager.GlideAscendRate = GlideAscendRate;
        MovementManager.GlideDescendRate = GlideDescendRate;
        MovementManager.FlapStaminaMax = FlapStaminaMax;
        MovementManager.FlapStaminaDrain = FlapStaminaDrain;
        MovementManager.FlapStaminaRegen = FlapStaminaRegen;
        MovementManager.TakeoffMaxHeight = TakeoffMaxHeight;
        MovementManager.TakeoffChargeCapTime = TakeoffChargeCapTime;
        MovementManager.TakeoffLockDuration = TakeoffLockDuration;
        MovementManager.CrouchSpeedMultiplier = CrouchSpeedMultiplier;
        
        // Connect animation finished signal
        GetNode<AnimationPlayer>("AnimationPlayer").AnimationFinished += (animName) => OnAnimationFinished(animName);
    }

    protected override void UpdateGrounded(float delta)
    {
        if (!IsOnFloor())
        {
            NextState = State.Airborne;
            MovementManager.ResetDash();
            return;
        }

        if (IsLanding)
        {
            MovementManager.SetHorizontalVelocity(0f);
            return;
        }

        float inputX = MovementManager.GetInputX();
        bool jumpPressed = Input.IsActionJustPressed("jump");
        bool downPressed = Input.IsActionPressed("down");
        bool upPressed = Input.IsActionPressed("up");

        if (upPressed && !downPressed)
        {
            NextState = State.TakeoffCharging;
            MovementManager.TakeoffChargeTime = 0f;
            return;
        }

        if (jumpPressed)
        {
            MovementManager.ApplyJump(JumpVelocity);
            NextState = State.Airborne;
            MovementManager.ResetDash();
            return;
        }
        

        float baseSpeed = MovementManager.ApplyCrouchMultiplier(RunSpeed, downPressed);
        MovementManager.SetGroundedVelocity(new Vector2(inputX * baseSpeed, 0f));

        // Update facing if player is pressing left/right
        if (inputX > 0) Facing = 1;
        else if (inputX < 0) Facing = -1;

        // Play appropriate animation
        AnimationManager.PlayGroundedMovementAnimation(inputX, downPressed);
    }

    protected override void UpdateAirborne(float delta)
    {
        MovementManager.AddGravity(delta);
        float inputX = MovementManager.GetInputX();
        MovementManager.ApplyAirborneMovement(inputX);

        // Update facing
        if (inputX > 0) Facing = 1;
        else if (inputX < 0) Facing = -1;

        // Dash with Space
        if (MovementManager.HasDashAvailable() && Input.IsActionJustPressed("jump"))
        {
            AnimationManager.PlayDashAnimation(Facing);
            MovementManager.StartDash(Facing);
        }

        if (MovementManager.IsDashing())
        {
            MovementManager.SetVelocity(MovementManager.GetDashVelocity());
        }
        

        if (IsOnFloor())
        {
            MovementManager.SetVerticalVelocity(0f);
            MovementManager.ResetStamina();
            AnimationManager.PlayLandingAnimation();
            IsLanding = true;
            NextState = State.Grounded;
        }
    }

    protected override void UpdateTakeoffCharging(float delta)
    {
        if (!IsOnFloor())
        {
            NextState = State.Airborne;
            MovementManager.ResetDash();
            return;
        }

        MovementManager.TakeoffChargeTime += delta;
        MovementManager.ResetVelocity();

        if (Input.IsActionJustReleased("up"))
        {
            _ApplyTakeoff();
            NextState = State.TakeoffImpulse;
            MovementManager.TakeoffLockTimer = TakeoffLockDuration;
            MovementManager.ResetDash();
            return;
        }

        if (Input.IsActionPressed("down"))
            NextState = State.Grounded;
    }

    protected override void UpdateTakeoffImpulse(float delta)
    {
        MovementManager.ApplyTakeoffRise();
    }

    protected override void UpdateGliding(float delta)
    {
        bool canExit = MovementManager.TakeoffLockTimer <= 0f;

        if (Input.IsActionPressed("down") && canExit)
        {
            NextState = State.Airborne;
            return;
        }

        if (IsOnFloor())
        {
            MovementManager.SetVerticalVelocity(0f);
            NextState = State.Grounded;
            MovementManager.ResetStamina();
            AnimationManager.PlayLandingAnimation();
            return;
        }

        float inputX = MovementManager.GetInputX();
        MovementManager.ApplyGlideMovement(inputX, delta);

        bool spaceHeld = Input.IsActionPressed("jump");
        if (inputX > 0) Facing = 1;
        else if (inputX < 0) Facing = -1;

        if (spaceHeld && MovementManager.CanGlideFlap())
        {
            // Ascend while space is held
            MovementManager.ApplyGlideAscent(delta);

            // Loop GlideFlap while space is held
            AnimationManager.PlayGlideFlapAnimation();
        }
        else
        {
            // Descend gently
            MovementManager.ApplyGlideDescent();
            AnimationManager.PlayGlideDescentAnimation(inputX);
        }
    }

    private void _ApplyTakeoff()
    {
        AnimationPlayer animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        MovementManager.CalculateTakeoffRiseSpeed(animPlayer, TakeoffAnimationSpeed);
        AnimationManager.EnterTakeoffImpulseState(TakeoffAnimationSpeed);
    }

    protected override void EnterState(State state)
    {
        switch (state)
        {
            case State.TakeoffImpulse:
                AnimationManager.EnterTakeoffImpulseState(TakeoffAnimationSpeed);
                break;
            case State.Gliding:
                AnimationManager.EnterGlidingState();
                break;
            case State.Grounded:
                AnimationManager.EnterGroundedState();
                break;
            case State.Airborne:
                AnimationManager.EnterAirborneState(PreviousState);
                break;
            case State.TakeoffCharging:
                AnimationManager.EnterTakeoffChargingState();
                break;
        }
    }

    protected override void OnAnimationFinishedHandler(string animName)
    {
        if (animName == "Land")
        {
            IsLanding = false;
            AnimationManager.PlayIdleAnimation();
        }
        
        if (animName == "TakeOff" && CurrentState == State.TakeoffImpulse)
        {
            NextState = State.Gliding;
        }
    }

    protected override void ExitState(State state) { }

    public override void _Ready()
    {
        this.InitializeManagers();
    }
    public override void _Draw()
    {
        base._Draw();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        for (int i = 0; i < GetSlideCollisionCount(); i++) {
            var collision = GetSlideCollision(i);
            if (collision.GetCollider() is TileMapLayer tilemap) {
                Vector2 worldPos = collision.GetPosition();
                Vector2I tileCoords = tilemap.LocalToMap(
                    tilemap.ToLocal(worldPos)
                );
                var tileData = tilemap.GetCellTileData(tileCoords);
                if (tileData != null) {
                    if (tileData.HasCustomData("HazardLevel")) {
                        Variant HazardLevel = tileData.GetCustomData("HazardLevel");
                        if (HazardLevel.VariantType == Variant.Type.Int) {
                            int HazardLevelInt = (int)HazardLevel;
                            if (HazardLevelInt == 0) {
                                return;
                            }
                            this.GetParent<Game>().HazardHit(HazardLevelInt);
                        }
                    }
                }
            }
        }
    }
}