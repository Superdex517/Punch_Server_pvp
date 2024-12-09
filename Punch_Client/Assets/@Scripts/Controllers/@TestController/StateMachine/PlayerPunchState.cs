using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunchState : PlayerBaseState
{
    public PlayerPunchState(TestPlayerCtr currentContext, PlayerStateFactory playerStateFactory)
            : base(currentContext, playerStateFactory) { }

    public override void EnterState() { }
    public override void ExitState() { }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void CheckSwitchStates() { }
    public override void InitializeSubState() { }
}
