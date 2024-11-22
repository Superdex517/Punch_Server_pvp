using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public partial class WaitingRoom : JobSerializer
    {
        public int WaitingRoomId
        {
            get { return RoomInfo.WaitingRoomId; }
            set { RoomInfo.WaitingRoomId = value; }
        }
        public string WaitingRoomTitle
        {
            get { return RoomInfo.WaitingRoomName; }
            set { RoomInfo.WaitingRoomName = value; }
        }
        public ClientSession Session { get; set; }
        public WaitingRoomInfo RoomInfo { get; private set; } = new WaitingRoomInfo();
        public EGameUIType UIType { get; protected set; } = EGameUIType.None;

        public int TemplateId { get; set; }
        public int MaxPlayerCount { get; set; }

        Dictionary<int, Hero> _players = new Dictionary<int, Hero>();
        public GameRoom GameRoom { get; set; }

        public WaitingRoom()
        {

        }

        public void Init(int maxPlayer, int mapTemplateId)
        {
            //TODO : 맵 여러개 생기면 추가

        }

        public void Update()
        {
            Flush();
        }

        public void EnterWaitingRoom(BaseObject obj)
        {
            if (obj == null)
                return;

            if(obj.ObjectType == EGameObjectType.Hero)
            {
                Hero hero = (Hero)obj;
                hero.SceneType = EGameSceneType.Waiting;
                _players.Add(hero.ObjectId, hero);

                {
                    S_EnterWaitingRoom enterPacket = new S_EnterWaitingRoom();
                    enterPacket.RoomInfo = RoomInfo;
                    hero.Session?.Send(enterPacket);
                }
            }
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


    }
}
