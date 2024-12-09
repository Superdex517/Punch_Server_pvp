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
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        Ctx.AppliedMovementX = Ctx.CurrentMovementInput.x;
        Ctx.AppliedMovementZ = Ctx.CurrentMovementInput.y;
    }

    public override void ExitState() 
    {
        
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
