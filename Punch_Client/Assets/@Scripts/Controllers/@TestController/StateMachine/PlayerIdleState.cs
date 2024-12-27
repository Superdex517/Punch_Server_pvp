using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(MyPlayer currentContext, PlayerStateFactory playerStateFactory) 
        : base(currentContext, playerStateFactory) { }
    
    public override void EnterState() 
    {
        Ctx.ObjectState = EObjectState.Idle;

        Ctx.AppliedMovementX = 0;
        Ctx.AppliedMovementZ = 0;

        Ctx.SendMovePacket = true;
    }
    public override void UpdateState() 
    {
        CheckSwitchStates();
    }
    public override void ExitState() 
    {
        Ctx.SendMovePacket = false;
    }
    public override void InitializeSubState() { }
    public override void CheckSwitchStates()
    {
        if (Ctx.IsMovementPressed && Ctx.IsRunPressed)
            SwitchState(Factory.Run());
        else if (Ctx.IsMovementPressed)
            SwitchState(Factory.Walk());
    }
}
