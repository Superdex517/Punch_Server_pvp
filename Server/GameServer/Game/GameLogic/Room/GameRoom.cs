﻿using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace GameServer
{
    public partial class GameRoom : JobSerializer
    {
        public int GameRoomId { get; set; }
        public int TemplateId { get; set; }

        Dictionary<int, Hero> _heroes = new Dictionary<int, Hero>();


        public void Init(int mapTemplateId, int zoneCells)
        {

        }

        public void Update()
        {
            Flush();
        }

        public void EnterGame(BaseObject obj, bool respawn = false, Vector3? pos = null)
        {
            if (obj == null)
                return;

            if (pos.HasValue)
                obj.Pos = pos.Value;
            else
                obj.Pos = GetSpawnPos(obj, checkObjects: true);

            EGameObjectType type = ObjectManager.GetObjectTypeFromId(obj.ObjectId);
            if(type == EGameObjectType.Hero)
            {
                Hero hero = (Hero)obj;
                _heroes.Add(obj.ObjectId, hero);
                hero.Room = this;

                hero.State = EObjectState.Idle;
                hero.Update();

                 // 입장한 사람한테 패킷 보내기.
                {
                    S_EnterGame enterPacket = new S_EnterGame();
                    enterPacket.MyHeroInfo = hero.MyHeroInfo;
                    enterPacket.Respawn = respawn;

                    hero.Session?.Send(enterPacket);

                }

                // 다른 사람들한테 입장 알려주기.
                S_Spawn spawnPacket = new S_Spawn();
                spawnPacket.Heroes.Add(hero.HeroInfo);
                Broadcast(obj.Pos, spawnPacket);
            }
        }

        public void LeaveGame(int objectId, bool kick = false)
        {
            EGameObjectType type = ObjectManager.GetObjectTypeFromId(objectId);

            Vector3 pos;

            if(type == EGameObjectType.Hero)
            {
                if (_heroes.Remove(objectId, out Hero hero) == false)
                    return;

                pos = hero.Pos;
                hero.Room = null;

                {
                    S_LeaveGame leavePacket = new S_LeaveGame();
                    hero.Session?.Send(leavePacket);
                }

                if (kick)
                {

                }
            }
            else
            {
                return;
            }

            {
                S_Despawn despawnPacket = new S_Despawn();
                despawnPacket.ObjectIds.Add(objectId);
                Broadcast(pos, despawnPacket);
            }
        }

        public void Broadcast(Vector3 pos, IMessage packet)
        {
            byte[] packetBuffer = ClientSession.MakeSendBuffer(packet);

            //룸 안에 있는 모든 Hero들에게 broadcast
            foreach (Hero p in _heroes.Values)
            {
                p.Session?.Send(packetBuffer);
            }
        }

        public Vector3 GetSpawnPos(BaseObject obj, bool checkObjects = true)
        {
            Vector3 randomPos;

            while (true)
            {
                randomPos.X = 0;
                randomPos.Y = 0;
                randomPos.Z = 0;

                return randomPos;
            }
        }
    }
}
