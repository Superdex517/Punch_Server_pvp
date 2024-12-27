using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunchState : PlayerBaseState
{
    public PlayerPunchState(MyPlayer currentContext, PlayerStateFactory playerStateFactory)
            : base(currentContext, playerStateFactory) 
    {
        IsRootState = true;
    }

    public override void EnterState() 
    {
        HandlePunch();
        InitializeSubState();
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
        SwitchState(Factory.Grounded());

        Ctx.CurrentPunchResetRoutine = Ctx.StartCoroutine(IPunchRoutine());
    }

    IEnumerator IPunchRoutine()
    {
        yield return new WaitForSeconds(.5f);

        ResetState();
    }

    public void ResetState()
    {
        if (!Ctx.IsMovementPressed && !Ctx.IsRunPressed)
            Ctx.ObjectState = EObjectState.Idle;
        else if (Ctx.IsMovementPressed && !Ctx.IsRunPressed)
            Ctx.ObjectState = EObjectState.Move;
        else
            Ctx.ObjectState = EObjectState.Move;
    }


    void HandlePunch()
    {
        Ctx.ObjectState = EObjectState.Skill;
        Ctx.SendMovePacket = true;
    }
}
