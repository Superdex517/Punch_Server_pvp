﻿using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ServerSession : PacketSession
{
    public void Send(IMessage packet)
    {
        MsgId msgId = (MsgId)Enum.Parse(typeof(MsgId), packet.Descriptor.Name);
        ushort size = (ushort)packet.CalculateSize();
        byte[] sendBuffer = new byte[size + 4];
        Array.Copy(BitConverter.GetBytes((ushort)(size + 4)), 0, sendBuffer, 0, sizeof(ushort));
        Array.Copy(BitConverter.GetBytes((ushort)msgId), 0, sendBuffer, 2, sizeof(ushort));
        Array.Copy(packet.ToByteArray(), 0, sendBuffer, 4, size);
        Send(new ArraySegment<byte>(sendBuffer));
    }

    //packetqueue에 데이터를 넣는 이유 -> 유니티는 멀티쓰레드 방식이 아니기 때문에
    //하나의 일감을 다 처리한 후에 queue에서 일감을 빼와서 일 처리를 한다
    public override void OnConnected(EndPoint endPoint)
    {
        Debug.Log($"OnConnected : {endPoint}");

        PacketManager.Instance.CustomHandler = (s, m, i) =>
        {
            PacketQueue.Instance.Push(this, i, m);
        };
    }

    public override void OnDisconnected(EndPoint endPoint)
    {
        Debug.Log($"OnDisconnected : {endPoint}");

        PacketQueue.Instance.Clear(this);
    }

    public override void OnRecvPacket(ArraySegment<byte> buffer)
    {
        PacketManager.Instance.OnRecvPacket(this, buffer);
    }

    public override void OnSend(int numOfBytes)
    {

    }
}
