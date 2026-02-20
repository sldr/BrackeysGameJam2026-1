using Godot;

/// <summary>
/// Handles all movement and physics logic for the player.
/// Maintains state-independent velocity management and movement calculations.
/// </summary>
public class MovementManager
{
    private CharacterBody2D _character;

    // Physics Constants (Editor Tunable)
    public float Gravity = 1200f;
    public float RunSpeed = 200f;
    public float JumpVelocity = -400f;
    public float DashSpeed = 300f;
    public float DashDuration = 0.15f;
    public float GlideHorizontalSpeed = 150f;
    public float GlideDriftSmoothness = 0.15f;
    public float GlideAscendRate = -200f;
    public float GlideDescendRate = 100f;
    public float FlapStaminaDrain = 20f;
    public float FlapStaminaRegen = 15f;
    public float TakeoffMaxHeight = 300f;
    public float TakeoffChargeCapTime = 1f;
    public float TakeoffLockDuration = 0.2f;
    public float CrouchSpeedMultiplier = 0.5f;

    // Velocity
    private Vector2 _velocity = Vector2.Zero;

    // Timers
    private float _dashTimer = 0f;
    public float TakeoffChargeTime = 0f;
    public float TakeoffLockTimer = 0f;
    public float TakeoffImpulseTimer = 0f;

    // Dash
    private bool _hasDashAvailable = true;
    private Vector2 _dashDirection = Vector2.Zero;

    // Takeoff
    public float TakeoffTargetHeight = 0f;
    public float TakeoffRiseSpeed = 0f;

    public MovementManager(CharacterBody2D character)
    {
        _character = character;
    }

    public void UpdateTimers(float delta)
    {
        if (_dashTimer > 0f) _dashTimer -= delta;
        if (TakeoffLockTimer > 0f) TakeoffLockTimer -= delta;
        if (TakeoffImpulseTimer > 0f) TakeoffImpulseTimer -= delta;
    }

    public void SetVelocity(Vector2 velocity)
    {
        _velocity = velocity;
    }

    public Vector2 GetVelocity()
    {
        return _velocity;
    }

    public void AddGravity(float delta)
    {
        _velocity.Y += Gravity * delta;
    }

    public void SetGroundedVelocity(Vector2 velocity)
    {
        _velocity = velocity;
    }

    public void SetHorizontalVelocity(float x)
    {
        _velocity.X = x;
    }

    public void SetVerticalVelocity(float y)
    {
        _velocity.Y = y;
    }

    public void ResetVelocity()
    {
        _velocity = Vector2.Zero;
    }

    public void ApplyJump(float jumpForce)
    {
        _velocity.Y = jumpForce;
    }

    public void ResetDash()
    {
        _hasDashAvailable = true;
    }

    public void StartDash(float dashDir)
    {
        _hasDashAvailable = false;
        _dashTimer = DashDuration;
        _dashDirection = new Vector2(dashDir, 0f);
    }

    public bool IsDashing()
    {
        return _dashTimer > 0f;
    }

    public Vector2 GetDashVelocity()
    {
        return _dashDirection * DashSpeed;
    }

    public float GetInputX()
    {
        return Input.GetAxis("left", "right");
    }

    public float ApplyCrouchMultiplier(float baseSpeed, bool crouching)
    {
        return crouching ? baseSpeed * CrouchSpeedMultiplier : baseSpeed;
    }

    public void ApplyAirborneMovement(float inputX)
    {
        _velocity.X = inputX * RunSpeed;
    }

    public void ApplyGlideMovement(float inputX, float delta)
    {
        float targetX = inputX * GlideHorizontalSpeed;
        _velocity.X = Mathf.Lerp(_velocity.X, targetX, GlideDriftSmoothness);
    }

    public bool TryApplyGlideAscent(Game game, float delta)
    {
        var (success, value) = game.TryDecreaseAnyStaminaUpTo(FlapStaminaDrain * delta);
        if (!success) {
            return false;
        }
        _velocity.Y = GlideAscendRate;
        return true;
    }

    public void ApplyGlideDescent()
    {
        _velocity.Y = GlideDescendRate;
    }

    public void ApplyTakeoffRise()
    {
        _velocity.Y = -TakeoffRiseSpeed; // negative Y = up in Godot
    }

    public void CalculateTakeoffRiseSpeed(AnimationPlayer animPlayer, float takeoffAnimationSpeed)
    {
        float chargePercent = Mathf.Clamp(TakeoffChargeTime / TakeoffChargeCapTime, 0f, 1f);
        TakeoffTargetHeight = TakeoffMaxHeight * chargePercent;

        float animLength = (float)animPlayer.CurrentAnimationLength / takeoffAnimationSpeed;
        TakeoffRiseSpeed = TakeoffTargetHeight / animLength;
    }

    public void ResetStamina(Game game)
    {
        game.ResetStamina();
    }

    public bool HasDashAvailable()
    {
        return _hasDashAvailable;
    }

}
