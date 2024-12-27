using System.Collections.Generic;
enum PlayerStates
{
    idle,
    walk,
    run,
    grounded,
    jump,
    fall,
    punch
}
public class PlayerStateFactory
{
    protected MyPlayer _context;
    Dictionary<PlayerStates, PlayerBaseState> _states = new Dictionary<PlayerStates, PlayerBaseState>();
    public PlayerStateFactory(MyPlayer currentContext)
    {
        _context = currentContext;
        _states[PlayerStates.idle] = new PlayerIdleState(_context, this);
        _states[PlayerStates.walk] = new PlayerWalkState(_context, this);
        _states[PlayerStates.run] = new PlayerRunState(_context, this);
        _states[PlayerStates.jump] = new PlayerJumpState(_context, this);
        _states[PlayerStates.grounded] = new PlayerGroundState(_context, this);
        _states[PlayerStates.fall] = new PlayerFallState(_context, this);
        _states[PlayerStates.punch] = new PlayerPunchState(_context, this);
    }

    public PlayerBaseState Idle()
    {
        return _states[PlayerStates.idle];
    }

    public PlayerBaseState Walk()
    {
        return _states[PlayerStates.walk];
    }

    public PlayerBaseState Run()
    {
        return _states[PlayerStates.run];
    }

    public PlayerBaseState Jump()
    {
        return _states[PlayerStates.jump];
    }

    public PlayerBaseState Punch()
    {
        return _states[PlayerStates.punch];
    }

    public PlayerBaseState Grounded()
    {
        return _states[PlayerStates.grounded];
    }

    public PlayerBaseState Fall()
    {
        return _states[PlayerStates.fall];
    }
}
