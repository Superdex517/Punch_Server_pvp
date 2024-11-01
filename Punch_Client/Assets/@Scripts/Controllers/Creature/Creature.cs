using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : BaseObject
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {

    }

    protected override void UpdateMove()
    {
        base.UpdateMove();

        // 이동 끝났으면 Idle 상태로 변경
        if (LerpPosCompleted)
        {
            ObjectState = EObjectState.Idle;
            return;
        }
    }

    public bool MoveToPosition(Vector3 destPos)
    {

        return Managers.Move.MoveTo(this, destPos);
    }
}
