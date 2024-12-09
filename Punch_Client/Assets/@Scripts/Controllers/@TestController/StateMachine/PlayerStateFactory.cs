public class PlayerStateFactory
{
    protected TestPlayerCtr _context;
    protected PlayerStateFactory _factory;
    public PlayerStateFactory(TestPlayerCtr currentContext)
    {
        _context = currentContext;
    }

    public PlayerBaseState Idle()
    {
        return new PlayerIdleState(_context, this);
    }

    public PlayerBaseState Run()
    {
        return new PlayerRunState(_context, this);
    }

    public PlayerBaseState Jump()
    {
        return new PlayerJumpState(_context, this);
    }

    public PlayerBaseState Punch()
    {
        return new PlayerPunchState(_context, this);
    }

    public PlayerBaseState Grounded()
    {
        return new PlayerGroundState(_context, this);
    }

    public PlayerBaseState Fall()
    {
        return new PlayerFallState(_context, this);
    }
}
