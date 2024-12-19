using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState, IRootState
{
    public PlayerJumpState(TestPlayerCtr currentContext, PlayerStateFactory playerStateFactory) 
        : base(currentContext, playerStateFactory) 
    {
        IsRootState = true;
        InitializeSubState();
    }

    public override void EnterState() 
    {
        HandleJump();
    }

    public override void UpdateState() 
    {
        HandleGravity();
        CheckSwitchStates();
    }


    public override void ExitState() 
    {
        if (Ctx.IsJumpPressed)
            Ctx.RequireNewJumpPress = true;
    }

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
        if (Ctx.IsGrounded())
            SwitchState(Factory.Grounded());
    }

    

    private void HandleJump()
    {
        Ctx.CurrentMovementY = Ctx.InitalJumpVelocity * 0.5f;
        Ctx.AppliedMovementY = Ctx.InitalJumpVelocity * 0.5f;
    }

    public void HandleGravity()
    {
        bool isFalling = Ctx.CurrentMovementY <= 0.0f || !Ctx.IsJumpPressed;
        float fallMultiplier = 2.0f;

        if (isFalling)
        {
            float previousYVelocity = Ctx.CurrentMovementY;
            Ctx.CurrentMovementY = Ctx.CurrentMovementY + (Ctx.Gravity * fallMultiplier * Time.deltaTime);
            Ctx.AppliedMovementY = Mathf.Max((previousYVelocity + Ctx.CurrentMovementY) * 0.5f, -20.0f);
        }
        else
        {
            float previousYVelocity = Ctx.CurrentMovementY;
            Ctx.CurrentMovementY = Ctx.CurrentMovementY + (Ctx.Gravity * Time.deltaTime);
            Ctx.AppliedMovementY = (previousYVelocity + Ctx.CurrentMovementY) * 0.5f;
        }
    }
}
