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
        public void HandleEnterWaitingRoom(C_EnterWaitingRoom waitigRoom)
        {
            Console.WriteLine("HandleWaitingRoom");

            MyHero = ObjectManager.Instance.Spawn<Hero>(1);
            {
                MyHero.ObjectInfo.PosInfo.State = EObjectState.Idle;
                MyHero.ObjectInfo.PosInfo.PosX = 0;
                MyHero.ObjectInfo.PosInfo.PosY = 0;
                MyHero.ObjectInfo.PosInfo.PosZ = 0;
                MyHero.ObjectInfo.PosInfo.Dir = 0;
                MyHero.Session = this;
            }

            GameLogic.Instance.WaitingRoom.Push(() =>
            {
                WaitingRoom waitingRoom = GameLogic.Instance.WaitingRoom;

                waitingRoom.Push(() =>
                {
                    waitingRoom.EnterWaitingRoom(MyHero);
                });
            });
        }

        public void HandleMakeRoom(C_MakeRoom makeRoom)
        {
            GameLogic.Instance.WaitingRoom.Push(() =>
            {
                WaitingRoom waitingRoom = GameLogic.Instance.WaitingRoom;

                Room = ObjectManager.Instance.SpawnUI<GameRoom>(1);
                {
                    Room.Session = this;
                    
                    //Room.GameRoomId = /*makeRoom.RoomInfo.RoomId*/;
                    //Room.GameRoomId = 1;
                    
                    waitingRoom.MakeGameRoom(Room);

                }
            });
        }

        public void HandleEnterRoom(C_EnterRoom makeRoom)
        {
            Console.WriteLine("EnterRoom");

            //TODO : player id만 찾아서 2명만 입장
        }

        public void HandleLeaveRoom(C_LeaveRoom makeRoom)
        {
            Console.WriteLine("LeaveRoom");

        }

        public void HandleDestroyRoom(C_DestroyRoom destroyRoom)
        {
            GameLogic.Instance.Push(() =>
            {
                GameLogic.Instance.WaitingRoom.Remove(destroyRoom.RoomInfo.RoomId);
            });
        }

        public void HandleEnterGame(C_EnterGame enterGamePacket)
        {
            Console.WriteLine("HandleEnterGame");

            // TODO : DB에서 마지막 좌표 등 갖고 와서 처리.
            GameLogic.Instance.Push(() =>
            {
                GameRoom room = GameLogic.Instance.WaitingRoom.Find(1);

                room?.Push(() =>
                {
                    Hero hero = MyHero;
                    room.EnterGame(hero, respawn: false, pos: null);
                });
            });
        }
    }
}
