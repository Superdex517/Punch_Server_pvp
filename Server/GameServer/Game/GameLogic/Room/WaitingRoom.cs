using Google.Protobuf;
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
    public partial class WaitingRoom : JobSerializer
    {
        public Dictionary<int, Hero> _heroes = new Dictionary<int, Hero>();
        
        public Dictionary<int, GameRoom> _rooms = new Dictionary<int, GameRoom>();

        public void Init(int mapTemplateId, int zoneCells)
        {
            
        }

        public void Update()
        {
            Flush();
        }

        public void BroadcastMakeRoom(Int32 roomId, IMessage packet)
        {
            byte[] packetBuffer = ClientSession.MakeSendBuffer(packet);

            //TODO : 현재 만들어진 룸 리스트 보여주기
            S_MakeRoom makeRoom = new S_MakeRoom();
            makeRoom.MyRoomInfo.RoomInfo.RoomId = roomId;

            foreach (Hero p in _heroes.Values)
            {
                p.Session?.Send(packetBuffer);
            }


        }
    }
}
