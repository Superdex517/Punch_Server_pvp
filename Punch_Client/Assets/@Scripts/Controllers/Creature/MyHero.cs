using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyHero : Hero
{
    protected bool _sendMovePacket = false;

    private Vector3 _destPos;
    public Vector3 DestPos
    {
        get { return _destPos; }
        set
        {
            if(_destPos == value) return;

            _destPos = value;
            _sendMovePacket = true;
        }
    }

    Vector3? _desiredDestPos;

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
        UpdateSendMovePacket();
    }

    #region 이동 동기화
    void UpdateInputKey()
    {

    }

    void UpdateSendMovePacket()
    {
        if (_sendMovePacket)
        {
            C_Move movePacket = new C_Move() { PosInfo = new PositionInfo() };
            movePacket.PosInfo.MergeFrom(PosInfo);
            movePacket.PosInfo.PosX = DestPos.x;
            movePacket.PosInfo.PosY = DestPos.y;
            movePacket.PosInfo.PosZ = DestPos.z;
            Managers.Network.GameServer.Send(movePacket);
            _sendMovePacket = false;
        }
        //일단 해보고 MoveDir은 어떻게 보낼지 고민해보기
    }
    #endregion
}
