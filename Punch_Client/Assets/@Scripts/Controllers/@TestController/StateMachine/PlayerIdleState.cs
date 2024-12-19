using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(TestPlayerCtr currentContext, PlayerStateFactory playerStateFactory) 
        : base(currentContext, playerStateFactory) { }
    
    public override void EnterState() 
    {
        Debug.Log("Idle Enter");
        Ctx.AppliedMovementX = 0;
        Ctx.AppliedMovementZ = 0;
        Ctx.ObjectState = EObjectState.Idle;

        Ctx.SendMovePacket = true;
    }
    public override void UpdateState() 
    {
        Debug.Log("Idle Update");
        CheckSwitchStates();
    }
    public override void ExitState() 
    {
        Debug.Log("Idle Exit");
        Ctx.SendMovePacket = false;
    }
    public override void InitializeSubState() { }
    public override void CheckSwitchStates()
    {
        if (Ctx.IsMovementPressed && Ctx.IsRunPressed)
            SwitchState(Factory.Run());
        else if (Ctx.IsMovementPressed)
            SetSubState(Factory.Walk());
    }
}
