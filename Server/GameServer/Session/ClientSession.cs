﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ServerCore;
using System.Net;
using Google.Protobuf;
using GameServer.Game;
using GameServer.Data;


namespace GameServer
{
    public partial class ClientSession : PacketSession
    {
        public long AccountDbId { get; set; }
        public int SessionId { get; set; }

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
            SessionManager.Instance.Remove(this);

            Console.WriteLine($"OnDisconnected : {endPoint}");
        }


        public override void OnSend(int numOfBytes)
        {
            Console.WriteLine($"Transferred bytes: {numOfBytes}");
        }
        #endregion
    }
}
