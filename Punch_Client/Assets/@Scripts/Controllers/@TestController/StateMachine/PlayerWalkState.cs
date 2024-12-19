using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(TestPlayerCtr currentContext, PlayerStateFactory factory)
        : base(currentContext, factory) { }

    public override void EnterState()
    {
        Ctx.ObjectState = EObjectState.Move;
        Ctx.SendMovePacket = true;
    }

    public override void UpdateState()
    {
        Ctx.AppliedMovementX = Ctx.CurrentMovementInput.x;
        Ctx.AppliedMovementZ = Ctx.CurrentMovementInput.y;
        CheckSwitchStates();

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
        else if(Ctx.IsMovementPressed && Ctx.IsRunPressed)
            SwitchState(Factory.Run());
    }
}
