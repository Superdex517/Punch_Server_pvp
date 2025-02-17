﻿using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public class BaseObject
    {
        public EGameSceneType SceneType { get; set; } = EGameSceneType.Lobby;
        public EGameObjectType ObjectType { get; protected set; } = EGameObjectType.None;
        public int ObjectId
        {
            get { return ObjectInfo.ObjectId; }
            set { ObjectInfo.ObjectId = value; }
        }

        public WaitingRoom WaitingRoom { get; set; }
        public ObjectInfo ObjectInfo { get; set; } = new ObjectInfo();
        public PositionInfo PosInfo { get; private set; } = new PositionInfo();

        public EMoveDir Dir
        {
            get { return PosInfo.MoveDir; }
            set { PosInfo.MoveDir = value; }
        }

        public EObjectState State
        {
            get { return PosInfo.State; }
            set { PosInfo.State = value; }
        }

        public BaseObject()
        {
            ObjectInfo.PosInfo = PosInfo;
        }

        public virtual void Update()
        {

        }

        public Vector3 Pos
        {
            get
            {
                return new Vector3(PosInfo.PosX, PosInfo.PosY, PosInfo.PosZ);
            }

            set
            {
                PosInfo.PosX = value.X; ;
                PosInfo.PosY = value.Y; ;
                PosInfo.PosZ = value.Z; ;
            }
        }

        public float MoveDir
        {
            get { return PosInfo.Dir; }
            set { PosInfo.Dir = value; }
        }

        public void BroadcastMove()
        {
            S_Move movePacket = new S_Move();
            movePacket.ObjectId = ObjectId;
            movePacket.PosInfo = PosInfo;
            WaitingRoom.GameRoom?.Broadcast(Pos, movePacket);
        }
    }
}
