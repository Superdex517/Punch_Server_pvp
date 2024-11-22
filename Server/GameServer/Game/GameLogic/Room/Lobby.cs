using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace GameServer
{
    public partial class Lobby : JobSerializer
    {
        Dictionary<int, Hero> _players = new Dictionary<int, Hero>();
        Dictionary<int, WaitingRoom> _waitingRooms = new Dictionary<int, WaitingRoom>();
        public ClientSession Session { get; set; }

        int _waitingRoomId = 1;
        int playerCount = 0;

        public void Update()
        {
            Flush();

            foreach (WaitingRoom room in _waitingRooms.Values)
            {
                room.Update();
            }
        }

        public void EnterLoby(BaseObject player)
        {
            if (player == null)
                return;

            EGameObjectType type = ObjectManager.GetObjectTypeFromId(player.ObjectId);

            if (type == EGameObjectType.Hero)
            {
                Hero hero = (Hero)player;
                hero.SceneType = EGameSceneType.Lobby;
                _players.Add(hero.ObjectId, hero);
                playerCount++;

                {
                    S_EnterLobby enterPacket = new S_EnterLobby();
                    enterPacket.PlayerCount = playerCount;
                    hero.Session?.Send(enterPacket);

                    //기존의 방이 있다면 기존의 방 생성 시켜준다
                    if (_waitingRooms.Count > 0)
                    {
                        S_SpawnWaitingRoomUI spawnUIPacket = new S_SpawnWaitingRoomUI();

                        foreach (WaitingRoom room in _waitingRooms.Values)
                        {
                            spawnUIPacket.Rooms.Add(room.RoomInfo);
                        }
                        hero.Session?.Send(spawnUIPacket);
                    }
                }
            }
        }

        public void LeaveLoby(int playerId, bool destroyRoom = false)
        {
            EGameObjectType type = ObjectManager.GetObjectTypeFromId(playerId);

            if (type == EGameObjectType.Hero)
            {
                if (_players.Remove(playerId, out Hero player) == false)
                    return;

                {
                    S_LeaveWaitingRoom leavePacket = new S_LeaveWaitingRoom();
                    player.Session?.Send(leavePacket);
                }

                if (destroyRoom)
                {

                }
            }
            else
            {
                return;
            }

            {

            }
        }

        public void MakeWaitingRoom(WaitingRoom room)
        {
            if (room == null)
                return;

            AddGameRoom(1, room);

            {
                S_MakeWaitingRoom makeRoomPacket = new S_MakeWaitingRoom();
                room.Session?.Send(makeRoomPacket);
            }

            //방 만들었다는걸 다른 대기자에게 알림
            S_SpawnWaitingRoomUI spawnPacket = new S_SpawnWaitingRoomUI();
            spawnPacket.Rooms.Add(room.RoomInfo);
            BroadcastMakeRoom(spawnPacket);
        }

        public WaitingRoom AddGameRoom(int mapTemplateId, WaitingRoom room)
        {
            //GameRoom myRoom = new GameRoom();
            room.Push(room.Init, mapTemplateId, 10);

            room.WaitingRoomId = _waitingRoomId;
            _waitingRooms.Add(_waitingRoomId, room);
            _waitingRoomId++;

            return room;
        }

        public void BroadcastMakeRoom(IMessage packet)
        {
            byte[] packetBuffer = ClientSession.MakeSendBuffer(packet);

            //TODO : 현재 만들어진 룸 리스트 보여주기
            foreach (Hero player in _players.Values)
            {
                player.Session?.Send(packetBuffer);
            }
        }

        public bool Remove(int roomId)
        {
            return _waitingRooms.Remove(roomId);
        }

        public WaitingRoom FindWaitingRoom(int waitingRoomId)
        {
            WaitingRoom room = null;
            if (_waitingRooms.TryGetValue(waitingRoomId, out room))
                return room;

            return null;
        }

        public List<WaitingRoom> GetWaitingRooms()
        {
            return _waitingRooms.Values.ToList();
        }
    }
}
