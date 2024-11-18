﻿using GameServer;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PacketHandler
{
    public static void C_EnterWaitingRoomHandler(PacketSession session, IMessage packet)
    {
        C_EnterWaitingRoom enterWaitingRoomPacket = (C_EnterWaitingRoom)packet;
        ClientSession clientSession = (ClientSession)session;
        clientSession.HandleEnterWaitingRoom(enterWaitingRoomPacket);
    }

    public static void C_MakeRoomHandler(PacketSession session, IMessage packet)
    {
        //TODO : announce room info
        C_MakeRoom makeRoomPacket = (C_MakeRoom)packet;
        ClientSession clientSession = (ClientSession)session;
        clientSession.HandleMakeRoom(makeRoomPacket);
    }

    public static void C_EnterRoomHandler(PacketSession session, IMessage packet)
    {
        //TODO : announce room info
        C_EnterRoom enterRoomPacket = (C_EnterRoom)packet;
        ClientSession clientSession = (ClientSession)session;
        clientSession.HandleEnterRoom(enterRoomPacket);
    }

    public static void C_LeaveRoomHandler(PacketSession session, IMessage packet)
    {
        C_LeaveRoom enterRoomPacket = (C_LeaveRoom)packet;
        ClientSession clientSession = (ClientSession)session;
        clientSession.HandleLeaveRoom(enterRoomPacket);
    }

    public static void C_DestroyRoomHandler(PacketSession session, IMessage packet)
    {
        C_DestroyRoom destroyRoom = (C_DestroyRoom)packet;
        ClientSession clientSession = (ClientSession)session;
        clientSession.HandleDestroyRoom(destroyRoom);
    }

    public static void C_EnterGameHandler(PacketSession session, IMessage packet)
    {
        C_EnterGame enterGamePacket = (C_EnterGame)packet;
        ClientSession clientSession = (ClientSession)session;
        clientSession.HandleEnterGame(enterGamePacket);
    }

    public static void C_MoveHandler(PacketSession session, IMessage packet)
    {
        C_Move movePacket = (C_Move)packet;
        ClientSession clientSession = (ClientSession)session;

        Hero hero = clientSession.MyHero;
        if (hero == null)
            return;

        GameRoom room = hero.Room;
        if (room == null)
            return;

        room.Push(room.HandleMove, hero, movePacket);
    }
}
