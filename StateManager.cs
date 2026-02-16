using Godot;

/// <summary>
/// Base state manager that coordinates movement, animation, and future states.
/// </summary>
public partial class StateManager : CharacterBody2D
{
    // State Machine
    public enum State { Grounded, Airborne, TakeoffCharging, TakeoffImpulse, Gliding, Combat }
    public State CurrentState = State.Grounded;
    protected State NextState = State.Grounded;
    protected State PreviousState;

    // Managers
    protected MovementManager MovementManager;
    protected AnimationManager AnimationManager;

    // Facing direction: +1 = right, -1 = left
    public int Facing = 1;

    // Landing state
    protected bool IsLanding = false;

    public override void _Ready()
    {
        InitializeManagers();
    }

    protected virtual void InitializeManagers()
    {
        MovementManager = new MovementManager(this);
        AnimationManager = new AnimationManager(GetNode<AnimationPlayer>("AnimationPlayer"));
    }

    public override void _Process(double delta)
    {
        float dt = (float)delta;
        UpdateState(dt);
        base.Velocity = MovementManager.GetVelocity();
        MoveAndSlide();
    }

    protected virtual void UpdateState(float delta)
    {
        // Update timers and state-specific logic
        MovementManager.UpdateTimers(delta);

        switch (CurrentState)
        {
            case State.Grounded:
                UpdateGrounded(delta);
                break;
            case State.Airborne:
                UpdateAirborne(delta);
                break;
            case State.TakeoffCharging:
                UpdateTakeoffCharging(delta);
                break;
            case State.TakeoffImpulse:
                UpdateTakeoffImpulse(delta);
                break;
            case State.Gliding:
                UpdateGliding(delta);
                break;
        }

        // State transition
        if (NextState != CurrentState)
        {
            ExitState(CurrentState);
            PreviousState = CurrentState;
            CurrentState = NextState;
            EnterState(CurrentState);
        }
    }

    protected virtual void UpdateGrounded(float delta) { }
    protected virtual void UpdateAirborne(float delta) { }
    protected virtual void UpdateTakeoffCharging(float delta) { }
    protected virtual void UpdateTakeoffImpulse(float delta) { }
    protected virtual void UpdateGliding(float delta) { }

    protected virtual void EnterState(State state) { }
    protected virtual void ExitState(State state) { }

    public void PlayAnimation(string animName)
    {
        AnimationManager.PlayAnimation(animName);
    }

    public void OnAnimationFinished(string animName)
    {
        AnimationManager.OnAnimationFinished(animName);
        OnAnimationFinishedHandler(animName);
    }

    protected virtual void OnAnimationFinishedHandler(string animName) { }

}
