using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public partial class WaitingRoom : JobSerializer
    {
        public int WaitingRoomId
        {
            get { return RoomInfo.RoomId; }
            set { RoomInfo.RoomId = value; }
        }
        public string WaitingRoomTitle
        {
            get { return RoomInfo.RoomName; }
            set { RoomInfo.RoomName = value; }
        }
        public ClientSession Session { get; set; }
        public RoomInfo RoomInfo { get; private set; } = new RoomInfo();
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
                hero.MyHeroInfo.Scene = EGameSceneType.Waiting;
                hero.WaitingRoom = this;
                _players.Add(hero.ObjectId, hero);
                MaxPlayerCount++;
                
                Console.WriteLine($"EnterWaitingRoom : {obj.ObjectId}, {_players.Count}");

                {
                    S_EnterWaitingRoom enterPacket = new S_EnterWaitingRoom();
                    enterPacket.MyHeroInfo = hero.MyHeroInfo;
                    enterPacket.RoomInfo = hero.WaitingRoom.RoomInfo;
                    hero.Session?.Send(enterPacket);
                }
            }
        }

        public void Ready(BaseObject obj)
        {
            if (obj == null)
                return;

            Hero hero = (Hero)obj;
            hero.IsReady = true;

            {

            }
        }

        public void LeaveWaitingRoom(int objectId, bool kick = false)
        {
            if (_players.Remove(objectId, out Hero hero) == false)
                return;

            hero.WaitingRoom = null;

            {
                S_LeaveWaitingRoom leavePacket = new S_LeaveWaitingRoom();
                hero.Session?.Send(leavePacket);
            }

            if (kick) 
            {
                
            }
            else
            {
                return;
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
