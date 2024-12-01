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
            MyHero = ObjectManager.Instance.SpawnPlayer<Hero>(1);
            {
                MyHero.SceneType = EGameSceneType.Lobby;
                MyHero.Session = this;
            }

            GameLogic.Instance.Lobby.Push(() =>
            {
                GameLogic.Instance.Lobby.EnterLoby(MyHero);
            });
        }

        public void HandleMakeWaitingRoom(C_MakeWaitingRoom makeWaitingRoom)
        {
            MyHero.Session.WaitingRoom = ObjectManager.Instance.SpawnRoom<WaitingRoom>();
            {
                MyHero.Session.WaitingRoom.RoomInfo.RoomName = makeWaitingRoom.RoomInfo.RoomName;
                MyHero.Session.WaitingRoom.AddGameRoom(1);
                MyHero.Session = this;

                Console.WriteLine($"{MyHero.ObjectId} Make WaitingRoom: {MyHero.Session.WaitingRoom.RoomInfo.RoomId}");
            }

            GameLogic.Instance.Lobby.Push(() =>
            {
                WaitingRoom waitingRoom = MyHero.Session.WaitingRoom;

                Hero hero = MyHero;

                GameLogic.Instance.Lobby.MakeWaitingRoom(waitingRoom, MyHero);
            });
        }

        public void HandleEnterWaitingRoom(C_EnterWaitingRoom enterRoom)
        {
            Console.WriteLine("Handle_EnterWaitingRoom");
            
            MyHero.SceneType = EGameSceneType.Waiting;

            GameLogic.Instance.Lobby.Push(() =>
            {
                Hero hero = MyHero;

                GameLogic.Instance.Lobby.FindWaitingRoom(enterRoom.RoomInfo.RoomId).EnterWaitingRoom(hero);
            });
        }

        public void HandleReady(C_Ready ready)
        {
            //TODO : ready check
            //host를 제외한 나머지 인원이 ready상태인지 체크한다
        }

        public void HandleStartGame(C_GameStart gameStartPacket)
        {
            Console.WriteLine("Handle_StartGame");
            
            GameLogic.Instance.Lobby.Push(() =>
            {
                GameLogic.Instance.Lobby.FindWaitingRoom(gameStartPacket.RoomInfo.RoomId).StartGame();
            });
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
            //spawn안해서 안움직이는지 체크
            MyHero.ObjectInfo.PosInfo.State = EObjectState.Idle;
            MyHero.SceneType = EGameSceneType.Game;
            MyHero.ObjectInfo.PosInfo.PosX = 0;
            MyHero.ObjectInfo.PosInfo.PosY = 0;
            MyHero.ObjectInfo.PosInfo.PosZ = 0;
            MyHero.ObjectInfo.PosInfo.Dir = 0;
            MyHero.Session = this;

            GameLogic.Instance.Lobby.Push(() =>
            {
                Hero hero = MyHero;

                GameLogic.Instance.Lobby.FindWaitingRoom(enterGamePacket.RoomInfo.RoomId).GameRoom.EnterGame(hero, respawn: false, pos: null);
            });
        }
    }
}
