using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerGroundState : PlayerBaseState, IRootState
{
    public PlayerGroundState(MyPlayer currentContext, PlayerStateFactory playerStateFactory)
        : base (currentContext, playerStateFactory) 
    {
        IsRootState = true;
    }

    public void HandleGravity()
    {
        Ctx.CurrentMovementY = Ctx.Gravity;
        Ctx.AppliedMovementY = Ctx.Gravity;
    }

    public override void EnterState() 
    {
        InitializeSubState();
        HandleGravity();
    }

    public override void UpdateState() 
    {
        CheckSwitchStates();
    }

    public override void ExitState() { }

    public override void InitializeSubState()
    {
        if (!Ctx.IsMovementPressed && !Ctx.IsRunPressed)
            SetSubState(Factory.Idle());
        else if (Ctx.IsMovementPressed && !Ctx.IsRunPressed)
            SetSubState(Factory.Walk());
        else
            SetSubState(Factory.Run());
    }

    public override void CheckSwitchStates() 
    {
        if (Ctx.IsJumpPressed && !Ctx.RequireNewJumpPress)
            SwitchState(Factory.Jump());
        else if (Ctx.IsPunchPressed && !Ctx.RequireNewPunchPress)
            SwitchState(Factory.Punch());
        else if(!Ctx.IsGrounded())
            SwitchState(Factory.Fall());
    }
}
