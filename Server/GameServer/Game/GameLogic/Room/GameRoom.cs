using Google.Protobuf;
using Google.Protobuf.Protocol;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public partial class GameRoom : JobSerializer
    {
        public int GameRoomId { get; set; }
        public int TemplateId { get; set; }
        public int MaxPlayerCount { get; set; }
        public ClientSession Session { get; set; }
        public EGameUIType UIType { get; protected set; } = EGameUIType.None;

        public Dictionary<int, Hero> _heroes = new Dictionary<int, Hero>();

        Random _rand = new Random();

        public GameRoom()
        {

        }

        public void Init(int mapTemplateId, int zoneCells)
        {
            //TODO : 맵 여러개 생기면 추가
            TemplateId = mapTemplateId;
        }

        public void Update()
        {
            Flush();
        }

        public void EnterWaitingList(BaseObject obj)
        {
            if (obj == null)
                return;

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

            if (type == EGameObjectType.Hero)
            {
                Hero hero = (Hero)obj;
                _heroes.Add(obj.ObjectId, hero);
                hero.WaitingRoom.GameRoom = this;

                ApplyMove(hero, new Vector3(hero.Pos.X, hero.Pos.Y, hero.Pos.Z), hero.MoveDir);

                hero.State = EObjectState.Idle;
                hero.Update();

                // 입장한 사람한테 패킷 보내기.
                {
                    S_EnterGame enterPacket = new S_EnterGame();
                    enterPacket.MyHeroInfo = hero.MyHeroInfo;
                    enterPacket.Respawn = respawn;

                    hero.Session?.Send(enterPacket);

                    hero.Vision?.Update();
                }

                // 다른 사람들한테 입장 알려주기.
                S_Spawn spawnPacket = new S_Spawn();
                spawnPacket.Heroes.Add(hero.HeroInfo);
                Broadcast(obj.Pos, spawnPacket);
            }

            //Console.WriteLine($"EnterGame_{obj}");
        }

        public void LeaveGame(int objectId, bool kick = false)
        {
            EGameObjectType type = ObjectManager.GetObjectTypeFromId(objectId);

            Vector3 pos;

            if (type == EGameObjectType.Hero)
            {
                if (_heroes.Remove(objectId, out Hero hero) == false)
                    return;

                pos = hero.Pos;
                hero.WaitingRoom.GameRoom = null;

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

        public void Gameover()
        {
            S_GameResult resultPacket = new S_GameResult();
            resultPacket.IsGameOver = true;

            byte[] packetBuffer = ClientSession.MakeSendBuffer(resultPacket);

            foreach (Hero p in _heroes.Values)
            {
                p.Session?.Send(packetBuffer);
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
                randomPos.X = _rand.Next(1, 50);
                randomPos.Y = 1;
                randomPos.Z = _rand.Next(1, 50);

                return randomPos;
            }
        }
    }
}
