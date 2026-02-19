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

    // Slash combo count
    private int slash_count = 0;

    // Landing state
    protected bool IsLanding = false;

    // Timer
    protected float SlashAnimStartBufferTimer = 0f;

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
        if (Input.IsActionJustPressed("attack"))
        {
           

            if (slash_count == 2)
            {
                playMegaSlash();
            }
            else if (slash_count == 1)
            {
                AnimationPlayer ShineAnim = GetNode<AnimationPlayer>("ShineEffect");
                ShineAnim.Play("ShineEffect");
                AnimationPlayer SpinAnim = GetNode<AnimationPlayer>("SpinSlash/AnimationPlayer2");
                
                SpinAnim.Stop();
                if (Facing == 1)
                {
                    SpinAnim.Play("SlashRight");
                }
                else if (Facing == -1)
                {
                    SpinAnim.Play("SlashLeft");
                }
            }
            else
            {
                AnimationPlayer SpinAnim = GetNode<AnimationPlayer>("SpinSlash/AnimationPlayer2");
                SpinAnim.Stop();
                if (Facing == 1)
                {
                    SpinAnim.Play("SlashRight");
                }
                else if (Facing == -1)
                {
                    SpinAnim.Play("SlashLeft");
                }
               
            }
            slash_count = (slash_count + 1) % 3;
        }
        float dt = (float)delta;
        UpdateState(dt);
        base.Velocity = MovementManager.GetVelocity();
        MoveAndSlide();
    }
    private void playMegaSlash()
	{
        AnimationPlayer ShineAnim = GetNode<AnimationPlayer>("ShineEffect");
        ShineAnim.Play("Stopping");
		if (Facing == 1)
            {
                AnimationPlayer SpinAnim = GetNode<AnimationPlayer>("SpinSlash/AnimationPlayer2");
                SpinAnim.Stop();
                SpinAnim.Play("SpinSlashRight");
                GD.Print("Test action pressed, emitting particles.");
            }
            else if (Facing == -1)
            {
                AnimationPlayer SpinAnim = GetNode<AnimationPlayer>("SpinSlash/AnimationPlayer2");
                SpinAnim.Stop();
                SpinAnim.Play("SpinSlashLeft");
            }
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
