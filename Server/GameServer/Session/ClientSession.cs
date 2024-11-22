using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ServerCore;
using System.Net;
using Google.Protobuf;
using GameServer;
using GameServer.Data;


namespace GameServer
{
    public partial class ClientSession : PacketSession
    {
        public long AccountDbId { get; set; }
        public int SessionId { get; set; }

        public Lobby Lobby { get; set; }
        public WaitingRoom WaitingRoom { get; set; }
        public GameRoom Room { get; set; }
        public Hero MyHero { get; set; }

        object _lock = new object();
         
        #region Network
        public void Send(IMessage packet)
        {
            Send(new ArraySegment<byte>(MakeSendBuffer(packet)));
        }

        public static byte[] MakeSendBuffer(IMessage packet)
        {
            MsgId msgId = (MsgId)Enum.Parse(typeof(MsgId), packet.Descriptor.Name);
            ushort size = (ushort)packet.CalculateSize();
            byte[] sendBuffer = new byte[size + 4];
            Array.Copy(BitConverter.GetBytes((ushort)(size + 4)), 0, sendBuffer, 0, sizeof(ushort));
            Array.Copy(BitConverter.GetBytes((ushort)msgId), 0, sendBuffer, 2, sizeof(ushort));
            Array.Copy(packet.ToByteArray(), 0, sendBuffer, 4, size);
            return sendBuffer;
        }

        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.OnRecvPacket(this, buffer);
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            GameLogic.Instance.Push(() =>
            {
                if (MyHero == null)
                    return;

                //만약 Lobby라면?
                //만약 WaitingRoom이라면?
                //만약 Game이라면?
                GameRoom room = GameLogic.Instance.Lobby.FindWaitingRoom(1).GameRoom;
                room.Push(room.LeaveGame, MyHero.ObjectId, false);
            });

            SessionManager.Instance.Remove(this);

            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

        public override void OnSend(int numOfBytes)
        {
            //Console.WriteLine($"Transferred bytes: {numOfBytes}");
        }
        #endregion
    }
}
