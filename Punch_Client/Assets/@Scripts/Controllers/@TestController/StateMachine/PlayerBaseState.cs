using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class PlayerBaseState : MonoBehaviour
{
    private bool _isRootState = false;
    private TestPlayerCtr _ctx;
    private PlayerStateFactory _factory;
    private PlayerBaseState _currentSuperState;
    private PlayerBaseState _currentSubState;

    protected bool IsRootState { set { _isRootState = value; } }
    protected TestPlayerCtr Ctx { get { return _ctx; } }
    protected PlayerStateFactory Factory { get { return _factory; } }

    public PlayerBaseState(TestPlayerCtr currentContext, PlayerStateFactory factory)
    {
        _ctx = currentContext;
        _factory = factory;
    }

    public virtual void EnterState() { }
    public virtual void UpdateState() { }
    public virtual void ExitState() { }
    public virtual void CheckSwitchStates() { }
    public virtual void InitializeSubState() { }

    public void UpdateStates() 
    {
        UpdateState();
        if(_currentSubState != null)
        {
            _currentSubState.UpdateStates();
        }
    }
    protected void SwitchState(PlayerBaseState newState) 
    {
        ExitState();

        newState.EnterState();

        if (_isRootState)
            _ctx.CurrentState = newState;
        else if(_currentSuperState != null)
            _currentSuperState.SetSubState(newState);
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
