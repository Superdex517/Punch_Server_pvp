using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(TestPlayerCtr currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) { }

    public override void EnterState() 
    {
        Ctx.ObjectState = EObjectState.Move; 
        Ctx.SendMovePacket = true;
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        Ctx.AppliedMovementX = Ctx.CurrentMovementInput.x * Ctx.RunMultiplier;
        Ctx.AppliedMovementZ = Ctx.CurrentMovementInput.y * Ctx.RunMultiplier;
    }

    public override void ExitState()
    {
        Ctx.SendMovePacket = false;
    }
    public override void InitializeSubState() { }
    public override void CheckSwitchStates()
    {
        if (!Ctx.IsMovementPressed)
            SwitchState(Factory.Idle());
        else if(Ctx.IsMovementPressed && !Ctx.IsRunPressed)
            SwitchState(Factory.Walk());
    }
}
