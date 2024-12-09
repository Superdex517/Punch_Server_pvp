using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerBaseState, IRootState
{
    public PlayerGroundState(TestPlayerCtr currentContext, PlayerStateFactory playerStateFactory)
        : base (currentContext, playerStateFactory) 
    {
        IsRootState = true;
        InitializeSubState();
    }

    public void HandleGravity()
    {
        Ctx.CurrentMovementY = Ctx.Gravity;
        Ctx.AppliedMovementY = Ctx.Gravity;
    }

    public override void EnterState() 
    {
        HandleGravity();
    }

    public override void UpdateState() 
    {
        CheckSwitchStates();
    }

    public override void ExitState() { }

    public override void CheckSwitchStates() 
    {
        if (Ctx.IsJumpPressed)
            SwitchState(Factory.Jump());
        else if(!Ctx.IsGrounded())
            SwitchState(Factory.Fall());
    }

    public override void InitializeSubState() 
    {
        if(!Ctx.IsMovementPressed && !Ctx.IsRunPressed)
            SetSubState(Factory.Idle());
        else if(Ctx.IsMovementPressed && !Ctx.IsRunPressed)
            SetSubState(Factory.Run());
        else
            SetSubState(Factory.Run());
    }
}
