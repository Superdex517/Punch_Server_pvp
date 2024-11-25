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
                MyHero.SceneType = EGameSceneType.Lobby;
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
                WaitingRoom = ObjectManager.Instance.SpawnUI<WaitingRoom>(makeWaitingRoom.RoomInfo.RoomId);
                {
                    WaitingRoom.Session = this;
                    WaitingRoom.RoomInfo.RoomName = makeWaitingRoom.RoomInfo.RoomName;
                    Lobby.MakeWaitingRoom(WaitingRoom);
                    WaitingRoom.EnterWaitingRoom(MyHero);
                    
                    Console.WriteLine($"Handle Make WaitingRoom: {WaitingRoom.RoomInfo.RoomId}");
                }
            });
        }

        public void HandleEnterWaitingRoom(C_EnterWaitingRoom enterRoom)
        {
            Console.WriteLine("EnterRoom");
            
            MyHero.SceneType = EGameSceneType.Waiting;

            GameLogic.Instance.Push(() =>
            {
                WaitingRoom waitingRoom = GameLogic.Instance.Lobby.FindWaitingRoom(enterRoom.RoomInfo.RoomId);

                Console.WriteLine(enterRoom.RoomInfo.RoomId);
                
                waitingRoom?.Push(() =>
                {
                    Hero hero = MyHero;

                    waitingRoom.EnterWaitingRoom(hero);
                });
            });

            //TODO : player id만 찾아서 1명만 입장
        }

        public void HandleReady(C_Ready ready)
        {
            //host를 제외한 나머지 인원이 ready상태인지 체크한다
            //ready.RoomInfo
        }

        public void HandleDestroyWaitingRoom(C_DestroyWaitingRoom destroyRoom)
        {
            GameLogic.Instance.Push(() =>
            {
                GameLogic.Instance.Lobby.Remove(destroyRoom.RoomInfo.RoomId);
            });
        }

        public void HandleEnterGame(C_EnterGame enterGamePacket)
        {
            Console.WriteLine("HandleEnterGame");

            //MyHero = ObjectManager.Instance.Spawn<Hero>(1);
            //{
            //    MyHero.ObjectInfo.PosInfo.State = EObjectState.Idle;
            //    MyHero.SceneType = EGameSceneType.Game;
            //    MyHero.ObjectInfo.PosInfo.PosX = 0;
            //    MyHero.ObjectInfo.PosInfo.PosY = 0;
            //    MyHero.ObjectInfo.PosInfo.PosZ = 0;
            //    MyHero.ObjectInfo.PosInfo.Dir = 0;
            //}

            MyHero.ObjectInfo.PosInfo.State = EObjectState.Idle;
            MyHero.SceneType = EGameSceneType.Game;
            MyHero.ObjectInfo.PosInfo.PosX = 0;
            MyHero.ObjectInfo.PosInfo.PosY = 0;
            MyHero.ObjectInfo.PosInfo.PosZ = 0;
            MyHero.ObjectInfo.PosInfo.Dir = 0;

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
