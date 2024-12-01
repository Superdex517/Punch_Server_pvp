using GameServer;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PacketHandler
{
    public static void C_EnterLobbyHandler(PacketSession session, IMessage packet)
    {
        C_EnterLobby enterLobyPacket = (C_EnterLobby)packet;
        ClientSession clientSession = (ClientSession)session;
        clientSession.HandleEnterLobby(enterLobyPacket);
    }
    public static void C_MakeWaitingRoomHandler(PacketSession session, IMessage packet)
    {
        C_MakeWaitingRoom makeWaitingRoomPacket = (C_MakeWaitingRoom)packet;
        ClientSession clientSession = (ClientSession)session;
        clientSession.HandleMakeWaitingRoom(makeWaitingRoomPacket);
    }

    public static void C_EnterWaitingRoomHandler(PacketSession session, IMessage packet)
    {
        C_EnterWaitingRoom enterWaitingRoomPacket = (C_EnterWaitingRoom)packet;
        ClientSession clientSession = (ClientSession)session;
        clientSession.HandleEnterWaitingRoom(enterWaitingRoomPacket);
    }   

    public static void C_ReadyHandler(PacketSession session, IMessage packet)
    {
        C_Ready readyPacket = (C_Ready)packet;
        ClientSession clientSession = (ClientSession)session;
        clientSession.HandleReady(readyPacket);
    }  

    public static void C_GameStartHandler(PacketSession session, IMessage packet)
    {
        C_GameStart startPacket = (C_GameStart)packet;
        ClientSession clientSession = (ClientSession)session;
        clientSession.HandleStartGame(startPacket);
    }    

    public static void C_DestroyWaitingRoomHandler(PacketSession session, IMessage packet)
    {
        C_DestroyWaitingRoom destroyWaitingRoomPacket = (C_DestroyWaitingRoom)packet;
        ClientSession clientSession = (ClientSession)session;
        clientSession.HandleDestroyWaitingRoom(destroyWaitingRoomPacket);
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

        GameRoom room = hero.WaitingRoom.GameRoom;
        if (room == null)
            return;

        room.Push(room.HandleMove, hero, movePacket);
    }
}
