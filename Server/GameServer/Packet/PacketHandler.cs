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

        if (hero != null)
            return;

        GameRoom room = hero.Room;
        if (room == null)
            return;

        room.Push(room.HandleMove, hero, movePacket);
    }
}
