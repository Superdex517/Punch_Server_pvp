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
        CheckSwitchStates();
        HandleGravity();
    }

    public override void ExitState() 
    {
        //Ctx.Animator.SetBool(Ctx.IsJumpingHash, false);

        //if (Ctx.IsJumpPressed)
        //{
        //    Ctx.RequireNewJumpPress = true;
        //}
        //Ctx.CurrentJumpResetRoutine = Ctx.StartCoroutine(IJumpResetRoutine());
        //if(Ctx.JumpCount == 3)
        //{
        //    Ctx.JumpCount = 0;
        //    Ctx.Animator.SetInteger(Ctx.JumpCountHash, Ctx.JumpCount);
        //}
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
    
    public override void CheckSwitchStates() 
    {
        if (Ctx.IsGrounded())
            SwitchState(Factory.Grounded());
    }
    private void HandleJump()
    {
        if(!Ctx.IsJumping && Ctx.IsGrounded())
        {
            Ctx.IsJumping = true;
            Ctx.CurrentMovementY = Ctx.InitalJumpVelocity * 0.5f;
            Ctx.AppliedMovementY = Ctx.InitalJumpVelocity * 0.5f;
        }
        else if(Ctx.IsJumpPressed && Ctx.IsJumping && Ctx.IsGrounded())
        {
            Ctx.IsJumping = false;
        }
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
