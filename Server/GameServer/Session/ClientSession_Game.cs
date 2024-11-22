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
        public void HandleEnterLobby(C_EnterLobby enterLoby)
        {

            Lobby = GameLogic.Instance.Lobby;

            MyHero = ObjectManager.Instance.Spawn<Hero>(1);
            {
                Console.WriteLine($"EnterLobby:{MyHero.ObjectId}");
                
                MyHero.Session = this;
            }


            GameLogic.Instance.Lobby.Push(() =>
            {
                Lobby.Push(() =>
                {
                    Lobby.EnterLoby(MyHero);
                });
            });
        }



        public void HandleMakeWaitingRoom(C_MakeWaitingRoom makeWaitingRoom)
        {

            GameLogic.Instance.Lobby.Push(() =>
            {
                WaitingRoom = ObjectManager.Instance.SpawnUI<WaitingRoom>(1);
                {
                    WaitingRoom.Session = this;
                    WaitingRoom.RoomInfo.WaitingRoomName = makeWaitingRoom.RoomInfo.WaitingRoomName;
                    Lobby.MakeWaitingRoom(WaitingRoom);
                    
                    Console.WriteLine($"Handle Make WaitingRoom: {WaitingRoom.RoomInfo.WaitingRoomId}");
                }
            });
        }

        public void HandleEnterWaitingRoom(C_EnterWaitingRoom makeRoom)
        {
            Console.WriteLine("EnterRoom");

            GameLogic.Instance.Push(() =>
            {
                WaitingRoom room = GameLogic.Instance.Lobby.FindWaitingRoom(1);

                room?.Push(() =>
                {
                    Hero hero = MyHero;
                    
                    room.EnterWaitingRoom(hero);
                });
            });

            //TODO : player id만 찾아서 1명만 입장
        }

        public void HandleLeaveWaitingRoom(S_LeaveWaitingRoom leaveWaitingRoom)
        {

        }

        public void HandleDestroyWaitingRoom(C_DestroyWaitingRoom destroyRoom)
        {
            GameLogic.Instance.Push(() =>
            {
                GameLogic.Instance.Lobby.Remove(destroyRoom.RoomInfo.WaitingRoomId);
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
            }

            // TODO : DB에서 마지막 좌표 등 갖고 와서 처리.
            GameLogic.Instance.Push(() =>
            {
                GameRoom room = GameLogic.Instance.Lobby.FindWaitingRoom(1).GameRoom;

                room?.Push(() =>
                {
                    Hero hero = MyHero;

                    room.EnterGame(hero, respawn: false, pos: null);
                });
            });
        }
    }
}
