using Godot;

/// <summary>
/// Handles all animation logic with state-based animation management.
/// Organized uniformly to be easily expandable for future states.
/// </summary>
public class AnimationManager
{
    private AnimationPlayer _animPlayer;

    // Animation state tracking
    private string _currentAnimation = "";

    // Animation speed multipliers for different states
    public float TakeoffAnimationSpeed = 1.0f;

    public AnimationManager(AnimationPlayer animPlayer)
    {
        _animPlayer = animPlayer;
    }

    /// <summary>
    /// Play an animation if it's not already playing
    /// </summary>
    public void PlayAnimation(string animName)
    {
        if (_currentAnimation != animName)
        {
            _animPlayer.Play(animName);
            _currentAnimation = animName;
        }
    }

    /// <summary>
    /// Play animation with custom speed scale
    /// </summary>
    public void PlayAnimation(string animName, float speedScale)
    {
        if (_currentAnimation != animName)
        {
            _animPlayer.Play(animName);
            _animPlayer.SpeedScale = speedScale;
            _currentAnimation = animName;
        }
    }

    /// <summary>
    /// Get the current animation name
    /// </summary>
    public string GetCurrentAnimation()
    {
        return _currentAnimation;
    }

    /// <summary>
    /// Get animation length (accounting for speed scale)
    /// </summary>
    public float GetAnimationLength(string animName, float speedScale = 1.0f)
    {
        return (float)_animPlayer.GetAnimation(animName).Length / speedScale;
    }

    /// <summary>
    /// Handle animation finished events
    /// </summary>
    public void OnAnimationFinished(string animName)
    {
        // This is called from StateManager when animation finishes
        // State-specific logic is handled in the state manager
    }

    #region Grounded State Animations

    /// <summary>
    /// Play run/walk animation based on input
    /// </summary>
    public void PlayGroundedMovementAnimation(float inputX, bool isCrouching)
    {
        if (inputX > 0)
        {
            PlayAnimation(isCrouching ? "CrouchRight" : "RunRight");
        }
        else if (inputX < 0)
        {
            PlayAnimation(isCrouching ? "CrouchLeft" : "RunLeft");
        }
        else
        {
            PlayAnimation(isCrouching ? "CrouchIdle" : "Idle");
        }
    }

    /// <summary>
    /// Play landing animation
    /// </summary>
    public void PlayLandingAnimation()
    {
        PlayAnimation("Land");
    }

    /// <summary>
    /// Play idle animation
    /// </summary>
    public void PlayIdleAnimation()
    {
        PlayAnimation("Idle");
    }

    #endregion

    #region Airborne State Animations

    /// <summary>
    /// Play jump animation with custom speed
    /// </summary>
    public void PlayJumpAnimation()
    {
        PlayAnimation("Jump", 2.0f);
    }

    /// <summary>
    /// Play airborne animation with default speed
    /// </summary>
    public void PlayAirborneAnimation()
    {
        PlayAnimation("Airborne", 1.0f);
    }

    /// <summary>
    /// Play dash animation based on facing direction
    /// </summary>
    public void PlayDashAnimation(int facing)
    {
        if (facing == 0)
        {
            PlayAnimation("Airborne"); // no dash if no facing
        }
        else if (facing == 1)
        {
            PlayAnimation("DashRight");
        }
        else
        {
            PlayAnimation("DashLeft");
        }
    }

    #endregion

    #region Takeoff State Animations

    /// <summary>
    /// Play takeoff charge animation
    /// </summary>
    public void PlayTakeoffChargeAnimation()
    {
        PlayAnimation("TakeOffCharge", 1.0f);
    }

    /// <summary>
    /// Play takeoff impulse animation with custom speed
    /// </summary>
    public void PlayTakeoffImpulseAnimation(float speedScale)
    {
        PlayAnimation("TakeOff", speedScale);
        TakeoffAnimationSpeed = speedScale;
    }

    #endregion

    #region Glide State Animations

    /// <summary>
    /// Play glide flapping animation (ascending)
    /// </summary>
    public void PlayGlideFlapAnimation()
    {
        PlayAnimation("GlideFlap");
    }

    /// <summary>
    /// Play glide descent animation based on direction
    /// </summary>
    public void PlayGlideDescentAnimation(float inputX)
    {
        if (inputX > 0)
        {
            PlayAnimation("GlideRight");
        }
        else if (inputX < 0)
        {
            PlayAnimation("GlideLeft");
        }
        else
        {
            PlayAnimation("GlideIdle");
        }
    }

    /// <summary>
    /// Play idle gliding animation
    /// </summary>
    public void PlayGlideIdleAnimation()
    {
        PlayAnimation("GlideIdle", 1.0f);
    }

    #endregion

    #region State Entry Animations

    /// <summary>
    /// Handle animations when entering Airborne state
    /// </summary>
    public void EnterAirborneState(StateManager.State previousState)
    {
        switch (previousState)
        {
            case StateManager.State.Grounded:
                PlayJumpAnimation();
                break;
            case StateManager.State.Gliding:
                PlayAirborneAnimation();
                break;
            default:
                PlayAirborneAnimation();
                break;
        }
    }

    /// <summary>
    /// Handle animations when entering Grounded state
    /// </summary>
    public void EnterGroundedState()
    {
        _animPlayer.SpeedScale = 1.0f;
    }

    /// <summary>
    /// Handle animations when entering TakeoffCharging state
    /// </summary>
    public void EnterTakeoffChargingState()
    {
        PlayTakeoffChargeAnimation();
    }

    /// <summary>
    /// Handle animations when entering TakeoffImpulse state
    /// </summary>
    public void EnterTakeoffImpulseState(float speedScale)
    {
        PlayTakeoffImpulseAnimation(speedScale);
    }

    /// <summary>
    /// Handle animations when entering Gliding state
    /// </summary>
    public void EnterGlidingState()
    {
        PlayGlideIdleAnimation();
        _animPlayer.SpeedScale = 1.0f;
    }

    #endregion

    /// <summary>
    /// Reset animation speed scale to normal
    /// </summary>
    public void ResetSpeedScale()
    {
        _animPlayer.SpeedScale = 1.0f;
    }

    /// <summary>
    /// Set animation speed scale
    /// </summary>
    public void SetSpeedScale(float speedScale)
    {
        _animPlayer.SpeedScale = speedScale;
    }
}
