using Google.Protobuf.Protocol;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public partial class ClientSession : PacketSession
    {
        public void HandleMakeRoom(C_MakeRoom makeRoom)
        {
            //여기까지 패킷 전달이 완료되었고
            Console.WriteLine($"MakeRoom");

            //방 리스트에 추가
            //unity spawn ui
            //broadcastmakeroom
            Room = ObjectManager.Instance.SpawnUI<GameRoom>(1);
            {

            }

            GameLogic.Instance.Push(() =>
            {
                GameRoom room = GameLogic.Instance.Add(1);
            });
        }

        public void HandleEnterRoom(C_EnterRoom makeRoom)
        {
            Console.WriteLine("EnterRoom");

        }

        public void HandleDestroyRoom(C_DestroyRoom destroyRoom)
        {
            GameLogic.Instance.Push(() =>
            {
                GameLogic.Instance.Remove(destroyRoom.MyRoomInfo.RoomInfo.RoomId);
            });
        }

        public void HandleEnterGame(C_EnterGame enterGamePacket)
        {
            Console.WriteLine("HandleEnterGame");

            MyHero = ObjectManager.Instance.Spawn<Hero>(1);
            {
                MyHero.ObjectInfo.PosInfo.State = EObjectState.Idle;
                MyHero.ObjectInfo.PosInfo.PosX = 0;
                MyHero.ObjectInfo.PosInfo.PosY = 0;
                MyHero.ObjectInfo.PosInfo.PosZ = 0;
                MyHero.ObjectInfo.PosInfo.Dir = 0;
                MyHero.Session = this;
            }

            // TODO : DB에서 마지막 좌표 등 갖고 와서 처리.
            GameLogic.Instance.Push(() =>
            {
                GameRoom room = GameLogic.Instance.Find(enterGamePacket.MyRoomInfo.RoomInfo.RoomId);

                room?.Push(() =>
                {
                    Hero hero = MyHero;
                    room.EnterGame(hero, respawn: false, pos: null);
                });
            });
        }
    }
}
