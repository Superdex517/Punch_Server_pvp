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
    public partial class WaitingRoom : JobSerializer
    {
        public List<Hero> _playerList = new List<Hero>();

        Dictionary<int, Hero> _players = new Dictionary<int, Hero>();
        Dictionary<int, GameRoom> _rooms = new Dictionary<int, GameRoom>();

        int _roomId = 1;

        public void Update()
        {
            Flush();

            foreach (GameRoom room in _rooms.Values)
            {
                room.Update();
            }
        }

        public void EnterWaitingRoom(BaseObject player)
        {
            _players.Add(player.ObjectId, (Hero)player);
        }

        public void MakeGameRoom(GameRoom room)
        {
            if (room == null)
                return;

            AddGameRoom(1);

            {
                S_MakeRoom makeRoomPacket = new S_MakeRoom();
                //makeRoomPacket.RoomInfo.RoomId = room.RoomInfo.RoomId;
                room.Session?.Send(makeRoomPacket);
            }

            S_SpawnUI spawnPacket = new S_SpawnUI();
            spawnPacket.Rooms.Add(room.RoomInfo);
            BroadcastMakeRoom(spawnPacket);

            Console.WriteLine($"Add Room {room.RoomInfo.RoomId}");
        }
        public GameRoom AddGameRoom(int mapTemplateId)
        {
            GameRoom room = new GameRoom();
            room.Push(room.Init, mapTemplateId, 10);
            room.RoomInfo.RoomId = _roomId;
            _rooms.Add(_roomId, room);
            _roomId++;

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
            return _rooms.Remove(roomId);
        }

        public GameRoom Find(int roomId)
        {
            GameRoom room = null;
            if (_rooms.TryGetValue(roomId, out room))
                return room;

            return null;
        }

        public List<GameRoom> GetRooms()
        {
            return _rooms.Values.ToList();
        }
    }
}
