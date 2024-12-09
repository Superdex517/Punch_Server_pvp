using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerBaseState, IRootState
{
    public PlayerFallState(TestPlayerCtr currentContext, PlayerStateFactory factory) 
        : base(currentContext, factory)
    {
        IsRootState = true;
        InitializeSubState();
    }
    public override void EnterState()
    {
        Ctx.AppliedMovementY = 100.0f;
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
        HandleGravity();
    }
    
    public override void ExitState() 
    {
        //애니메이션 원상태
        Ctx.ObjectState = EObjectState.Idle;
    }

    public void HandleGravity()
    {
        float previousYVelocity = Ctx.CurrentMovementY;
        Ctx.CurrentMovementY = Ctx.CurrentMovementY + Ctx.Gravity * Time.deltaTime;
        Ctx.AppliedMovementY = Mathf.Max((previousYVelocity + Ctx.CurrentMovementY) * 0.5f, -20.0f);
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.IsGrounded())
            SwitchState(Factory.Grounded());
    }
    
    public override void InitializeSubState()
    {
        if (!Ctx.IsMovementPressed && !Ctx.IsRunPressed)
            SetSubState(Factory.Idle());
        else if (Ctx.IsMovementPressed && !Ctx.IsRunPressed)
            SetSubState(Factory.Run());
        else
            SetSubState(Factory.Run());
    }
}
