using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunchState : PlayerBaseState
{
    public PlayerPunchState(TestPlayerCtr currentContext, PlayerStateFactory playerStateFactory)
            : base(currentContext, playerStateFactory) 
    {
    }

    public override void EnterState() 
    {
        InitializeSubState();
        HandlePunch();

    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState() 
    {
        Debug.Log("Punch Exit");

        if (Ctx.IsPunchPressed)
            Ctx.RequireNewPunchPress = true;
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
        Debug.Log("Punch Check");

    }

    void HandlePunch()
    {
        Ctx.ObjectState = EObjectState.Skill;
        Ctx.SendMovePacket = true;
    }
}
