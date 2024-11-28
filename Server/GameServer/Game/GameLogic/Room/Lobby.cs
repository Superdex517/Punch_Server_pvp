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
        public WaitingRoom WaitingRoom { get; set; }
        public GameRoom Room { get; set; }

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
                _players.Add(hero.ObjectId, hero);
                playerCount++;

                {
                    S_EnterLobby enterPacket = new S_EnterLobby();
                    enterPacket.MyHeroInfo = hero.MyHeroInfo;
                    enterPacket.MyHeroInfo.Scene = EGameSceneType.Lobby;

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

        public void MakeWaitingRoom(WaitingRoom room, BaseObject obj)
        {
            if (room == null)
                return;

            if (obj == null)
                return;

            AddWaitingRoom(room, 1);

            GameLogic.Instance.Lobby.FindWaitingRoom(room.RoomInfo.RoomId).EnterWaitingRoom(obj);

            {
                S_MakeWaitingRoom makeRoomPacket = new S_MakeWaitingRoom();
                makeRoomPacket.RoomInfo = room.RoomInfo;
                room.Session?.Send(makeRoomPacket);
            }

            BroadcastRoom(room);
        }

        public WaitingRoom AddWaitingRoom(WaitingRoom waitingRoom, int mapTemplateId)
        {
            waitingRoom.Push(waitingRoom.Init, mapTemplateId, 10);

            //room.WaitingRoomId = _waitingRoomId;
            //_waitingRoomId++;
            _waitingRooms.Add(waitingRoom.WaitingRoomId, waitingRoom);

            return waitingRoom;
        }

        public void BroadcastRoom(WaitingRoom room)
        {
            //방 만들었다는걸 다른 대기자에게 알림
            S_SpawnWaitingRoomUI spawnPacket = new S_SpawnWaitingRoomUI();
            spawnPacket.Rooms.Add(room.RoomInfo);
            Broadcast(spawnPacket);
        }

        public void Broadcast(IMessage packet)
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
            {
                Console.WriteLine("room is");
                return room;
            }
            return null;
        }


        public List<WaitingRoom> GetWaitingRooms()
        {
            return _waitingRooms.Values.ToList();
        }
    }
}
