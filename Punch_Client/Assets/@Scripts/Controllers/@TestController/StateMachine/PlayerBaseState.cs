using UnityEngine;

public abstract class PlayerBaseState
{
    private bool _isRootState = false;
    private MyPlayer _ctx;
    private PlayerStateFactory _factory;
    private PlayerBaseState _currentSuperState;
    private PlayerBaseState _currentSubState;

    protected bool IsRootState { set { _isRootState = value; } }
    protected MyPlayer Ctx { get { return _ctx; } }
    protected PlayerStateFactory Factory { get { return _factory; } }
    public PlayerBaseState(MyPlayer currentContext, PlayerStateFactory factory)
    {
        _ctx = currentContext;
        _factory = factory;
    }

    public virtual void EnterState() {  }
    public virtual void UpdateState() { }
    public virtual void ExitState() { }
    public virtual void CheckSwitchStates() { }
    public virtual void InitializeSubState() { }

    public void UpdateStates()
    {
        UpdateState();

        if (_currentSubState != null)
        {
            _currentSubState.UpdateStates();
        }
    }

    protected void SwitchState(PlayerBaseState newState) 
    {
        ExitState();

        newState.EnterState();

        if (_isRootState)
        {
            _ctx.CurrentState = newState;
        }
        else if (_currentSuperState != null)
        {
            _currentSuperState.SetSubState(newState);
        }
        
    }

    protected void SetSuperState(PlayerBaseState newSuperState) 
    {
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(PlayerBaseState newSubState) 
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}
